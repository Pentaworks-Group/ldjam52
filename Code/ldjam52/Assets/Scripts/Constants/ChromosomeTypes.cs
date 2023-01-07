using System;

namespace Assets.Scripts.Constants
{
    public static class ChromosomeTypes
    {
        // 'Temp': reaction to temperature
        public const String TEMP = "Temp";
        // 'Sun': reaction to sunlight
        public const String SUN = "Sun";
        // 'Fertility': reaction to Fertility of the ground
        public const String FERTILITY = "Fertility";
        // 'Water': need of Water (could come from Humidity of the ground or irrigation)
        public const String WATER = "Water";
        // 'Growth': the speed of the growth on the field
        public const String GROWTH = "Growth";
        // 'Wither': time in which the plant withers (on the field)
        public const String WITHER = "Wither";
        // 'Durability': time after which the plan is not sellable anymore (being in storage)
        public const String DURABILITY = "Durability";
        // 'Harvest': amount of harvest
        public const String HARVEST = "Harvest";
        // 'Seeds': amount of seeds coming out of a harvest
        public const String SEEDS = "Seeds";
        // 'PlantValue': selling value of the plant
        public const String PLANTVALUE = "PlantValue";
        // 'SeedsValue': selling value of the new seeds
        public const String SEEDSVALUE = "SeedsValue";
    }
}
