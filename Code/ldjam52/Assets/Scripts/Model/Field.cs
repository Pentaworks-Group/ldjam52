using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class Field
    {
        public Plant Seed { get; set; }
        // Humidity of the field
        public Double Humidity { get; set; }
        // Temperature of the Biome
        public Double Temperature { get; set; }
        // Sunsihne of the field
        public Double Sunshine { get; set; }
        // Fertility of the field
        public Double Fertility { get; set; }


    }
}
