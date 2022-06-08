using ChatManagerUtility.Commands;
using ChatManagerUtility.Configs;
using ChatManagerUtility.Events;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static ChatManagerUtility.Config;

namespace ChatManagerUtility
{
    public class ChatManagerUpdater
    {

        /// <summary>
        /// Whether this <see cref="ChatManagerUpdater"/> is running
        /// </summary>
        public bool isRunning;

        /// <summary>
        /// Internal running thread
        /// </summary>
        public Thread currentRunningThread;

        /// <summary>
        /// Cancellation token used to identify when to stop thread
        /// </summary>
        public CancellationTokenSource cancellationToken;

        /// <summary>
        /// All the possible messages incoming to the system
        /// </summary>
        public Queue<MessageTypeHandler> AllIncomingMessages { get; set; }

        /// <summary>
        /// Current queue of messages to display
        /// </summary>
        public Queue<MessageTypeHandler> MessagesToDisplay { get; set; }

        /// <summary>
        /// How long the internal thread should sleep for
        /// </summary>
        private int SleepTime { get; set; } = 3500;

        /// <summary>
        /// How many characters are allowed per line
        /// </summary>
        private int CharacterLimit { get; set; }

        /// <summary>
        /// Current player
        /// </summary>
        private Player player { get; set; }

        /// <summary>
        /// Console generated message - built in conjunction with <see cref="ConsoleMsgToSend"/>
        /// </summary>
        private StringBuilder MsgToSend { get; set; }

        /// <summary>
        /// Console generated message - built in conjunction with <see cref="MsgToSend"/>
        /// </summary>
        public StringBuilder ConsoleMsgToSend { get; private set; }

        /// <summary>
        /// How many messages are allowed to be displayed
        /// </summary>
        private int DisplayableLimit { get; set; }

        /// <summary>
        /// How long messages display for
        /// </summary>
        private int DisplayTimeLimit { get; set; }

        /// <summary>
        /// What size of screen to place text
        /// </summary>
        private LocationEnum TextPlacement { get; set; }

        /// <summary>
        /// Whether to send the data to the client console
        /// </summary>
        public bool SendToClientConsole { get; set; }

        /// <summary>
        /// Whether to send the data to the client hint system
        /// </summary>
        public bool SendToClientHintSystem { get; set; }

        /// <summary>
        /// What chat streams that are currently allowed
        /// </summary>
        private HashSet<MessageType> ConsumerTypes { get; set; }

        /// <summary>
        /// Size of hint text
        /// </summary>
        private String SizeOfText { get; set; }

        /// <summary>
        /// Current allocated chat colors on chat type
        /// </summary>
        readonly ChatColors chatColors;

