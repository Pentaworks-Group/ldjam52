using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class World
    {
        public Int32 Height { get; set; }
        public Int32 Width { get; set; }
        public Farm Farm { get; set; }
        public List<Tile> Tiles { get; set; }
    }
}
