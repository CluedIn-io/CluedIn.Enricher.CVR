using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Branche
	{
		[JsonProperty("branchekode")]
		public int Branchekode { get; set; }

		[JsonProperty("branchetekst")]
		public string Branchetekst { get; set; }

		[JsonProperty("periode")]
		public Periode Periode { get; set; }

		[JsonProperty("sidstOpdateret")]
		public string SidstOpdateret { get; set; }
	}
}