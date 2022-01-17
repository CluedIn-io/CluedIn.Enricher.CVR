using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class VirksomhedMetadata
	{
		[JsonProperty("nyesteNavn")]
		public NyesteNavn NyesteNavn { get; set; }

		[JsonProperty("nyesteVirksomhedsform")]
		public NyesteVirksomhedsform NyesteVirksomhedsform { get; set; }

		[JsonProperty("nyesteBeliggenhedsadresse")]
		public Adresse NyesteBeliggenhedsadresse { get; set; }

		[JsonProperty("nyesteHovedbranche")]
		public Branche NyesteHovedbranche { get; set; }

		[JsonProperty("nyesteBibranche1")]
		public Branche NyesteBibranche1 { get; set; }

		[JsonProperty("nyesteBibranche2")]
		public Branche NyesteBibranche2 { get; set; }

		[JsonProperty("nyesteBibranche3")]
		public Branche NyesteBibranche3 { get; set; }

		[JsonProperty("nyesteStatus")]
		public NyesteStatus NyesteStatus { get; set; }

		[JsonProperty("nyesteKontaktoplysninger")]
		public List<string> NyesteKontaktoplysninger { get; set; }

		[JsonProperty("antalPenheder")]
		public int AntalPenheder { get; set; }

		[JsonProperty("nyesteAarsbeskaeftigelse")]
		public NyesteAarsbeskaeftigelse NyesteAarsbeskaeftigelse { get; set; }

		[JsonProperty("nyesteKvartalsbeskaeftigelse")]
		public NyesteKvartalsbeskaeftigelse NyesteKvartalsbeskaeftigelse { get; set; }

		[JsonProperty("nyesteMaanedsbeskaeftigelse")]
		public NyesteMaanedsbeskaeftigelse NyesteMaanedsbeskaeftigelse { get; set; }

		[JsonProperty("sammensatStatus")]
		public string SammensatStatus { get; set; }

		[JsonProperty("stiftelsesDato")]
		public DateTime? StiftelsesDato { get; set; }

		[JsonProperty("virkningsDato")]
		public DateTime? VirkningsDato { get; set; }

		[JsonProperty("nyesteFadCprnumre")]
		public List<object> NyesteFadCprnumre { get; set; }
	}
}