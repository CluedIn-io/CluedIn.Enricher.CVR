namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class CompanyResult
	{
		public int took { get; set; }
		public bool timed_out { get; set; }
		public Shards _shards { get; set; }
		public Hits hits { get; set; }
	}
}