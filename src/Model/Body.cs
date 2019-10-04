using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model
{
    public class Body
    {
        [JsonProperty("from")]
        public long From { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("query")]
        public Query Query { get; set; }
    }
}
