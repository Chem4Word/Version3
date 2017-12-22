// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

namespace Chem4Word.Model.Converters.MDL
{
    public class MDLConstants
    {
        public const string M_CHG = "M  CHG";             // Represents the tag in the MDL Properties Block for Charge
        public const string M_ISO = "M  ISO";             // Represents the tag in the MDL Properties Block for an Isotopes list
        public const string M_RAD = "M  RAD";             // Represents the tag in the MDL Properties Block for an Radical list

        public const string M_END = "M  END";             // Represents the tag in the MDL Properties Block terminating the MolFile
        public const string SDF_END = "$$$$";             // Represents the end of the SD file block
    }
}