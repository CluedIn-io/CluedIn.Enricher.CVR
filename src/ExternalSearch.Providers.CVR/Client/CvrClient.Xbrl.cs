using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using CluedIn.Core;
using CluedIn.ExternalSearch.Providers.CVR.Model;
using CluedIn.ExternalSearch.Providers.CVR.Model.Xbrl;

using RestSharp;

namespace CluedIn.ExternalSearch.Providers.CVR.Client
{
    public partial class CvrClient
    {
        public Result<FinancialReportSummary> GetXbrlReport(string url)
        {
            // cluedin          - "http://regnskaber.virk.dk/18684563/ZG9rdW1lbnRsYWdlcjovLzAzLzc0LzRjLzk0LzRjLzg0NDItNDY3Zi1hNjI5LWI4MjQ3NGY5YjMzZg.xml"
            // sitecore danmark - "http://regnskaber.virk.dk/46897513/ZG9rdW1lbnRsYWdlcjovLzAzLzJmLzM1L2MxLzIzL2E4ZmMtNDRhNC05ZTU0LWJlZDc3NTI5MmZhYw.xml"

            var client  = new RestClient(url);
            var request = new RestRequest(Method.GET);

            var response = client.Execute<XbrlResponse>(request);

            if (response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.OK)
            {
                using (var stringReader = new StringReader(response.Content))
                using (var reader = new XmlTextReader(stringReader))
                {
                    var doc = XDocument.Load(reader);

                    /*
                        xmlns="http://www.xbrl.org/2003/instance"
                        xmlns:g="http://xbrl.dcca.dk/sob"
                        xmlns:b="http://xbrl.dcca.dk/entryBalanceSheetAccountFormIncomeStatementByNature"
                        xmlns:h="http://xbrl.dcca.dk/mrv"
                        xmlns:f="http://xbrl.dcca.dk/arr"
                        xmlns:d="http://xbrl.dcca.dk/cmn"
                        xmlns:e="http://xbrl.dcca.dk/fsa"
                        xmlns:c="http://xbrl.dcca.dk/gsd"
                        xmlns:xlink="http://www.w3.org/1999/xlink"
                        xmlns:xbrli="http://www.xbrl.org/2003/instance"
                        xmlns:iso4217="http://www.xbrl.org/2003/iso4217"
                        xmlns:xbrldi="http://xbrl.org/2006/xbrldi"
                        xmlns:link="http://www.xbrl.org/2003/linkbase"
                        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
                        xsi:schemaLocation="http://xbrl.dcca.dk/entryBalanceSheetAccountFormIncomeStatementByNature http://archprod.service.eogs.dk/taxonomy/20151001/entryDanishGAAPBalanceSheetAccountFormIncomeStatementByNatureIncludingManagementsReviewStatisticsAndTax20151001.xsd"
                     */

                    XNamespace defaultNs = "http://www.xbrl.org/2003/instance";
                    XNamespace g         = "http://xbrl.dcca.dk/sob";
                    XNamespace b         = "http://xbrl.dcca.dk/entryBalanceSheetAccountFormIncomeStatementByNature";
                    XNamespace h         = "http://xbrl.dcca.dk/mrv";
                    XNamespace f         = "http://xbrl.dcca.dk/arr";
                    XNamespace d         = "http://xbrl.dcca.dk/cmn";
                    XNamespace e         = "http://xbrl.dcca.dk/fsa";
                    XNamespace c         = "http://xbrl.dcca.dk/gsd";
                    XNamespace xlink     = "http://www.w3.org/1999/xlink";
                    XNamespace xbrli     = "http://www.xbrl.org/2003/instance";
                    XNamespace iso4217   = "http://www.xbrl.org/2003/iso4217";
                    XNamespace xbrldi    = "http://xbrl.org/2006/xbrldi";
                    XNamespace link      = "http://www.xbrl.org/2003/linkbase";
                    XNamespace xsi       = "http://www.w3.org/2001/XMLSchema-instance";

                    var namespaceManager = new XmlNamespaceManager(reader.NameTable);
                    namespaceManager.AddNamespace("def", "http://www.xbrl.org/2003/instance");
                    namespaceManager.AddNamespace("e", "http://xbrl.dcca.dk/fsa");
                    namespaceManager.AddNamespace("c", "http://xbrl.dcca.dk/gsd");

                    var contextElements = doc.Root.Elements(defaultNs + "context");
                    var contexts        = new Dictionary<string, string>();

                    foreach (var element in contextElements)
                    {
                        var comment = element.PreviousNode as XComment;

                        if (comment == null)
                            continue;

                        contexts[comment.Value.ToLower()] = element.Attribute("id").Value;
                    }

                    XElement informationOnTypeOfSubmittedReport = doc.Root.Element(c + "InformationOnTypeOfSubmittedReport");

                    string contextName;

                    if (informationOnTypeOfSubmittedReport != null && informationOnTypeOfSubmittedReport.Value == "Årsrapport")
                        contextName = informationOnTypeOfSubmittedReport.Attribute("contextRef").Value;
                    else
                        contexts.TryGetValue("aktuelle periode enkelt selskab", out contextName);

                    contexts.TryGetValue("slutdato aktuelle periode enkelt selskab", out var endPeriodContext);

                    XElement reportingPeriodEndDateElement  = doc.Root.Element(c + "ReportingPeriodEndDate");
                    XElement dateOfApprovalOfReportElement  = doc.Root.Element(c + "DateOfApprovalOfReport");

                    // Resultat før skat
                    XElement profitLossFromOrdinaryActivitiesBeforeTax = doc.Root.Element(e + "ProfitLossFromOrdinaryActivitiesBeforeTax");

                    // Bruttofortjeneste 
                    XElement grossProfitLoss = doc.Root.Element(e + "GrossProfitLoss");

                    // Årets resultat
                    XElement profitLoss = doc.Root.XPathSelectElement("e:ProfitLoss[@contextRef = '" + contextName + "']", namespaceManager);

                    // Egenkapital
                    XElement equity = doc.Root.XPathSelectElement("e:Equity[@contextRef = '" + endPeriodContext + "']", namespaceManager);

                    // Balance
                    XElement liabilitiesAndEquity = doc.Root.XPathSelectElement("e:LiabilitiesAndEquity[@contextRef = '" + endPeriodContext + "']", namespaceManager);

                    var report = new FinancialReportSummary();

                    DateTimeOffset reportingPeriodEndDate = DateTimeOffset.MinValue;
                    DateTimeOffset dateOfApprovalOfReport = DateTimeOffset.MinValue;

                    if (reportingPeriodEndDateElement != null)
                        DateTimeOffset.TryParse(reportingPeriodEndDateElement.Value, out reportingPeriodEndDate);

                    if (dateOfApprovalOfReportElement != null)
                        DateTimeOffset.TryParse(dateOfApprovalOfReportElement.Value, out dateOfApprovalOfReport);

                    report.ProfitLossFromOrdinaryActivitiesBeforeTax    = this.GetAmount(profitLossFromOrdinaryActivitiesBeforeTax, namespaceManager);
                    report.GrossProfitLoss                              = this.GetAmount(grossProfitLoss, namespaceManager);
                    report.ProfitLoss                                   = this.GetAmount(profitLoss, namespaceManager);
                    report.Equity                                       = this.GetAmount(equity, namespaceManager);
                    report.LiabilitiesAndEquity                         = this.GetAmount(liabilitiesAndEquity, namespaceManager);
                    report.Year                                         = reportingPeriodEndDate != DateTimeOffset.MinValue ? (int?)reportingPeriodEndDate.Year : null;
                    report.DateOfApprovalOfReport                       = dateOfApprovalOfReport != DateTimeOffset.MinValue ? (DateTimeOffset?)dateOfApprovalOfReport : null;

                    return new Result<FinancialReportSummary>(response.Content, report);
                }
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
                return null;
            else
                throw new Exception(string.Format("Could not get xbrl report - StatusCode: {0}; Message: {1}", response.StatusCode, response.ErrorMessage));
        }

        private Amount GetAmount(XElement value, XmlNamespaceManager namespaceManager)
        {
            if (value == null)
                return null;

            var v        = value.Value;
            var unitRef  = value.Attribute("unitRef").Value;

            string decimals = "0";
            if (value.Attribute("decimals") != null)
                decimals = value.Attribute("decimals").Value;

            var measure = value.Document.Root.XPathSelectElement("def:unit[@id = '" + unitRef + "']/def:measure", namespaceManager);

            return new Amount()
                       {
                           Value = long.Parse(v),
                           Decimals = int.Parse(decimals),
                           Unit = measure.Value
                       };
        }
    }
}
