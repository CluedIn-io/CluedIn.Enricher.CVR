using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Udgaaende
	{

		[JsonProperty("sekvensnr")]
		public int Sekvensnr { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("vaerditype")]
		public string Vaerditype { get; set; }

		[JsonProperty("vaerdier")]
		public List<Vaerdier> Vaerdier { get; set; }
	}
}