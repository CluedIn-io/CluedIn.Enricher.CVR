using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Vrvirksomhed
	{
		public int cvrNummer { get; set; }
		public List<RegNummer> regNummer { get; set; }
		public int? brancheAnsvarskode { get; set; }
		public bool reklamebeskyttet { get; set; }
		public List<Navne> navne { get; set; }
		public List<Binavne> binavne { get; set; }
		public List<Adresse> postadresse { get; set; }
		public List<Adresse> beliggenhedsadresse { get; set; }
		public List<TelefonNummer> telefonNummer { get; set; }
		public List<TelefaxNummer> telefaxNummer { get; set; }
		public List<ElektroniskPost> elektroniskPost { get; set; }
		public List<Hjemmeside> hjemmeside { get; set; }
		public List<ObligatoriskEmail> obligatoriskEmail { get; set; }
		public List<Livsforloeb> livsforloeb { get; set; }
		public List<Branche> hovedbranche { get; set; }
		public List<Branche> bibranche1 { get; set; }
		public List<Branche> bibranche2 { get; set; }
		public List<Branche> bibranche3 { get; set; }
		public List<Status> status { get; set; }
		public List<Virksomhedsstatus> virksomhedsstatus { get; set; }
		public List<Virksomhedsform> virksomhedsform { get; set; }
		public List<Aarsbeskaeftigelse> aarsbeskaeftigelse { get; set; }
		public List<Kvartalsbeskaeftigelse> kvartalsbeskaeftigelse { get; set; }
		public List<Maanedsbeskaeftigelse> maanedsbeskaeftigelse { get; set; }
		public List<Attributter> attributter { get; set; }
		public List<Penheder> penheder { get; set; }
		public List<DeltagerRelation> deltagerRelation { get; set; }
		public List<Fusioner> fusioner { get; set; }
		public List<Spaltninger> spaltninger { get; set; }
		public VirksomhedMetadata virksomhedMetadata { get; set; }
		public int samtId { get; set; }
		public bool fejlRegistreret { get; set; }
		public int dataAdgang { get; set; }
		public string enhedsNummer { get; set; }
		public string enhedstype { get; set; }
		public string sidstIndlaest { get; set; }
		public string sidstOpdateret { get; set; }
		public bool fejlVedIndlaesning { get; set; }
		public string naermesteFremtidigeDato { get; set; }
		public object fejlBeskrivelse { get; set; }
		public string virkningsAktoer { get; set; }
	}
}