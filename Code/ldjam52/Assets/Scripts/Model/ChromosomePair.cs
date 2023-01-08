namespace Assets.Scripts.Model
{
    public class ChromosomePair
    {
        public Chromosome Chromosome1 { get; set; }
        public Chromosome Chromosome2 { get; set; }
        public bool IsVisible { get; set; } = false;

        public bool Equals(ChromosomePair other)
        {
            //Don't Compare IsVisible, because it doesn't affect the equality of the genome
            return (Chromosome1 == other.Chromosome1) &&
                   (Chromosome2 == other.Chromosome2);
        }
    }
}
