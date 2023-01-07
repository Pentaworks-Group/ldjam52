using System;

using GameFrame.Core.Math;
using GameFrame.Core.Media;

namespace Assets.Scripts.Model
{
    public class Tile
    {
        public Guid ID { get; set; }
        public Boolean IsOwned { get; set; }
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
        public Single Temperature { get; set; }
        public Single Fertility { get; set; }
        public Single Sunshine { get; set; }
        public Single Humidity { get; set; }
    }
}
