using System;

using GameFrame.Core.Math;

namespace Assets.Scripts.Model
{
    public class Tile
    {
        public Boolean IsOwned { get; set; }
        public Biome Biome { get; set; }
        public Field Field { get; set; }
        public Vector2 Position { get; set; }
    }
}
