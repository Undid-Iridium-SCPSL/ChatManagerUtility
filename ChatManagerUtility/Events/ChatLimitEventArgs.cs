using ChatManagerUtility.Configs;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagerUtility.Events
{
    public class ChatLimitEventArgs : BaseEventArgs
    {

        private String channelToChangeSubscription;

        private MessageType channelToChangeSub;
        public ChatLimitEventArgs(string channelToFlip, Player curPlayer, MessageType channel) : base(curPlayer)
        {
            channelToChangeSubscription = channelToFlip;
            channelToChangeSub = channel;
        }
        public MessageType ChannelToChangeSubscription { get { return channelToChangeSub;} }
        public String ChannelToFlip { get { return channelToChangeSubscription; } }
    }
    
}
