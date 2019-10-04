using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Xbrl
{
	public class Hits
	{

		[JsonProperty("total")]
		public int Total { get; set; }

		[JsonProperty("max_score")]
		public double MaxScore { get; set; }

		[JsonProperty("hits")]
		public List<Hit> hits { get; set; }
	}
}