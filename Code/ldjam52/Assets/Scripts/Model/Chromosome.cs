using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class Chromosome
    {
        //Expected Value (Normal Distribution)
        public Double Value_0 { get; set; }
        //Variance (Normal Distribution)
        public Double Value_dev { get; set; }
        //Dominance of the Chromosome
        public Boolean dominant { get; set; } = false;
    }
}
