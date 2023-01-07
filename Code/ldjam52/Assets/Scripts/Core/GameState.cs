using System;

using Assets.Scripts.Model;

namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        public World World { get; set; }
        public GameMode GameMode { get; set; }

        public Decimal AvailableFunds { get; set; }

        public float ElapsedTime { get; set; }
    }
}
