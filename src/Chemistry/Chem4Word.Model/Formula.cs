namespace Chem4Word.Model
{
    public class Formula
    {
        public string Id { get; set; }

        public string Convention { get; set; }

        public string Inline { get; set; }

        public bool IsValid { get; set; }

        public Formula()
        {
        }
    }
}