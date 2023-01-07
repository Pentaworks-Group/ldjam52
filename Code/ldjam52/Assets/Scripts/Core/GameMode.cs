using System;
using System.Collections.Generic;

using Assets.Scripts.Model;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core
{
    public class GameMode
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public Vector2 GeneratedWorldSize { get; set; }
        public Decimal PlayerStartingFunds { get; set; }
        public List<Plant> AvailablePlants { get; set; }
        public List<Biome> AvailableBiomes { get; set; }
    }
}
