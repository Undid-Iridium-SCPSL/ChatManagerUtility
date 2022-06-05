using ChatManagerUtility.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatManagerUtility
{
    public class ChatManagerUpdater
    {

        public bool isRunning;

        public Thread currentRunningThread;

        public CancellationTokenSource cancellationToken;

        public Queue<MessageTypeHandler> AllIncomingMessages;

        public Queue<MessageTypeHandler> MessagesToDisplay;

        public ChatManagerUpdater(){
            //CancellationTokenSource cts = new CancellationTokenSource();
            //ThreadPool.QueueUserWorkItem(new WaitCallback(ChatManagerParser), cts.Token);
            cancellationToken = new CancellationTokenSource();
            currentRunningThread = new Thread(() => ChatManager(cancellationToken, this));
            isRunning = true;
            GlobalMessaging.IncomingGlobalMessage += this.consumeMessage;
            AllIncomingMessages = new Queue<MessageTypeHandler>();
            MessagesToDisplay = new Queue<MessageTypeHandler>();
        }

        private void consumeMessage(GlobalMsgEventArgs ev)
        {
            
        }

        private static void ChatManager(object obj, ChatManagerUpdater operatingClass)
        {
            CancellationToken token = (CancellationToken)obj;
            while (ChatManagerCore.isRunning)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                operatingClass.processQueue();

                Thread.Sleep(5000);


            }
        }

        private void processQueue(){
            
        }

        internal void shutdown()
        {
            
        }
    }
}
