namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Hit
	{
		public string _index { get; set; }
		public string _type { get; set; }
		public string _id { get; set; }
		public int _score { get; set; }
		public Source _source { get; set; }
	}
}