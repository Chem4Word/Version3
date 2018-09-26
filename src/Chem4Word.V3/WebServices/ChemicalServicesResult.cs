// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Collections.Generic;

namespace Chem4Word.WebServices
{
    public class ChemicalServicesResult
    {
        public ChemicalProperties[] Properties { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Messages { get; set; }
    }
}
