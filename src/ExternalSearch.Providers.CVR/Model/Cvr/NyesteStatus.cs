using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class NyesteStatus
	{

		[JsonProperty("statuskode")]
		public int Statuskode { get; set; }

		[JsonProperty("statustekst")]
		public string Statustekst { get; set; }

		[JsonProperty("kreditoplysningkode")]
		public int? Kreditoplysningkode { get; set; }

		[JsonProperty("kreditoplysningtekst")]
		public string Kreditoplysningtekst { get; set; }

		[JsonProperty("periode")]
		public Periode Periode { get; set; }

		[JsonProperty("sidstOpdateret")]
		public string SidstOpdateret { get; set; }
	}
}