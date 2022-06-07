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

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (IncomingGlobalMessage != null){
                IncomingGlobalMessage(new GlobalMsgEventArgs("[G]:" + arguments.At(0)));
            }
            response = "Assume it was good";
            return true;
        }
    }
}
