using Exiled.Events;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatManagerUtility
{
    public class ChatManagerCore
    {
        /// <summary>
        /// Whether this application is running or not - should be cleaned up when round ends/plugin disabled
        /// </summary>
        static bool isRunning;

        /// <summary>
        /// 
        /// </summary>
        public ChatManagerCore(){
            isRunning = true;
        }

        /// <summary>
        /// 
        /// </summary>
        ~ChatManagerCore()
        {
            isRunning = false;
        }

        private static void ChatManagerParser(object obj){
            while(ChatManagerCore.isRunning){
                if()
            }
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            if(ev.Player != null){
                CancellationTokenSource cts = new CancellationTokenSource();
                ThreadPool.QueueUserWorkItem(new WaitCallback(ChatManagerParser), cts.Token);
                //Thread thread = new Thread(new ThreadStart(ChatManagerParser));
                ev.Player.SessionVariables.Add("ChatManagerThreadToken", cts);
            }
        }

        public void OnLeft(LeftEventArgs ev)
        {
            if (ev.Player != null)
            {
                ev.Player.SessionVariables.Remove("ChatManagerEnabled");
            }
        }
    }
}
