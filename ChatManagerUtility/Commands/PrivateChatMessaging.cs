using ChatManagerUtility.Events;
using CommandSystem;
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
            if(IncomingPrivateMessage != null){
                IncomingPrivateMessage(new PrivateMsgEventArgs("[P]:" + arguments.At(0)));
            }
            response = "Assume it was good";
            return true;
        }
    }
}
