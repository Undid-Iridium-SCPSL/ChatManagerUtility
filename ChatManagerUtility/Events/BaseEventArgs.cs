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

        public BaseEventArgs(string Text)
        {
            EventMessage = Text;
        }
        public string GetMessage()
        {
            return EventMessage;
        }

    }
}
