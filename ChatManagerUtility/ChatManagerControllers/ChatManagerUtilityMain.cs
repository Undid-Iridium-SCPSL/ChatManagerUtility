using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlayerEvents = Exiled.Events.Handlers.Player;
using ServerEvents = Exiled.Events.Handlers.Server;


namespace ChatManagerUtility
{
    public class ChatManagerUtilityMain : Plugin<Config>
    {
        public static bool isEnabledAtRuntime { get; set; }

        /// <summary>
        /// Gets a static instance of the <see cref="RespawnControllerMain"/> class.
        /// </summary>
        public static ChatManagerUtilityMain Instance { get; private set; }

        /// <inheritdoc />
        public override string Author => "Undid-Iridium";

        /// <inheritdoc />
        public override string Name => "FastRespawnUtility";

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(5, 1, 3);

        /// <inheritdoc />
        public override Version Version { get; } = new Version(1, 0, 0);


        /// <summary>
        /// Gets an instance of the <see cref="ChatManagerCore"/> class.
        /// </summary>
        public ChatManagerCore ChatManagerCoreMonitor { get; private set; }

        /// <inheritdoc />
        public override void OnEnabled()
        {
            Instance = this;
            ChatManagerCoreMonitor = new ChatManagerCore();
            PlayerEvents.Verified += ChatManagerCoreMonitor.OnVerified;
            PlayerEvents.Left += ChatManagerCoreMonitor.OnLeft;
            isEnabledAtRuntime = true;
            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            PlayerEvents.Verified -= ChatManagerCoreMonitor.OnVerified;
            PlayerEvents.Left -= ChatManagerCoreMonitor.OnLeft;
            isEnabledAtRuntime = false;
            ChatManagerCoreMonitor = null;
            Instance = null;
            base.OnDisabled();
            
        }
    }
}
