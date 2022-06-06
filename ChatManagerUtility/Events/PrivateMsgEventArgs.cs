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
    public class PrivateMsgEventArgs : BaseEventArgs
    {
        private MessageType MessageType;
        public PrivateMsgEventArgs(string Text) : base(Text)
        {
            MessageType = MessageType.Private;
        }
        public MessageType GetMsgType()
        {
            return MessageType;
        }
    }
}
