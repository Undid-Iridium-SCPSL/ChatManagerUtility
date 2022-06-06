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
        [Description("How long to sleep on every iteration before consuming more messages (In seconds).")]
        public float SleepTime { get; set; } = 3f;

        /// <inheritdoc />
        [Description("Whether to enabled or disable plugin")]
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        [Description("Whether to enabled/disable debug")]
        public bool IsDebugEnabled { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        [Description("Amount of characters per line to show")]
        public int CharacterLimit { get; set; } = 128;

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


        [Description("Chat colors")]
        public class ChatColors {

            [Description("Global chat color - Use hex to assign the color.")]
            public string GlobalChatColor { get; set; } = "#0D0630";

            [Description("Local chat color - Use hex to assign the color.")]
            public string LocalChatColor { get; set; } = "#18314F";

            [Description("Private chat color - Use hex to assign the color.")]
            public string PrivateChatColor { get; set; } = "#B56B45";

            [Description("Team chat color - Use hex to assign the color.")]
            public string TeamChatColor { get; set; } = "#8BBEB2";



            internal string ParseColor(MessageType type){
                switch(type){
                    case MessageType.Global:
                        return GlobalChatColor;
                    case MessageType.Local:
                        return LocalChatColor;
                    case MessageType.Private:
                        return PrivateChatColor;
                    case MessageType.Team:
                        return TeamChatColor;
                    default:
                        return "#4C4C4C";
                }
            }
        }
    }
}
