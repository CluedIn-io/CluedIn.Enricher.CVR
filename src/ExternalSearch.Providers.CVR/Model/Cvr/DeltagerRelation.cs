using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class DeltagerRelation
	{

		[JsonProperty("deltager")]
		public Deltager Deltager { get; set; }

		[JsonProperty("kontorsteder")]
		public List<Kontorsteder> Kontorsteder { get; set; }

		[JsonProperty("organisationer")]
		public List<Organisationer> Organisationer { get; set; }
	}
}