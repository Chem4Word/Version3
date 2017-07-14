using System.Collections.ObjectModel;

namespace Chem4Word.Model
{
    public class Reaction
    {
        public readonly ObservableCollection<Molecule> Reactants;
        public readonly ObservableCollection<Molecule> Products;
        public double Yield;
        public string[] Reagents;
        public string[] Solvents;
        public double Temperature;
        public string AdditionalConditions;
    }
}