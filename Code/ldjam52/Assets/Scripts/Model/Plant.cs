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
        //Set of Chromosome Pairs
        //The following Sets have to be defined:
        // 'Temp': reaction to temperature
        // 'Sun': reaction to sunlight
        // 'Fertility': reaction to Fertility of the ground
        // 'Water': need of Water (could come from Humidity of the ground or irrigation)
        // 'Growth': the speed of the growth on the field
        // 'Wither': time in which the plant withers (on the field)
        // 'Durability': time after which the plan is not sellable anymore (being in storage)
        // 'Harvest': amount of harvest
        // 'Seeds': amount of seeds coming out of a harvest
        // 'PlantValue': selling value of the plant
        // 'SeedsValue': selling value of the new seeds
        public Dictionary<String, ChromosomePair> genome = new Dictionary<string, ChromosomePair> { };

    }
}
