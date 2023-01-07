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
        public Chromosome Temp_1 { get; set; }
        public Chromosome Temp_2 { get; set; }
        public Chromosome Sun_1 { get; set; }
        public Chromosome Sun_2 { get; set; }
        public Chromosome Fertility_1 { get; set; }
        public Chromosome Fertility_2 { get; set; }
        public Chromosome Water_1 { get; set; }
        public Chromosome Water_2 { get; set; }
        public Chromosome Growth_1 { get; set; }
        public Chromosome Growth_2 { get; set; }
        public Chromosome Age_1 { get; set; }
        public Chromosome Age_2 { get; set; }
        public Chromosome Wither_1 { get; set; }
        public Chromosome Wither_2 { get; set; }
        public Chromosome Durabilty_1 { get; set; }
        public Chromosome Durabilty_2 { get; set; }
        public Chromosome Harvest_1 { get; set; }
        public Chromosome Harvest_2 { get; set; }
        public Chromosome Seeds_1 { get; set; }
        public Chromosome Seeds_2 { get; set; }
        public Chromosome Plant_Value_1 { get; set; }
        public Chromosome Plant_Value_2 { get; set; }
        public Chromosome Seeds_Value_1 { get; set; }
        public Chromosome Seeds_Value_2 { get; set; }
    }
}
