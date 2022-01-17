using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Aarsbeskaeftigelse
	{

		[JsonProperty("aar")]
		public int Aar { get; set; }

		[JsonProperty("antalInklusivEjere")]
		public object AntalInklusivEjere { get; set; }

		[JsonProperty("antalAarsvaerk")]
		public object AntalAarsvaerk { get; set; }

		[JsonProperty("antalAnsatte")]
		public object AntalAnsatte { get; set; }

		[JsonProperty("sidstOpdateret")]
		public string SidstOpdateret { get; set; }

		[JsonProperty("intervalKodeAntalInklusivEjere")]
		public string IntervalKodeAntalInklusivEjere { get; set; }

		[JsonProperty("intervalKodeAntalAarsvaerk")]
		public string IntervalKodeAntalAarsvaerk { get; set; }

		[JsonProperty("intervalKodeAntalAnsatte")]
		public string IntervalKodeAntalAnsatte { get; set; }
	}
}