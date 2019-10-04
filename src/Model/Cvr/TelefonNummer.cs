using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class TelefonNummer
	{

		[JsonProperty("kontaktoplysning")]
		public string Kontaktoplysning { get; set; }

		[JsonProperty("hemmelig")]
		public bool Hemmelig { get; set; }

		[JsonProperty("periode")]
		public Periode Periode { get; set; }

		[JsonProperty("sidstOpdateret")]
		public string SidstOpdateret { get; set; }
	}
}