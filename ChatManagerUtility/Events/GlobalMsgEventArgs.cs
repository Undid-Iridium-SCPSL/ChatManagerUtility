﻿using ChatManagerUtility.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagerUtility.Events
{
    //This is a class which describes the event to the class that recieves it.
    //An EventArgs class must always derive from System.EventArgs.
    public class GlobalMsgEventArgs : BaseEventArgs
    {
        private MessageType MessageType;
        public GlobalMsgEventArgs(string Text, Exiled.API.Features.Player player) : base(Text, player)
        {
            MessageType = MessageType.GLOBAL;
        }
        public MessageType GetMsgType()
        {
            return MessageType;
        }
    }
}
