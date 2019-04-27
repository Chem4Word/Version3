// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

namespace Chem4Word.Helpers
{
    public class TargetWord
    {
        public string ChemicalName { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int ChemistryId { get; set; }
    }
}