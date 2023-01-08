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

        public bool Equals(Chromosome other)
        {
            return (Value0 == other.Value0) &&
                   (ValueDev == other.ValueDev) &&
                   (IsDominant == other.IsDominant);
        }
    }
}
