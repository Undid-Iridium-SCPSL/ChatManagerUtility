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
    public delegate void PrivateMsgEventHandler(PrivateMsgEventArgs ev);

    //https://stackoverflow.com/questions/623451/how-can-i-make-my-own-event-in-c
    [CommandHandler(typeof(ClientCommandHandler))]
    class PrivateMessaging : ICommand
    {
        public string Command { get; } = "PrivateMessaging";

        public string[] Aliases { get; } = { "p", "private" };

        public string Description { get; } = "PrivateMessaging Utility";

        public static event PrivateMsgEventHandler IncomingPrivateMessage;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "You must provide a message to send";
                return false;
            }

        

            try{
                StringBuilder sb = new StringBuilder();
                for (int pos = 1; pos < arguments.Count; pos++)
                {
                    sb.Append(arguments.At(pos));
                    sb.Append(" ");
                }
                Player player = Player.Get(sender);
                IncomingPrivateMessage?.Invoke(new PrivateMsgEventArgs($"[P][{player.Nickname}]:" + sb.ToString(), player, Player.Get(arguments.At(0))));
                response = "Assume it was good";
                return true;
            }
            catch (Exception ex){
                response = $"Unable to send TeamMessaging because of {ex}";
            }
            return false;
        }
    }
}
