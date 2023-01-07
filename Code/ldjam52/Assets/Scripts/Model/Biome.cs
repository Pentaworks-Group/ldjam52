 using System;

namespace Assets.Scripts.Model
{
    public class Biome
    {
        public String Name { get; set; }
        public Double Humidity_min { get; set; }
        public Double Humidity_max { get; set; }
        public Double Temperature_min { get; set; }
        public Double Temperature_max { get; set; }
        public Double Sunshine_min { get; set; }
        public Double Sunshine_max { get; set; }
        public Double Fertility_min { get; set; }
        public Double Fertility_max { get; set; }
    }
}
