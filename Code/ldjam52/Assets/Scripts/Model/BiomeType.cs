using System;

using GameFrame.Core.Media;

namespace Assets.Scripts.Model
{
    public class BiomeType
    {
        public String Name { get; set; }
        public Color Color { get; set; }

        public Int32 TilePriceMin { get; set; }
        public Int32 TilePriceMax { get; set; }
        
        //Use while generating world.
        public Single WeightMin { get; set; }
        public Single WeightMax { get; set; }

        //Game Value: Possible Humidity values for the Biome
        public Single HumidityMin { get; set; }
        public Single HumidityMax { get; set; }
        //Game Value: Possible Temperature values for the Biome
        public Single TemperatureMin { get; set; }
        public Single TemperatureMax { get; set; }
        //Game Value: Possible Sunhsine values for the Biome
        public Single SunshineMin { get; set; }
        public Single SunshineMax { get; set; }
        //Game Value: Possible Fertility values for the Biome
        public Single FertilityMin { get; set; }
        public Single FertilityMax { get; set; }
    }
}
