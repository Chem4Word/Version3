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