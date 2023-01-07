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
        public Double Temp_0 { get; set; }
        public Double Temp_dev { get; set; }
        public Double Sun_0 { get; set; }
        public Double Sun_dev { get; set; }
        public Double Fertilty_0 { get; set; }
        public Double Fertility_dev { get; set; }
        public Double Water_0 { get; set; }
        public Double Water_dev { get; set; }
        public Double Growth_min { get; set; }
        public Double Growth_max { get; set; }
        public Double Age_min { get; set; }
        public Double Age_max { get; set; }
        public Double Wither_min { get; set; }
        public Double Wither_max { get; set; }
        public Double Durability_min { get; set; }
        public Double Durability_max { get; set; }
        public Double Harvest_min { get; set; }
        public Double Harvest_max { get; set; }
        public Double Seeds_min { get; set; }
        public Double Seeds_max { get; set; }  
        public Double Plant_Value_min { get; set; }
        public Double Plant_Value_max { get; set; }
        public Double Seeds_Value_min { set; get; }
        public Double Seeds_Value_max { set; get; }
    }
}
