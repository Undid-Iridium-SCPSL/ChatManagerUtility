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
    public delegate void TeamMsgEventHandler(TeamMsgEventArgs ev);

    //https://stackoverflow.com/questions/623451/how-can-i-make-my-own-event-in-c
    [CommandHandler(typeof(ClientCommandHandler))]
    class TeamMessaging : ICommand
    {
        public string Command { get; } = "TeamMessaging";

        public string[] Aliases { get; } = { "t", "team" };

        public string Description { get; } = "TeamMessaging Utility";

        public static event TeamMsgEventHandler IncomingTeamMessage;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(IncomingTeamMessage != null){
                IncomingTeamMessage(new TeamMsgEventArgs(arguments.At(0)));
            }
            response = "Assume it was good";
            return true;
        }
    }
}