        /// <summary>
        /// Initalizes several event handlers, and configuration per class. In addition, generates a thread that uses this class
        /// to handle receiving and parsing messages from users.
        /// </summary>
        /// <param name="currentPlayer"></param>
        public ChatManagerUpdater(Player currentPlayer)
        {
            //CancellationTokenSource cts = new CancellationTokenSource();
            //ThreadPool.QueueUserWorkItem(new WaitCallback(ChatManagerParser), cts.Token);
            Log.Debug($"ChatManagerUpdater loaded in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            cancellationToken = new CancellationTokenSource();
            isRunning = true;



            GlobalMessaging.IncomingGlobalMessage += this.ConsumeGlobalMessage;
            LocalMessaging.IncomingLocalMessage += this.ConsumeLocalMessage;
            PrivateMessaging.IncomingPrivateMessage += this.ConsumePrivateMessage;
            TeamMessaging.IncomingTeamMessage += this.ConsumeTeamMessage;
            ChatLimitMessaging.IncomingChatLimitMessage += this.ConsumeChatLimit;
            ConsumerTypes = new HashSet<MessageType> { MessageType.GLOBAL, MessageType.LOCAL, MessageType.PRIVATE, MessageType.TEAM };


            
            AllIncomingMessages = new Queue<MessageTypeHandler>();
            MessagesToDisplay = new Queue<MessageTypeHandler>();
            SleepTime = (int)(ChatManagerUtilityMain.Instance.Config.SleepTime * 1000);
            CharacterLimit = ChatManagerUtilityMain.Instance.Config.CharacterLimit;

            player = currentPlayer;
            MsgToSend = new StringBuilder();
            ConsoleMsgToSend = new StringBuilder();
            DisplayableLimit = ChatManagerUtilityMain.Instance.Config.DisplayLimit;
            DisplayTimeLimit = (int)(ChatManagerUtilityMain.Instance.Config.DisplayTimeLimit);
            TextPlacement = ChatManagerUtilityMain.Instance.Config.TextPlacement;

            SendToClientConsole = ChatManagerUtilityMain.Instance.Config.SendToConsole;
            SendToClientHintSystem = ChatManagerUtilityMain.Instance.Config.SendToHintSystem;

            SizeOfText = ChatManagerUtilityMain.Instance.Config.SizeOfHintText;

            switch (TextPlacement) {
                case LocationEnum.Center:
                    MsgToSend.Append("<align=\"center\">");
                    break;
                case LocationEnum.Right:
                    MsgToSend.Append("<align=\"right\">");
                    break;
                case LocationEnum.Left:
                default:
                    MsgToSend.Append("<align=\"left\">");
                    break;
            }

            chatColors = ChatManagerUtilityMain.Instance.Config.AssociatedChatColors;
            Log.Debug($"ChatManagerUpdater Finished loading in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);

            currentRunningThread = new Thread(() => ChatManager(cancellationToken.Token, this));
            currentRunningThread.Start();

        }

        /// <summary>
        /// Used to subscribe/unsubscribe from specific channel events
        /// </summary>
        /// <param name="ev"></param>
        private void ConsumeChatLimit(ChatLimitEventArgs ev)
        {
            Log.Debug($"ConsumeGlobalMessage loaded in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);

            if(ev.GetPlayer() != this.player){
                return;
            }

            if (ConsumerTypes.Contains(ev.ChannelToChangeSubscription))
            {
                switch (ev.ChannelToChangeSubscription)
                {
                    case MessageType.GLOBAL:
                        GlobalMessaging.IncomingGlobalMessage -= this.ConsumeGlobalMessage;
                        break;
                    case MessageType.LOCAL:
                        LocalMessaging.IncomingLocalMessage -= this.ConsumeLocalMessage;
                        break;
                    case MessageType.TEAM:
                        PrivateMessaging.IncomingPrivateMessage -= this.ConsumePrivateMessage;
                        break;
                    case MessageType.PRIVATE:
                        TeamMessaging.IncomingTeamMessage -= this.ConsumeTeamMessage;
                        break;
                }
                ConsumerTypes.Remove(ev.ChannelToChangeSubscription);
            }
            else{
                switch (ev.ChannelToChangeSubscription)
                {
                    case MessageType.GLOBAL:
                        GlobalMessaging.IncomingGlobalMessage += this.ConsumeGlobalMessage;
                        break;
                    case MessageType.LOCAL:
                        LocalMessaging.IncomingLocalMessage += this.ConsumeLocalMessage;
                        break;
                    case MessageType.TEAM:
                        PrivateMessaging.IncomingPrivateMessage += this.ConsumePrivateMessage;
                        break;
                    case MessageType.PRIVATE:
                        TeamMessaging.IncomingTeamMessage += this.ConsumeTeamMessage;
                        break;
                }
                ConsumerTypes.Add(ev.ChannelToChangeSubscription);
            }
            Log.Debug($"ConsumeGlobalMessage Finished loading in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
        }

        /// <summary>
        /// Event handler to consume messages from <see cref="GlobalMessaging"/>
        /// </summary>
        /// <param name="ev"> Current event </param>
        private void ConsumeGlobalMessage(GlobalMsgEventArgs ev)
        {
            Log.Debug($"ConsumeGlobalMessage loaded in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            AllIncomingMessages.Enqueue(new MessageTypeHandler(ev.GetMessage(), Time.time, CharacterLimit, ev.GetMsgType(), chatColors.ParseColor(ev.GetMsgType()), SizeOfText));
            Log.Debug($"ConsumeGlobalMessage Finished loading in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
        }
        /// <summary>
        /// Event handler to consume messages from <see cref="LocalMessaging"/>
        /// </summary>
        /// <param name="ev"> Current event </param>
        private void ConsumeLocalMessage(LocalMsgEventArgs ev)
        {
            Log.Debug($"ConsumeLocalMessage loaded in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            
            Player eventPlayer = ev.GetPlayer();

            float num2 = Vector3.Distance(eventPlayer.ReferenceHub.transform.position, this.player.ReferenceHub.transform.position);
            if (num2 <= 9f)
            {
                AllIncomingMessages.Enqueue(new MessageTypeHandler(ev.GetMessage(), Time.time, CharacterLimit, ev.GetMsgType(), chatColors.ParseColor(ev.GetMsgType()), SizeOfText));
            }
            Log.Debug($"ConsumeLocalMessage Finished loading in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
        }

        /// <summary>
        /// Event handler to consume messages from <see cref="PrivateMessaging"/>
        /// </summary>
        /// <param name="ev"> Current event </param>
        private void ConsumePrivateMessage(PrivateMsgEventArgs ev)
        {
            Log.Debug($"ConsumePrivateMessage loaded in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            if(ev.PlayerToMsg() != this.player){
                return;
            }
            AllIncomingMessages.Enqueue(new MessageTypeHandler(ev.GetMessage(), Time.time, CharacterLimit, ev.GetMsgType(), chatColors.ParseColor(ev.GetMsgType()), SizeOfText));
            Log.Debug($"ConsumePrivateMessage Finished loading in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
        }

        /// <summary>
        /// Event handler to consume messages from <see cref="TeamMessaging"/>
        /// </summary>
        /// <param name="ev"> Current event </param>
        private void ConsumeTeamMessage(TeamMsgEventArgs ev)
        {
            Log.Debug($"ConsumeTeamMessage loaded in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            if (ev.GetPlayer().Role.Side != this.player.Role.Side)
            {
                return;
            }
            AllIncomingMessages.Enqueue(new MessageTypeHandler(ev.GetMessage(), Time.time, CharacterLimit, ev.GetMsgType(), chatColors.ParseColor(ev.GetMsgType()), SizeOfText));
            Log.Debug($"ConsumeTeamMessage Finished loading in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
        }

        /// <summary>
        /// Thread loop that handles processing the queue'd message data
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="operatingClass"></param>
        private static void ChatManager(object obj, ChatManagerUpdater operatingClass)
        {
            Log.Debug($"ChatManager loaded in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            CancellationToken token = (CancellationToken)obj;

            while (ChatManagerCore.isRunning)
            {
                Log.Debug($"ChatManager Finished loading in, in loop", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
                if (token.IsCancellationRequested)
                {
                    Log.Debug($"ChatManager, Cancellation received.", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
                    return;
                }

                operatingClass.ProcessQueue();

                Thread.Sleep(operatingClass.SleepTime);
                Log.Debug($"ChatManager, After sleep.", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            }
        }

        /// <summary>
        /// Sends data both to client console, and client hint system.
        /// </summary>
        public void SendMessage()
        {
            //this.player.hint.add(msg);
            try
            {
                this.player = Player.Get(this.player.Id);
                MsgToSend.Append("</align>");
                Log.Debug($"SendMessage entered, msg to send {MsgToSend} and Player {player is null}, Nickname {player?.Nickname}", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
                //this.player.ShowHint("<line-height=75%><voffset=30em><align=center><color=#247BA0> hi </color> asdasdasd </align> </voffset>", 10);
                if (SendToClientHintSystem)
                {
                    this.player.ShowHint(MsgToSend.ToString());
                }

                //this.player.ReferenceHub.queryProcessor.TargetReplyPlain(this.player.ReferenceHub.queryProcessor.connectionToClient, MsgToSend.ToString(), true, true, string.Empty);
                //this.player.ReferenceHub.queryProcessor.TargetReply(this.player.ReferenceHub.queryProcessor.connectionToClient, MsgToSend.ToString(), true, true, string.Empty);
                if (ConsoleMsgToSend.Length > 5 && SendToClientConsole)
                {
                    this.player.ReferenceHub.queryProcessor.GCT.SendToClient(this.player.ReferenceHub.queryProcessor.connectionToClient, ConsoleMsgToSend.ToString(), "green");
                }
                MsgToSend.Clear();
                ConsoleMsgToSend.Clear();
                ConsoleMsgToSend.Append("\n");

                switch (TextPlacement)
                {
                    case LocationEnum.Center:
                        MsgToSend.Append("<align=\"center\">");
                        break;
                    case LocationEnum.Right:
                        MsgToSend.Append("<align=\"right\">");
                        break;
                    case LocationEnum.Left:
                    default:
                        MsgToSend.Append("<align=\"left\">");
                        break;
                }
                Log.Debug($"SendMessage After send and clear.", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            }
            catch (Exception ex){
                Log.Debug($"Unable to send message {ex}", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            }
        }

        public void AppendSendableHint(string msg){
            Log.Debug($"AppendSendableHint entered, msg: {msg}", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            MsgToSend.Append(msg);
            Log.Debug($"AppendSendableHint finished.", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
        }

        private void ProcessQueue(){

            Log.Debug($"ProcessQueue entered, starting MessageCount loop AllIncomingMessages.Count {AllIncomingMessages.Count} vs MessagesToDisplay.Count {MessagesToDisplay.Count}", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            while (MessagesToDisplay.Count < DisplayableLimit)
            {
                if(AllIncomingMessages.IsEmpty()){
                    break;
                }
                MessagesToDisplay.Enqueue(AllIncomingMessages.Dequeue());
            }
            Log.Debug($"ProcessQueue finished first check if there were enough messages, how many msg's: {MessagesToDisplay.Count}.", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);

            Queue<MessageTypeHandler> messagesToRetain = new Queue<MessageTypeHandler>();
            while (!MessagesToDisplay.IsEmpty())
            {
                MessageTypeHandler currentMessageHandler = MessagesToDisplay.Dequeue();
                ConsoleMsgToSend.Append(currentMessageHandler.ConsoleMsg + "\n");
                AppendSendableHint(currentMessageHandler.Msg + "\n");
                Log.Debug($"Time.time {Time.time} vs {currentMessageHandler.MsgTime} equals {Time.time - currentMessageHandler.MsgTime} vs display limit {DisplayTimeLimit}");
                if (Time.time - currentMessageHandler.MsgTime < DisplayTimeLimit){

                    messagesToRetain.Enqueue(currentMessageHandler);
                   
                }
            }
            Log.Debug($"ProcessQueue, MessagesToDisplay iterated, and finished. How big was messagesToRetain {messagesToRetain.Count}", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            //while(messagesToRetain.Count > 0)
            //{
            //    Log.Debug("Retaining message iteratively!!!");
            //    MessagesToDisplay.Enqueue(messagesToRetain.Dequeue());
            //}
            MessagesToDisplay = messagesToRetain;
            SendMessage();
            Log.Debug($"ProcessQueue finished. Size of queue {MessagesToDisplay.Count}", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
        }

        internal void Shutdown()
        {
            Log.Debug($"shutdown for ChatManagerUpdater called.", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            this.cancellationToken.Cancel();
        }
    }
}
