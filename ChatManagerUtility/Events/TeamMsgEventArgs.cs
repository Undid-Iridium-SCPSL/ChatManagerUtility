using ChatManagerUtility.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagerUtility.Events
{
    //This is a class which describes the event to the class that recieves it.
    //An EventArgs class must always derive from System.EventArgs.
    public class TeamMsgEventArgs : BaseEventArgs
    {
        private MessageType MessageType;
        public TeamMsgEventArgs(string Text, Exiled.API.Features.Player player) : base(Text, player)
        {
            MessageType = MessageType.TEAM;
        }
        public MessageType GetMsgType()
        {
            return MessageType;
        }
    }
}
