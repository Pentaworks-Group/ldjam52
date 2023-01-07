using System;

namespace Assets.Scripts.Model
{
    public class Biome
    {
        public String Name { get; set; }
        public String MaterialReference { get; set; } // Used to load the related Material to color the tile
        //Game Value: Possible Humidity values for the Biome
        public Double HumidityMin { get; set; }
        public Double HumidityMax { get; set; }
        //Game Value: Possible Temperature values for the Biome
        public Double TemperatureMin { get; set; }
        public Double TemperatureMax { get; set; }
        //Game Value: Possible Sunhsine values for the Biome
        public Double SunshineMin { get; set; }
        public Double SunshineMax { get; set; }
        //Game Value: Possible Fertility values for the Biome
        public Double FertilityMin { get; set; }
        public Double FertilityMax { get; set; }
    }
}
