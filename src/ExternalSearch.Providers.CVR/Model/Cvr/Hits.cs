using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Hits
	{
		public int total { get; set; }
		public double? max_score { get; set; }
		public List<Hit> hits { get; set; }
	}
}