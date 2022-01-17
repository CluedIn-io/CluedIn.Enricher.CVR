namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Kommune
	{
		public int kommuneKode { get; set; }
		public string kommuneNavn { get; set; }
		public Periode periode { get; set; }
		public string sidstOpdateret { get; set; }
	}
}