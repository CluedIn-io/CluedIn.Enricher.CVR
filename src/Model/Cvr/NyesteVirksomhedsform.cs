using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class NyesteVirksomhedsform
	{
		[JsonProperty("virksomhedsformkode")]
		public int Virksomhedsformkode { get; set; }

		[JsonProperty("kortBeskrivelse")]
		public string KortBeskrivelse { get; set; }

		[JsonProperty("langBeskrivelse")]
		public string LangBeskrivelse { get; set; }

		[JsonProperty("ansvarligDataleverandoer")]
		public string AnsvarligDataleverandoer { get; set; }

		[JsonProperty("periode")]
		public Periode Periode { get; set; }

		[JsonProperty("sidstOpdateret")]
		public string SidstOpdateret { get; set; }
	}
}