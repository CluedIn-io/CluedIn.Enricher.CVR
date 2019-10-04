using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class VrproduktionsEnhed
	{
		public List<Branche> bibranche3 { get; set; }
		public List<Branche> bibranche1 { get; set; }
		public ProduktionsEnhedMetadata produktionsEnhedMetadata { get; set; }
		public int pNummer { get; set; }
		public List<ElektroniskPost> elektroniskPost { get; set; }
		public string sidstOpdateret { get; set; }
		public bool fejlRegistreret { get; set; }
		public List<Branche> bibranche2 { get; set; }
		public object fejlBeskrivelse { get; set; }
		public string enhedstype { get; set; }
		public int dataAdgang { get; set; }
		public List<Attributter> attributter { get; set; }
		public bool fejlVedIndlaesning { get; set; }
		public List<object> postadresse { get; set; }
		public List<Branche> hovedbranche { get; set; }
		public List<Livsforloeb> livsforloeb { get; set; }
		public int samtId { get; set; }
		public List<Kvartalsbeskaeftigelse> kvartalsbeskaeftigelse { get; set; }
		public bool reklamebeskyttet { get; set; }
		public string naermesteFremtidigeDato { get; set; }
		public List<Aarsbeskaeftigelse> aarsbeskaeftigelse { get; set; }
		public List<Virksomhedsrelation> virksomhedsrelation { get; set; }
		public List<Adresse> beliggenhedsadresse { get; set; }
		public List<Navne> navne { get; set; }
		public string sidstIndlaest { get; set; }
		public string virkningsAktoer { get; set; }
		public object brancheAnsvarskode { get; set; }
		public List<TelefaxNummer> telefaxNummer { get; set; }
		public long enhedsNummer { get; set; }
		public List<DeltagerRelation> deltagerRelation { get; set; }
		public List<TelefonNummer> telefonNummer { get; set; }
	}
}