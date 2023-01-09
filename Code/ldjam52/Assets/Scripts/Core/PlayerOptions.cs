using System;

namespace Assets.Scripts.Core
{
    public class PlayerOptions : GameFrame.Core.PlayerOptions
    {
        public Boolean IsMouseScreenEdgeScrollingEnabled { get; set; }

        public float TouchSensivity { get; set; }
    }
}
