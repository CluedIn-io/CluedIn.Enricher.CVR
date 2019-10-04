using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Virksomhedsstatus
	{

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("periode")]
		public Periode Periode { get; set; }

		[JsonProperty("sidstOpdateret")]
		public string SidstOpdateret { get; set; }
	}
}