// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

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