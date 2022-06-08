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

        /// <summary>
        /// In essence, how long the thread will sleep before trying to look for new messages from internal queue.
        /// </summary>
        [Description("How long to sleep on every iteration before consuming more messages (In seconds).")]
        public float SleepTime { get; set; } = 3f;

        /// <summary>
        /// How many characters per line are allowed. 
        /// </summary>
        [Description("Amount of characters per line to show")]
        public int CharacterLimit { get; set; } = 64;

        /// <summary>
        /// Amount of messages to show both to console, and hint.
        /// </summary>
        [Description("Amount of lines to show")]
        public int DisplayLimit { get; set; } = 15;

        /// <summary>
        /// How long messages live for both in console, and hint.
        /// </summary>
        [Description("How long to show the messages")]
        public float DisplayTimeLimit { get; set; } = 3f;

        /// <summary>
        /// What part of the screent to place the text (Always on bottom)
        /// </summary>
        [Description("Where to place text (Always on bottom)")]
        public LocationEnum TextPlacement { get; set; } = LocationEnum.Left;

        /// <summary>
        /// Gets or sets what types of end round outputs should be shown.
        /// </summary>
        [Description("Chat colors instance")]
        public ChatColors AssociatedChatColors { get; set; } = new ChatColors();

        /// <summary>
        /// Text size, based on http://digitalnativestudios.com/textmeshpro/docs/rich-text/#color
        /// </summary>
        [Description("Size of the text to show")]
        public string SizeOfHintText { get; set; } = "<size=50%>";

        /// <summary>
        /// Whether to allow certain chat's to be enabled/disabled.
        /// </summary>

        [Description("Whether to allow type of messages, if not specified then it will be ignored and commands for it rejected.")]
        public HashSet<MessageType> MsgTypesAllowed { get; set; } = new HashSet<MessageType>() { MessageType.GLOBAL, MessageType.LOCAL, MessageType.PRIVATE, MessageType.TEAM };

        /// <summary>
        /// Disables or enables sending to client console
        /// </summary>
        [Description("Whether to send the messages to console")]
        public bool SendToConsole { get; set; } = false;

        /// <summary>
        /// Disables or enables sending to client hint system
        /// </summary>
        [Description("Whether to send the messages to hint system")]
        public bool SendToHintSystem { get; set; } = true;

        /// <summary>
        /// Thet type of colors to use for the hint system, console does not accept the same as far as I can tell.
        /// </summary>
        [Description("Chat colors")]
        public class ChatColors {

            /// <summary>
            /// Use hex for color type
            /// </summary>
            [Description("Global chat color - Use hex to assign the color.")]
            public string GlobalChatColor { get; set; } = "<color=#85C7F2> ";

            /// <summary>
            /// Use hex for color type
            /// </summary>
            [Description("Local chat color - Use hex to assign the color.")]
            public string LocalChatColor { get; set; } = "<color=#85C7F2> ";

            /// <summary>
            /// Use hex for color type
            /// </summary>
            [Description("Private chat color - Use hex to assign the color.")]
            public string PrivateChatColor { get; set; } = "<color=#ADD7F6> ";

            /// <summary>
            /// Use hex for color type
            /// </summary>
            [Description("Team chat color - Use hex to assign the color.")]
            public string TeamChatColor { get; set; } = "<color=#3B28CC> ";



            /// <summary>
            /// Parses the color type
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
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
