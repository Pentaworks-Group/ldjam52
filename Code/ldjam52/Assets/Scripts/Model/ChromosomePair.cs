namespace Assets.Scripts.Model
{
    public class ChromosomePair
    {
        public Chromosome Chromosome1 { get; set; }
        public Chromosome Chromosome2 { get; set; }

        public bool Equals(ChromosomePair other)
        {
            return (Chromosome1 == other.Chromosome1) &&
                   (Chromosome2 == other.Chromosome2);
        }
    }
}
