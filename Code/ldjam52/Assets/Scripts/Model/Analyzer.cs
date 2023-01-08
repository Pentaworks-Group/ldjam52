using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class Analyzer
    {
        public String Name { get; set; } = "Plant Analyzer";
        public String Description { get; set; } = "Can analyze plants and give some information about how it grows.";
        public int DevelopmentStage { get; set; } = 0;  
        public int MaxDevelopmentStage { get; set; } = 0;
    }
}
