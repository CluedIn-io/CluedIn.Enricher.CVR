using System;
using System.Text;
using CluedIn.Core;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
	public class Adresse
	{
		[JsonProperty("landekode")]
		public string Landekode { get; set; }

		[JsonProperty("fritekst")]
		public string Fritekst { get; set; }

		[JsonProperty("vejkode")]
		public int Vejkode { get; set; }

		[JsonProperty("kommune")]
		public Kommune Kommune { get; set; }

		[JsonProperty("husnummerFra")]
		public int? HusnummerFra { get; set; }

		[JsonProperty("adresseId")]
		public string AdresseId { get; set; }

		[JsonProperty("sidstValideret")]
		public string SidstValideret { get; set; }

		[JsonProperty("husnummerTil")]
		public int? HusnummerTil { get; set; }

		[JsonProperty("bogstavFra")]
		public string BogstavFra { get; set; }

		[JsonProperty("bogstavTil")]
		public string BogstavTil { get; set; }

		[JsonProperty("etage")]
		public string Etage { get; set; }

		[JsonProperty("sidedoer")]
		public string Sidedoer { get; set; }

		[JsonProperty("conavn")]
		public string Conavn { get; set; }

		[JsonProperty("postboks")]
		public string Postboks { get; set; }

		[JsonProperty("vejnavn")]
		public string Vejnavn { get; set; }

		[JsonProperty("bynavn")]
		public string Bynavn { get; set; }

		[JsonProperty("postnummer")]
		public int Postnummer { get; set; }

		[JsonProperty("postdistrikt")]
		public string Postdistrikt { get; set; }

		[JsonProperty("periode")]
		public Periode Periode { get; set; }

		[JsonProperty("sidstOpdateret")]
		public string SidstOpdateret { get; set; }

		public override string ToString()
		{
			var sb = new StringBuilder();

			if (!string.IsNullOrEmpty(this.Fritekst))
			{
				sb.AppendLine(this.Fritekst);
				sb.AppendLine(this.Landekode);
			}
			else
			{
				if (!string.IsNullOrEmpty(this.Conavn))
					sb.AppendLine("C/O " + this.Conavn);

				if (!string.IsNullOrEmpty(this.Postboks))
					sb.AppendLine("Postboks " + this.Postboks);

				if (this.Vejnavn != null)
				{
					sb.Append(this.Vejnavn);

					if (this.HusnummerFra != null)
						sb.AppendFormat(" {0}", this.HusnummerFra);

					if (this.HusnummerTil != null && this.HusnummerFra != this.HusnummerTil)
						sb.AppendFormat("-{0}", this.HusnummerTil);

					if (this.BogstavFra != null)
						sb.AppendFormat(" {0}", this.BogstavFra);
            
					if (this.BogstavTil != null)
						sb.AppendFormat("-{0}", this.BogstavTil);

					if (this.Etage != null)
						sb.AppendFormat(" {0}", this.Etage);

					if (this.Sidedoer != null)
						sb.AppendFormat(" {0}", this.Sidedoer);

					sb.Append(Environment.NewLine);
				}

				sb.AppendFormat("{0} {1}" + Environment.NewLine, this.Postnummer, this.Postdistrikt ?? this.Bynavn ?? (this.Kommune != null ? this.Kommune.kommuneNavn.ToLower().ToTitleCase() : null));
				sb.AppendLine(this.Landekode);
			}

			return sb.ToString();
		}
	}
}