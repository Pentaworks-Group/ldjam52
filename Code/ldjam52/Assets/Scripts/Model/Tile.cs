using System;

using GameFrame.Core.Math;

namespace Assets.Scripts.Model
{
    public class Tile
    {
        public Guid ID { get; set; }
        public Biome Biome { get; set; }
        public Boolean IsOwned { get; set; }
        public Vector3 Position { get; set; }
        public Field Field { get; set; }
        public Building Building { get; set; }
        public Int32 Price { get; set; }
    }
}
