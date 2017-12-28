// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Newtonsoft.Json;

namespace Chem4Word.Searcher.ChEBIPlugin
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options
    {
        #region Constructors

        public Options()
        {
            RestoreDefaults();
        }

        #endregion Constructors

        #region Properties

        [JsonProperty]
        public string ChEBIWebServiceUri { get; set; }

        [JsonProperty]
        public int DisplayOrder { get; set; }

        [JsonProperty]
        public int MaximumResults { get; set; }

        #endregion Properties

        #region Methods

        public Options Clone()
        {
            Options clone = new Options();

            clone.ChEBIWebServiceUri = StripTrailingSlash(ChEBIWebServiceUri);
            clone.DisplayOrder = DisplayOrder;
            clone.MaximumResults = MaximumResults;
            return clone;
        }

        public void RestoreDefaults()
        {
            ChEBIWebServiceUri = Constants.DefaultChEBIWebServiceUri;
            DisplayOrder = Constants.DefaultDisplayOrder;
            MaximumResults = Constants.DefaultMaximumSearchResults;
        }

        private string StripTrailingSlash(string uri)
        {
            if (uri.EndsWith("/"))
            {
                uri = uri.Remove(uri.Length - 1);
            }

            return uri;
        }

        #endregion Methods
    }
}