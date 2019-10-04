using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Xbrl
{
	public class Dokumenter
	{

		[JsonProperty("dokumentUrl")]
		public string DokumentUrl { get; set; }

		[JsonProperty("dokumentMimeType")]
		public string DokumentMimeType { get; set; }

		[JsonProperty("dokumentType")]
		public string DokumentType { get; set; }
	}
}