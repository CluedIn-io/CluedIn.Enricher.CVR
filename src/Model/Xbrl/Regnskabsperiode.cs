using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Xbrl
{
	public class Regnskabsperiode
	{

		[JsonProperty("startDato")]
		public string StartDato { get; set; }

		[JsonProperty("slutDato")]
		public string SlutDato { get; set; }
	}
}