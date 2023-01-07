using System;

using Assets.Scripts.Model;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Generating.World
{
    public class BiomeSpawn
    {
        public BiomeSpawn(Biome biome)
        {
            this.Biome = biome;

            this.Weight = UnityEngine.Random.Range(biome.WeightMin, biome.WeightMax);
        }

        public Biome Biome { get; }
        public Single Weight { get; }

        public Vector3 Position { get; set; }
    }
}
