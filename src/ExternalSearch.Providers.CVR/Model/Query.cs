using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model
{
	public class Query
	{
		[JsonProperty("query_string")]
		public QueryString QueryString { get; set; }
	}
}