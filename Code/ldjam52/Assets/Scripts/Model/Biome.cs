using System;

using GameFrame.Core.Math;

namespace Assets.Scripts.Model
{
    public class Biome
    {
        public BiomeType Type { get; set; }
        public Single Weight { get; set; }
        public Int32 Size { get; set; }
        public Vector3 SpawnPosition { get; set; }
    }
}
