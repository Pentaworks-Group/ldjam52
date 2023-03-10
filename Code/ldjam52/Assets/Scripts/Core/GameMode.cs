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
        public Core.Generating.WorldGenerationSettings World {get;set;}
        public Core.Generating.PlayerSettings Player {get;set;}
        public List<Plant> AvailablePlants { get; set; }

        public List<StorageItem> AvailableShopItems { get; set; }

        public List<BiomeType> AvailableBiomes { get; set; }
    }
}
