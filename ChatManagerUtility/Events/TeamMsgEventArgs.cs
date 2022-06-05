using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagerUtility.Events
{
    //This is a class which describes the event to the class that recieves it.
    //An EventArgs class must always derive from System.EventArgs.
    public class TeamMsgEventArgs : EventArgs
    {
        public string EventMessage;
        public TeamMsgEventArgs(string Text)
        {
            EventMessage = Text;
        }
        public string GetInfo()
        {
            return EventMessage;
        }
    }
}
