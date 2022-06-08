using ChatManagerUtility.Configs;
using ChatManagerUtility.Events;
using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagerUtility.Commands
{
    public delegate void ChatLimitEventHandler(ChatLimitEventArgs ev);

    [CommandHandler(typeof(ClientCommandHandler))]
    class ChatLimitMessaging : ICommand
    {
        public string Command { get; } = "ChatLimit";

        public string[] Aliases { get; } = { "cl", "chatl" };

        public string Description { get; } = "ChatLimit Utility";

        public static event ChatLimitEventHandler IncomingChatLimitMessage;

        /// <summary>
        /// Handles incoming messages from client console for individuals to subscribe/unsubscribe from streams (channels).
        /// </summary>
        /// <returns></returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(arguments.Count == 0){
                response = "You must provide one parameter modify subscription, Options are: global, local, private, team ";
                return false;
            }
            Player player = Player.Get(sender);
            if(Enum.TryParse(arguments.At(0).ToUpper(), out MessageType channel)){
                IncomingChatLimitMessage?.Invoke(new ChatLimitEventArgs(arguments.At(0), player, channel));
                response = "Message has been accepted";
                return true;
            }
            response = $"Channel to change subscription was specified incorrectly: {arguments.At(0)}. Options are: GLOBAL, LOCAL, PRIVATE, TEAM";
            return false;
           
        }
    }
}
