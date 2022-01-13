using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Spaltninger
	{

		[JsonProperty("enhedsNummerOrganisation")]
		public string EnhedsNummerOrganisation { get; set; }

		[JsonProperty("organisationsNavn")]
		public List<OrganisationsNavn> OrganisationsNavn { get; set; }

		[JsonProperty("indgaaende")]
		public List<Indgaaende> Indgaaende { get; set; }

		[JsonProperty("udgaaende")]
		public List<Udgaaende> Udgaaende { get; set; }
	}
}