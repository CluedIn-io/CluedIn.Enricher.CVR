using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class MedlemsData
	{

		[JsonProperty("attributter")]
		public List<Attributter> Attributter { get; set; }
	}
}