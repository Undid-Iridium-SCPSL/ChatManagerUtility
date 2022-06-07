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
    public delegate void LocalMsgEventHandler(LocalMsgEventArgs ev);

    //https://stackoverflow.com/questions/623451/how-can-i-make-my-own-event-in-c
    [CommandHandler(typeof(ClientCommandHandler))]
    class LocalMessaging : ICommand
    {
        public string Command { get; } = "LocalMessaging";

        public string[] Aliases { get; } = { "l", "local" };

        public string Description { get; } = "LocalMessaging Utility";

        public static event LocalMsgEventHandler IncomingLocalMessage;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "You must provide a message to send";
                return false;
            }

            try{ 
                Player player = Player.Get(sender);
                String nameToShow = player.Nickname.Length < 6 ? player.Nickname : player.Nickname.Substring(0, (player.Nickname.Length / 3) + 1);
                IncomingLocalMessage?.Invoke(new LocalMsgEventArgs($"[L][{nameToShow}]:" + String.Join(" ", arguments.ToList()), player));
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
