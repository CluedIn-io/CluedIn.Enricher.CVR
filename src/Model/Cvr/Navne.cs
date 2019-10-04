using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Navne
	{

		[JsonProperty("navn")]
		public string Navn { get; set; }

		[JsonProperty("periode")]
		public Periode Periode { get; set; }

		[JsonProperty("sidstOpdateret")]
		public string SidstOpdateret { get; set; }
	}
}