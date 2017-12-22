// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Newtonsoft.Json;

namespace Chem4Word.Searcher.OpsinPlugIn
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options
    {
        [JsonProperty]
        public string OpsinWebServiceUri { get; set; }

        [JsonProperty]
        public int DisplayOrder { get; set; }

        public Options()
        {
            RestoreDefaults();
        }

        public Options Clone()
        {
            Options clone = new Options();

            clone.OpsinWebServiceUri = OpsinWebServiceUri;
            clone.DisplayOrder = DisplayOrder;

            return clone;
        }

        public void RestoreDefaults()
        {
            OpsinWebServiceUri = Constants.DefaultOpsinWebServiceUri;
            DisplayOrder = Constants.DefaultDisplayOrder;
        }
    }
}