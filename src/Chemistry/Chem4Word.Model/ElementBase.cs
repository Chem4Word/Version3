namespace Chem4Word.Model
{
    public abstract class ElementBase
    {
        public virtual double AtomicWeight { get; set; }

        public virtual string Symbol { get; set; }

        public virtual string Name { get; set; }

        public virtual string Colour { get; set; }
    }
}