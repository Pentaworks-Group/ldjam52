using System;
using System.Collections.Generic;

using Assets.Scripts.Model;

namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        public World World { get; set; }
        public GameMode GameMode { get; set; }

        public Dictionary<Guid, Plant> KnownPlants { get; set; }

        public FarmStorage FarmStorage { get; set; }

        public float ElapsedTime { get; set; }
    }
}
