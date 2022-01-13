using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Xbrl
{
	public class XbrlResponse
	{

		[JsonProperty("took")]
		public int Took { get; set; }

		[JsonProperty("timed_out")]
		public bool TimedOut { get; set; }

		[JsonProperty("_shards")]
		public Shards Shards { get; set; }

		[JsonProperty("hits")]
		public Hits Hits { get; set; }
	}
}