namespace Assets.Scripts.Model
{
    public class ChromosomePair
    {
        public Chromosome Chromosome1 { get; set; }
        public Chromosome Chromosome2 { get; set; }
        public bool IsVisible { get; set; } = false;

        public bool SameChromosomePair(ChromosomePair other)
        {
            //Don't Compare IsVisible, because it doesn't affect the equality of the genome
            return (Chromosome1.Equals(other.Chromosome1)) &&
                   (Chromosome2.Equals(other.Chromosome2));
        }
    }
}
