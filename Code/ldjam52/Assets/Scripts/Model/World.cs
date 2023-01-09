using System;
using System.Collections.Generic;

using Assets.Scripts.Model.Buildings;

namespace Assets.Scripts.Model
{
    public class World
    {
        public Int32 Height { get; set; }
        public Int32 Width { get; set; }
        public Farm Farm { get; set; }
        public Shop Shop { get; set; }
        public Laboratory Laboratory { get; set; }
        public List<Tile> Tiles { get; set; }
    }
}
