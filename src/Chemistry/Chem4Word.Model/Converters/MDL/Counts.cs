// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

namespace Chem4Word.Model.Converters
{
    public class Counts
    {
        public string Version { get; set; }
        public int Atoms { get; set; }
        public int Bonds { get; set; }
        public string Message { get; set; }
    }
}