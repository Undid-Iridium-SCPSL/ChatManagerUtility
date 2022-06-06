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

        public bool isRunning;

        public Thread currentRunningThread;

        public CancellationTokenSource cancellationToken;

        public Queue<MessageTypeHandler> AllIncomingMessages;

        public Queue<MessageTypeHandler> MessagesToDisplay;

        private int SleepTime { get; set; } = 3500;

        private int CharacterLimit { get; set; }

        private Player player { get; set; }

        private StringBuilder MsgToSend { get; set; }

        private int DisplayableLimit { get; set; }

        private int DisplayTimeLimit { get; set; }

        private LocationEnum TextPlacement { get; set; }

        readonly ChatColors chatColors;

        public ChatManagerUpdater(Player currentPlayer)
        {
            //CancellationTokenSource cts = new CancellationTokenSource();
            //ThreadPool.QueueUserWorkItem(new WaitCallback(ChatManagerParser), cts.Token);
            cancellationToken = new CancellationTokenSource();
            currentRunningThread = new Thread(() => ChatManager(cancellationToken, this));
            isRunning = true;
            GlobalMessaging.IncomingGlobalMessage += this.ConsumeGlobalMessage;
            LocalMessaging.IncomingLocalMessage += this.ConsumeLocalMessage;
            PrivateMessaging.IncomingPrivateMessage += this.ConsumePrivateMessage;
            TeamMessaging.IncomingTeamMessage += this.ConsumeTeamMessage;

            AllIncomingMessages = new Queue<MessageTypeHandler>();
            MessagesToDisplay = new Queue<MessageTypeHandler>();
            SleepTime = (int)(ChatManagerUtilityMain.Instance.Config.SleepTime * 1000);
            CharacterLimit = ChatManagerUtilityMain.Instance.Config.CharacterLimit;
            player = currentPlayer;
            MsgToSend = new StringBuilder();
            DisplayableLimit = ChatManagerUtilityMain.Instance.Config.DisplayLimit;
            DisplayTimeLimit = (int)(ChatManagerUtilityMain.Instance.Config.DisplayTimeLimit * 1000);
            TextPlacement = ChatManagerUtilityMain.Instance.Config.TextPlacement;

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

        }

        private void ConsumeGlobalMessage(GlobalMsgEventArgs ev)
        {
            AllIncomingMessages.Append(new MessageTypeHandler(ev.GetMessage(), Time.time, CharacterLimit, ev.GetMsgType(), chatColors.ParseColor(ev.GetMsgType())));
        }
        private void ConsumeLocalMessage(LocalMsgEventArgs ev)
        {
            AllIncomingMessages.Append(new MessageTypeHandler(ev.GetMessage(), Time.time, CharacterLimit, ev.GetMsgType(), chatColors.ParseColor(ev.GetMsgType())));
        }

        private void ConsumePrivateMessage(PrivateMsgEventArgs ev)
        {
            AllIncomingMessages.Append(new MessageTypeHandler(ev.GetMessage(), Time.time, CharacterLimit, ev.GetMsgType(), chatColors.ParseColor(ev.GetMsgType())));
        }
        private void ConsumeTeamMessage(TeamMsgEventArgs ev)
        {
            AllIncomingMessages.Append(new MessageTypeHandler(ev.GetMessage(), Time.time, CharacterLimit, ev.GetMsgType(), chatColors.ParseColor(ev.GetMsgType())));
        }


        private static void ChatManager(object obj, ChatManagerUpdater operatingClass)
        {
            CancellationToken token = (CancellationToken)obj;
            while (ChatManagerCore.isRunning)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                operatingClass.ProcessQueue();

                Thread.Sleep(operatingClass.SleepTime);

            }
        }

        public void SendMessage()
        {
            //this.player.hint.add(msg);
            this.player.ShowHint(MsgToSend.ToString());
            MsgToSend.Clear();
        }

        public void AppendSendableHint(string msg){
            MsgToSend.Append(msg);
        }

        private void ProcessQueue(){
            
            while(MessagesToDisplay.Count < DisplayableLimit)
            {
                if(AllIncomingMessages.IsEmpty()){
                    break;
                }
                MessagesToDisplay.Enqueue(AllIncomingMessages.Dequeue());
            }
            Queue<MessageTypeHandler> messagesToRetain = new Queue<MessageTypeHandler>();
            while (!MessagesToDisplay.IsEmpty())
            {
                MessageTypeHandler currentMessageHandler = MessagesToDisplay.Dequeue();
                AppendSendableHint(currentMessageHandler.Msg);
                if (Time.time - currentMessageHandler.MsgTime < DisplayTimeLimit){
                    messagesToRetain.Append(currentMessageHandler);
                }
                
            }
            MessagesToDisplay = messagesToRetain;
            SendMessage();
        }

        internal void shutdown()
        {
            this.cancellationToken.Cancel();
        }
    }
}
