using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Deltager
	{

		[JsonProperty("enhedsNummer")]
		public string EnhedsNummer { get; set; }

		[JsonProperty("enhedstype")]
		public string Enhedstype { get; set; }

		[JsonProperty("forretningsnoegle")]
		public long? Forretningsnoegle { get; set; }

		[JsonProperty("organisationstype")]
		public object Organisationstype { get; set; }

		[JsonProperty("sidstIndlaest")]
		public string SidstIndlaest { get; set; }

		[JsonProperty("sidstOpdateret")]
		public string SidstOpdateret { get; set; }

		[JsonProperty("navne")]
		public List<Navne> Navne { get; set; }

		[JsonProperty("beliggenhedsadresse")]
		public List<Adresse> Beliggenhedsadresse { get; set; }
	}
}