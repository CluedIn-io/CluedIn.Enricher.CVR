using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Kontorsteder
	{

		[JsonProperty("penhed")]
		public Penhed Penhed { get; set; }

		[JsonProperty("attributter")]
		public List<Attributter> Attributter { get; set; }
	}
}