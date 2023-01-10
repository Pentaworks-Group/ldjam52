using System;

namespace Assets.Scripts.Model
{
    public class Field
    {
        public Plant Seed { get; set; }
        // Humidity of the field
        public bool IsHumidityVisible { get; set; } = false;
        public Double Humidity { get; set; }
        // Temperature of the Biome
        public bool IsTemperatureVisible { get; set; } = false;
        public Double Temperature { get; set; }
        // Sunsihne of the field
        public bool IsSunshineVisible { get; set; } = false;
        public Double Sunshine { get; set; }
        // Fertility of the field
        public bool IsFertiliyVisible { get; set; } = false;
        public Double Fertility { get; set; }
        // Growth Progress
        public Double GrowthProgress { get; set; } = 0;

        public float TimePlanted { get; set; } = -1;
    }
}
