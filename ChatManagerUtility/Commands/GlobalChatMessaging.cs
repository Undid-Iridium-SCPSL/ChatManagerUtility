using ChatManagerUtility.Events;
using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagerUtility
{
    //First we have to define a delegate that acts as a signature for the
    //function that is ultimately called when the event is triggered.
    //You will notice that the second parameter is of MyEventArgs type.
    //This object will contain information about the triggered event.
    public delegate void GlobalMsgEventHandler(GlobalMsgEventArgs ev);

    //https://stackoverflow.com/questions/623451/how-can-i-make-my-own-event-in-c
    [CommandHandler(typeof(ClientCommandHandler))]
    class GlobalMessaging : ICommand
    {
        public string Command { get; } = "GlobalMessaging";

        public string[] Aliases { get; } = { "g", "global" };

        public string Description { get; } = "GlobalMessaging Utility";

        public static event GlobalMsgEventHandler IncomingGlobalMessage;

        /// <summary>
        /// Handles incoming messages from client console for Global messages. 
        /// </summary>
        /// <returns></returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {


            if (!ChatManagerUtilityMain.Instance.Config.MsgTypesAllowed.Contains(Configs.MessageType.GLOBAL))
            {
                response = "This has been disabled by an administrator. Contact them to enable global chat.";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "You must provide a message to send";
                return false;
            }

            try{ 
                Player player = Player.Get(sender);
                String nameToShow = player.Nickname.Length < 6 ? player.Nickname : player.Nickname.Substring(0, (player.Nickname.Length / 3) + 1);
                IncomingGlobalMessage?.Invoke(new GlobalMsgEventArgs($"[G][{nameToShow}]:" + String.Join(" ", arguments.ToList()), player));
                response = "Global Message has been accepted";
                return true;
            }
            catch (Exception ex){
                response = $"Unable to send Global Message because of {ex}";
            }
            return false;
        }
    }
}
