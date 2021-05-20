// ---------------------------------------------------------------------------
//  Copyright (c) 2021, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

namespace Chem4Word.Telemetry
{
    public class AddInProperties
    {
        public string KeyName { get; set; }
        public string Description { get; set; }
        public string FriendlyName { get; set; }
        public int LoadBehaviour { get; set; }
        public string Manifest { get; set; }
    }
}