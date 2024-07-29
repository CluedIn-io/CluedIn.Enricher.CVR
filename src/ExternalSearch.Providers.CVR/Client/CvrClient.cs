using System;
using System.Collections.Generic;
using System.Linq;

using CluedIn.Core;
using CluedIn.ExternalSearch.Providers.CVR.Model;

namespace CluedIn.ExternalSearch.Providers.CVR.Client
{
    public partial class CvrClient
    {
        public CvrResult GetCvrResult(int cvrNumber)
        {
            var result = new CvrResult(cvrNumber);

            try
            {
                var organization = ActionExtensions.ExecuteWithRetry(() => this.GetCompanyByCvrNumber(cvrNumber), isTransient: ex => ex.IsTransient());

                if (organization == null)
                    return null;

                result.RawCvrResult = organization.RawContent;
                result.Organization = organization.Data;

                result = this.GetAdditionalData(result);
            }
            catch (Exception ex)
            {
                result.ErrorException = ex;
            }

            return result;
        }

        public IEnumerable<CvrResult> GetCvrResultsByName(string name, bool matchPastNames)
        {
            var organizations = ActionExtensions.ExecuteWithRetry(() => this.GetCompanyByName(name, matchPastNames), isTransient: ex => ex.IsTransient());

            if (organizations == null)
                yield break;

            foreach (var organization in organizations)
            {
                var result = new CvrResult(organization.Data.CvrNumber);

                try
                {
                    result.RawCvrResult = organization.RawContent;
                    result.Organization = organization.Data;

                    result = this.GetAdditionalData(result);
                }
                catch (Exception ex)
                {
                    result.ErrorException = ex;
                }

                yield return result;
            }
        }

        private CvrResult GetAdditionalData(CvrResult result)
        {
            int cvrNumber = result.Organization.CvrNumber;

            var financialReport = ActionExtensions.ExecuteWithRetry(() => this.GetFinancialYearlyReport(cvrNumber), isTransient: ex => ex.IsTransient());

            if (financialReport == null)
                return result;

            result.RawCvrFinancialReportResult  = financialReport.RawContent;
            result.FinancialReport              = financialReport.Data;

            var document = financialReport.Data.Dokumenter.FirstOrDefault(d => d.DokumentType == "AARSRAPPORT" && d.DokumentMimeType == "application/xml");

            if (document != null)
            {
                try
                {
                    var xbrlReport = ActionExtensions.ExecuteWithRetry(() => this.GetXbrlReport(document.DokumentUrl), isTransient: ex => ex.IsTransient());

                    result.RawXbrl                  = xbrlReport.RawContent;
                    result.FinancialReportSummary   = xbrlReport.Data;
                }
                catch { }
            }

            return result;
        }
    }
}
