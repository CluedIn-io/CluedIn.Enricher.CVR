using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Penheder
	{

		[JsonProperty("pNummer")]
		public int PNummer { get; set; }

		[JsonProperty("periode")]
		public Periode Periode { get; set; }

		[JsonProperty("sidstOpdateret")]
		public string SidstOpdateret { get; set; }
	}
}