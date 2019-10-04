using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CluedIn.Core;

using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model
{
    public class FinancialReportSummary
    {
        public int? Year { get; set; }
        public Amount ProfitLossFromOrdinaryActivitiesBeforeTax { get; set; }
        public Amount GrossProfitLoss { get; set; }
        public Amount ProfitLoss { get; set; }
        public Amount Equity { get; set; }
        public Amount LiabilitiesAndEquity { get; set; }
        public DateTimeOffset? DateOfApprovalOfReport { get; set; }

        [JsonIgnore]
        public string Currency
        {
            get
            {
                var units = new HashSet<string>();

                if (this.ProfitLossFromOrdinaryActivitiesBeforeTax != null)
                    units.Add(this.ProfitLossFromOrdinaryActivitiesBeforeTax.Unit);

                if (this.GrossProfitLoss != null)
                    units.Add(this.GrossProfitLoss.Unit);

                if (this.ProfitLoss != null)
                    units.Add(this.ProfitLoss.Unit);

                if (this.Equity != null)
                    units.Add(this.Equity.Unit);

                if (this.LiabilitiesAndEquity != null)
                    units.Add(this.LiabilitiesAndEquity.Unit);

                if (units.Count == 1)
                    return units.First().Replace("iso4217:", string.Empty);

                return null;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Resultat før skat: {0}", this.ProfitLossFromOrdinaryActivitiesBeforeTax.Value);
            sb.AppendLine("Bruttofortjeneste: {0}", this.GrossProfitLoss.Value);
            sb.AppendLine("Årets resultat: {0}", this.ProfitLoss.Value);
            sb.AppendLine("Egenkapital: {0}", this.Equity.Value);
            sb.AppendLine("Balance: {0}", this.LiabilitiesAndEquity.Value);

            return sb.ToString();
        }
    }
}