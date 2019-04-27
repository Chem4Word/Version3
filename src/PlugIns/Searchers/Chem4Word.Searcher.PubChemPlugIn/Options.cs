// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Newtonsoft.Json;

namespace Chem4Word.Searcher.PubChemPlugIn
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options
    {
        [JsonProperty]
        public string PubChemWebServiceUri { get; set; }

        [JsonProperty]
        public string PubChemRestApiUri { get; set; }

        [JsonProperty]
        public int DisplayOrder { get; set; }

        [JsonProperty]
        public int ResultsPerCall { get; set; }

        public Options()
        {
            RestoreDefaults();
        }

        public Options Clone()
        {
            Options clone = new Options();

            clone.PubChemRestApiUri = PubChemRestApiUri;
            clone.PubChemWebServiceUri = PubChemWebServiceUri;
            clone.DisplayOrder = DisplayOrder;
            clone.ResultsPerCall = ResultsPerCall;

            return clone;
        }

        public void RestoreDefaults()
        {
            PubChemRestApiUri = Constants.DefaultPubChemRestApiUri;
            PubChemWebServiceUri = Constants.DefaultPubChemWebServiceUri;
            ResultsPerCall = Constants.DefaultSearchResultsPerCall;
            DisplayOrder = Constants.DefaultDisplayOrder;
        }
    }
}