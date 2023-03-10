using System;

using Assets.Scripts.Model;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Generating
{
    public class WorldGenerationSettings
    {
        public Int32 Height { get; set; }
        public Int32 Width { get; set; }
        public Int32 RandomBiomesAmount { get; set; }

        /// <summary>
        /// Forces the Generator to use all biomes available in addition to the <see cref="RandomBiomesAmount"/>.
        /// </summary>
        public Boolean IsAllBiomesUsageForced { get; set; }
        public Single TotalDayDuration { get; set; }
        public BuildingSettings Farm { get; set; }
        public BuildingSettings Shop { get; set; }
        public BuildingSettings Laboratory { get; set; }
    }
}
