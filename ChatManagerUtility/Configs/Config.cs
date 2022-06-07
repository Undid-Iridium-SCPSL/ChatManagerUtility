using ChatManagerUtility.Configs;
using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagerUtility
{
    public class Config : IConfig
    {
        /// <inheritdoc />
        [Description("Whether to enabled or disable plugin")]
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        [Description("Whether to enabled/disable debug")]
        public bool IsDebugEnabled { get; set; } = false;

        [Description("How long to sleep on every iteration before consuming more messages (In seconds).")]
        public float SleepTime { get; set; } = 3f;

        /// <summary>
        /// 
        /// </summary>
        [Description("Amount of characters per line to show")]
        public int CharacterLimit { get; set; } = 64;

        [Description("Amount of lines to show")]
        public int DisplayLimit { get; set; } = 15;

        [Description("How long to show the messages")]
        public float DisplayTimeLimit { get; set; } = 3f;

        [Description("Where to place text")]
        public LocationEnum TextPlacement { get; set; } = LocationEnum.Left;

        /// <summary>
        /// Gets or sets what types of end round outputs should be shown.
        /// </summary>
        [Description("Chat colors instance")]
        public ChatColors AssociatedChatColors { get; set; } = new ChatColors();

        [Description("Size of the text to show")]
        public string SizeOfHintText { get; set; } = "<size=50%>";

        [Description("Whether to allow type of messages, if not specified then it will be ignored and commands for it rejected.")]
        public HashSet<MessageType> MsgTypesAllowed { get; set; } = new HashSet<MessageType>() { MessageType.GLOBAL, MessageType.LOCAL, MessageType.PRIVATE, MessageType.TEAM };

        [Description("Chat colors")]
        public class ChatColors {

            [Description("Global chat color - Use hex to assign the color.")]
            public string GlobalChatColor { get; set; } = "<color=#85C7F2> ";

            [Description("Local chat color - Use hex to assign the color.")]
            public string LocalChatColor { get; set; } = "<color=#85C7F2> ";

            [Description("Private chat color - Use hex to assign the color.")]
            public string PrivateChatColor { get; set; } = "<color=#ADD7F6> ";

            [Description("Team chat color - Use hex to assign the color.")]
            public string TeamChatColor { get; set; } = "<color=#3B28CC> ";



            internal string ParseColor(MessageType type){
                switch(type){
                    case MessageType.GLOBAL:
                        return GlobalChatColor;
                    case MessageType.LOCAL:
                        return LocalChatColor;
                    case MessageType.PRIVATE:
                        return PrivateChatColor;
                    case MessageType.TEAM:
                        return TeamChatColor;
                    default:
                        return "<color=#4C4C4C> ";
                }
            }
        }
    }
}
