namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Virksomhedsform
	{
		public int virksomhedsformkode { get; set; }
		public string kortBeskrivelse { get; set; }
		public string langBeskrivelse { get; set; }
		public string ansvarligDataleverandoer { get; set; }
		public Periode periode { get; set; }
		public string sidstOpdateret { get; set; }
	}
}