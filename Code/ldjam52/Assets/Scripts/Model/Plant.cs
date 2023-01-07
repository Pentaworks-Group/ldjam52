using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class Plant
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public String ImageName { get; set; }
        //Chromosome Pair defining the reaction to temperature
        public ChromosomePair Temp { get; set; }
        //Chromosome Pair defining the reaction to Sunlight
        public ChromosomePair Sun { get; set; }
        //Chromosome Pair defining the reaction to Fertility of the ground
        public ChromosomePair Fertility { get; set; }
        //Chromosome Pair defining the need of Water (could come from Humidity of the ground or irrigation)
        public ChromosomePair Water { get; set; }
        //Chromosome Pair defining the speed of the growth on the field
        public ChromosomePair Growth { get; set; }
        //Chromosome Pair defining the time in which the plant withers (on the field)
        public ChromosomePair Wither { get; set; }
        //Chromosome Pair defining the time after which the plan is not sellable anymore (being in storage)
        public ChromosomePair Durability { get; set; }
        //Chromosome Pair defining the amount of harvest
        public ChromosomePair Harvest { get; set; }
        //Chromosome Pair defining the amount of seeds coming out of a harvest
        public ChromosomePair Seeds { get; set; }
        //Chromosome Pair defining the selling value of the plant
        public ChromosomePair Plant_Value { get; set; }
        //Chromosome Pair defining the selling value of the new seeds
        public ChromosomePair Seeds_Value { get; set; }
    }
}
