// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

namespace Chem4Word.Helpers
{
    public class C4wAtomDefinition
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string AtomicNumber { get; set; }
        public bool AddH { get; set; }
        public string Colour { get; set; }
        public double CovalentRadius { get; set; }
        public double VdWRadius { get; set; }
        public int Valency { get; set; }
        public double Mass { get; set; }
        public string Valencies { get; set; }
    }
}
