 using System;

namespace Assets.Scripts.Model
{
    public class Biome
    {
        public String Name { get; set; }
        //Game Value: Possible Humidity values for the Biome
        public Double Humidity_min { get; set; }
        public Double Humidity_max { get; set; }
        //Game Value: Possible Temperature values for the Biome
        public Double Temperature_min { get; set; }
        public Double Temperature_max { get; set; }
        //Game Value: Possible Sunhsine values for the Biome
        public Double Sunshine_min { get; set; }
        public Double Sunshine_max { get; set; }
        //Game Value: Possible Fertility values for the Biome
        public Double Fertility_min { get; set; }
        public Double Fertility_max { get; set; }
    }
}
