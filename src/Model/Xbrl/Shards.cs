using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Xbrl
{
    public class Shards
    {

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("successful")]
        public int Successful { get; set; }

        [JsonProperty("failed")]
        public int Failed { get; set; }
    }
}