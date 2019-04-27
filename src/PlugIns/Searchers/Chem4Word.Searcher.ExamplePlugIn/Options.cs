// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Newtonsoft.Json;

namespace Chem4Word.Searcher.ExamplePlugIn
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options
    {
        [JsonProperty]
        public bool Property1 { get; set; }

        [JsonProperty]
        public bool Property2 { get; set; }

        public Options()
        {
            RestoreDefaults();
        }

        public Options Clone()
        {
            Options clone = new Options();

            clone.Property1 = Property1;
            clone.Property2 = Property2;

            return clone;
        }

        public void RestoreDefaults()
        {
            Property1 = true;
            Property2 = true;
        }
    }
}