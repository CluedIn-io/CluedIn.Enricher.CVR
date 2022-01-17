using System;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Periode
	{
		[JsonProperty("gyldigFra")]
		[JsonConverter(typeof(YearMonthDayDateConverter))]
		public DateTime GyldigFra { get; set; }

		[JsonProperty("gyldigTil")]
		[JsonConverter(typeof(YearMonthDayDateConverter))]
		public DateTime? GyldigTil { get; set; }
	}
}