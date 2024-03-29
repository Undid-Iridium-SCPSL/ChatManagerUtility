﻿using ChatManagerUtility.Events;
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
    public delegate void TeamMsgEventHandler(TeamMsgEventArgs ev);

    //https://stackoverflow.com/questions/623451/how-can-i-make-my-own-event-in-c
    [CommandHandler(typeof(ClientCommandHandler))]
    class TeamMessaging : ICommand
    {
        public string Command { get; } = "TeamMessaging";

        public string[] Aliases { get; } = { "t", "team" };

        public string Description { get; } = "TeamMessaging Utility";

        public static event TeamMsgEventHandler IncomingTeamMessage;

        /// <summary>
        /// Handles incoming messages from client console for Team messages. 
        /// </summary>
        /// <returns></returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (!ChatManagerUtilityMain.Instance.Config.MsgTypesAllowed.Contains(Configs.MessageType.TEAM))
            {
                response = "This has been disabled by an administrator. Contact them to enable team chat.";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "You must provide a message to send";
                return false;
            }

            try
            {
                Player player = Player.Get(sender);
                if (player.Role.Type is RoleType.Spectator)
                {
                    response = "Local Message cannot be sent while in spectator mode.";
                    return false;
                }
                String nameToShow = player.Nickname.Length < 6 ? player.Nickname : player.Nickname.Substring(0, (player.Nickname.Length / 3) + 1);
                IncomingTeamMessage?.Invoke(new TeamMsgEventArgs($"[T][{nameToShow}]:" + String.Join(" ", arguments.ToList()), player));
                response = "Team Message has been processed.";
                return true;
            }
            catch (Exception ex){
                response = $"Unable to send TeamMessaging because of {ex}";
            }
            return false;
        }
    }
}
