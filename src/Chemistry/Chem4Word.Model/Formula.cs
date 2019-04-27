// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

namespace Chem4Word.Model
{
    public class Formula
    {
        public string Id { get; set; }

        public string Convention { get; set; }

        public string Inline { get; set; }

        public bool IsValid { get; set; }

        public Formula()
        {
        }
    }
}