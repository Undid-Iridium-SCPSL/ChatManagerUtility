using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagerUtility.Events
{
    public class BaseEventArgs : EventArgs
    {
        private string EventMessage;
        private Player curPlayer;

        public BaseEventArgs(string Text)
        {
            EventMessage = Text;
        }

        public BaseEventArgs(string Text, Player curPlayer)
        {
            this.EventMessage = Text;
            this.curPlayer = curPlayer;
        }

        public BaseEventArgs(Player curPlayer)
        {
            this.curPlayer = curPlayer;
        }

        public Player GetPlayer()
        {
            return curPlayer;
        }

        public string GetMessage()
        {
            return EventMessage;
        }

    }
}
