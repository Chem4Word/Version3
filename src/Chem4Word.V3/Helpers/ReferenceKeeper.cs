// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Chem4Word.Helpers
{
    /// <summary>
    /// The sole purpose of this class is to keep references to assemblies which may only be used in supporting assemblies
    /// </summary>
    public class ReferenceKeeper
    {
        public CloudTable Table { get; set; }
    }
}
