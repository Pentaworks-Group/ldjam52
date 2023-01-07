using System;

namespace Assets.Scripts.Model
{
    public class Chromosome
    {
        //Expected Value (Normal Distribution)
        public Double Value0 { get; set; }
        //Variance (Normal Distribution)
        public Double ValueDev { get; set; }
        //Dominance of the Chromosome
        public Boolean IsDominant { get; set; } = false;
    }
}
