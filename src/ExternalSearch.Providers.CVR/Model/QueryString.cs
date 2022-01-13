using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model
{
	public class QueryString
	{
		[JsonProperty("query")]
		public object Query { get; set; }

		[JsonProperty("fields")]
		public List<string> Fields { get; set; }
	}
}