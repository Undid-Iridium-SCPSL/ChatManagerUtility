using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;

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
        public ChatManagerCore() {
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
            if (ev.Player != null) {
                Log.Debug($"OnVerified loaded in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
                ChatManagerUpdater chatManagerUpdater = new ChatManagerUpdater(ev.Player);
                //Thread thread = new Thread(new ThreadStart(ChatManagerParser));
                ev.Player.SessionVariables.Add("ChatManagerToken", chatManagerUpdater);
                Log.Debug($"OnVerified Finished", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            }
        }

        public void OnLeft(LeftEventArgs ev)
        {
            if (ev.Player != null)
            {
                Log.Debug($"OnLeft loaded in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
                if (ev.Player.SessionVariables.TryGetValue("ChatManagerToken", out object ChatManager)) {
                    ((ChatManagerUpdater)ChatManager).Shutdown();
                    ev.Player.SessionVariables.Remove("ChatManagerToken");
                    Log.Debug($"OnLeft Finished", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
                }
            }
        }

        internal void OnRestarting()
        {
            OnEndRound(null);
        }

        internal void OnEndRound(RoundEndedEventArgs ev)
        {
            Log.Debug($"OnEndRound loaded in", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
            foreach (Player player in Player.List){
                if (player.SessionVariables.TryGetValue("ChatManagerToken", out object ChatManager))
                {
                    Timing.CallDelayed(3f, delegate { ((ChatManagerUpdater)ChatManager).Shutdown(); });
                    player.SessionVariables.Remove("ChatManagerToken");
                    Log.Debug($"OnEndRound Finished", ChatManagerUtilityMain.Instance.Config.IsDebugEnabled);
                }
            }
        }
    }
}
