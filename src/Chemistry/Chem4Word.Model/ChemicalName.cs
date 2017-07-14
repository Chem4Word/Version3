namespace Chem4Word.Model
{
    public class ChemicalName
    {
        public string Id { get; set; }

        public string DictRef { get; set; }

        public string Name { get; set; }

        public bool IsValid { get; set; }

        public ChemicalName()
        {
        }
    }
}