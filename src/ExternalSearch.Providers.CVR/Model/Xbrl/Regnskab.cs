using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Xbrl
{
	public class Regnskab
	{

		[JsonProperty("regnskabsperiode")]
		public Regnskabsperiode Regnskabsperiode { get; set; }
	}
}