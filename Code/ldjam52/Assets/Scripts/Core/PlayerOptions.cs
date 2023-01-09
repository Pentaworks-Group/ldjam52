using System;

namespace Assets.Scripts.Core
{
    public class PlayerOptions : GameFrame.Core.PlayerOptions
    {
        public Boolean IsMouseScreenEdgeScrollingEnabled { get; set; }

        public float MoveSensivity { get; set; } = 0.5f;
        public float ZoomSensivity { get; set; } = 0.5f;
    }
}
