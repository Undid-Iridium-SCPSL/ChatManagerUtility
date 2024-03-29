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
    public delegate void PrivateMsgEventHandler(PrivateMsgEventArgs ev);

    //https://stackoverflow.com/questions/623451/how-can-i-make-my-own-event-in-c
    [CommandHandler(typeof(ClientCommandHandler))]
    class PrivateMessaging : ICommand
    {
        public string Command { get; } = "PrivateMessaging";

        public string[] Aliases { get; } = { "p", "private" };

        public string Description { get; } = "PrivateMessaging Utility";

        public static event PrivateMsgEventHandler IncomingPrivateMessage;

        /// <summary>
        /// Handles incoming messages from client console for private messages. 
        /// </summary>
        /// <returns></returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!ChatManagerUtilityMain.Instance.Config.MsgTypesAllowed.Contains(Configs.MessageType.PRIVATE))
            {
                response = "This has been disabled by an administrator. Contact them to enable private chat.";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "You must provide a message to send";
                return false;
            }

            try{
                Player player = Player.Get(sender);
                Player targetPlayer = Player.Get(arguments.At(0));
                if (player.Role.Type is RoleType.Spectator && targetPlayer.Role.Type != RoleType.Spectator)
                {
                    response = "Private Message cannot be sent while in spectator mode to a non-spectator Player.";
                    return false;
                }

                StringBuilder sb = new StringBuilder();
                for (int pos = 1; pos < arguments.Count; pos++)
                {
                    sb.Append(arguments.At(pos));
                    sb.Append(" ");
                }
                
                IncomingPrivateMessage?.Invoke(new PrivateMsgEventArgs($"[P][{player.Nickname}]:" + sb.ToString(), player, targetPlayer));
                response = "Private Message has been accepted";
                return true;
            }
            catch (Exception ex){
                response = $"Unable to send PrivateMessaging because of {ex}";
            }
            return false;
        }
    }
}
