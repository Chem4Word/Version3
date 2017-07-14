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