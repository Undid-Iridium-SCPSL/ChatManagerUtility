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
            
            if(!ChatManagerUtilityMain.Instance.Config.MsgTypesAllowed.Contains(Configs.MessageType.LOCAL))
            {
                response = "This has been disabled by an administrator. Contact them to enable local chat.";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "You must provide a message to send";
                return false;
            }

           

            try{ 
                Player player = Player.Get(sender);
                if(player.Role.Type is RoleType.Spectator){
                    response = "Local Message cannot be sent while in spectator mode.";
                    return false;
                }
                String nameToShow = player.Nickname.Length < 6 ? player.Nickname : player.Nickname.Substring(0, (player.Nickname.Length / 3) + 1);
                IncomingLocalMessage?.Invoke(new LocalMsgEventArgs($"[L][{nameToShow}]:" + String.Join(" ", arguments.ToList()), player));
                response = "Local Message has been accepted";
                return true;
            }
            catch (Exception ex){
                response = $"Unable to send LocalMessaging because of {ex}";
            }
            return false;
        }
    }
}
