using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Xbrl
{
	public class Offentliggoerelse
	{

		[JsonProperty("cvrNummer")]
		public int CvrNummer { get; set; }

		[JsonProperty("indlaesningsId")]
		public object IndlaesningsId { get; set; }

		[JsonProperty("sidstOpdateret")]
		public string SidstOpdateret { get; set; }

		[JsonProperty("omgoerelse")]
		public bool Omgoerelse { get; set; }

		[JsonProperty("regNummer")]
		public object RegNummer { get; set; }

		[JsonProperty("offentliggoerelsestype")]
		public string Offentliggoerelsestype { get; set; }

		[JsonProperty("regnskab")]
		public Regnskab Regnskab { get; set; }

		[JsonProperty("indlaesningsTidspunkt")]
		public string IndlaesningsTidspunkt { get; set; }

		[JsonProperty("sagsNummer")]
		public string SagsNummer { get; set; }

		[JsonProperty("dokumenter")]
		public List<Dokumenter> Dokumenter { get; set; }

		[JsonProperty("offentliggoerelsesTidspunkt")]
		public string OffentliggoerelsesTidspunkt { get; set; }
	}
}