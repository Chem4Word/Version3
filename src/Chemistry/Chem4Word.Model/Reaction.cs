// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

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