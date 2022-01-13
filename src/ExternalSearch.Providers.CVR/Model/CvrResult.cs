using System;

using CluedIn.ExternalSearch.Providers.CVR.Model.Xbrl;

using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Model
{
    public class CvrResult
    {
        public CvrResult()
        {
        }

        public CvrResult(int cvrNumber)
        {
            this.CvrNumber = cvrNumber;
        }

        public int CvrNumber { get; set; }
        public CvrOrganization Organization { get; set; }
        public FinancialReportSummary FinancialReportSummary { get; set; }
        public Offentliggoerelse FinancialReport { get; set; }

        public string RawCvrResult { get; set; }
        public string RawCvrFinancialReportResult { get; set; }
        public string RawXbrl { get; set; }

        [JsonIgnore]
        public Exception ErrorException { get; set; }
    }
}