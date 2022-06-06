using Exiled.Events.EventArgs;

namespace ChatManagerUtility
{
    public class ChatManagerCore
    {
        /// <summary>
        /// Whether this application is running or not - should be cleaned up when round ends/plugin disabled
        /// </summary>
        public static bool isRunning;

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

       

        public void OnVerified(VerifiedEventArgs ev)
        {
            if(ev.Player != null){
                ChatManagerUpdater chatManagerUpdater = new ChatManagerUpdater(ev.Player);
                //Thread thread = new Thread(new ThreadStart(ChatManagerParser));
                ev.Player.SessionVariables.Add("ChatManagerToken", chatManagerUpdater);
            }
        }

        public void OnLeft(LeftEventArgs ev)
        {
            if (ev.Player != null)
            {
                if (ev.Player.SessionVariables.TryGetValue("ChatManagerToken", out object ChatManager)){
                    ((ChatManagerUpdater)ChatManager).shutdown();
                    ev.Player.SessionVariables.Remove("ChatManagerToken");
                }
            }
        }
    }
}
