using ChatManagerUtility.Configs;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagerUtility.Events
{
    //This is a class which describes the event to the class that recieves it.
    //An EventArgs class must always derive from System.EventArgs.
    public class PrivateMsgEventArgs : BaseEventArgs
    {
        private MessageType MessageType;
        private Player playerToMsg;
        public PrivateMsgEventArgs(string Text, Player currentPlayer, Player playerToMessage) : base(Text, currentPlayer)
        {
            MessageType = MessageType.PRIVATE;
            playerToMsg = playerToMessage;
        }

        public Player PlayerToMsg()
        {
            return playerToMsg;
        }

        public MessageType GetMsgType()
        {
            return MessageType;
        }
    }
}
