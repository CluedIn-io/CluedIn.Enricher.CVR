using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class ProduktionsEnhedMetadata
	{
		public int nyesteCvrNummerRelation { get; set; }
		public Adresse nyesteBeliggenhedsadresse { get; set; }
		public Branche nyesteBibranche1 { get; set; }
		public NyesteKvartalsbeskaeftigelse nyesteKvartalsbeskaeftigelse { get; set; }
		public Branche nyesteBibranche3 { get; set; }
		public string sammensatStatus { get; set; }
		public NyesteNavn nyesteNavn { get; set; }
		public NyesteAarsbeskaeftigelse nyesteAarsbeskaeftigelse { get; set; }
		public Branche nyesteBibranche2 { get; set; }
		public Branche nyesteHovedbranche { get; set; }
		public List<object> nyesteKontaktoplysninger { get; set; }
	}
}