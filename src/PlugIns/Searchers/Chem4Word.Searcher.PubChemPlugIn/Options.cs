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