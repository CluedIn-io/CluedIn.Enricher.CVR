using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Organisationer
	{

		[JsonProperty("enhedsNummerOrganisation")]
		public string EnhedsNummerOrganisation { get; set; }

		[JsonProperty("hovedtype")]
		public string Hovedtype { get; set; }

		[JsonProperty("organisationsNavn")]
		public List<OrganisationsNavn> OrganisationsNavn { get; set; }

		[JsonProperty("attributter")]
		public List<Attributter> Attributter { get; set; }

		[JsonProperty("medlemsData")]
		public List<MedlemsData> MedlemsData { get; set; }
	}
}