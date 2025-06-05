using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

using CluedIn.Core;
using CluedIn.ExternalSearch.Providers.CVR.Model;
using CluedIn.ExternalSearch.Providers.CVR.Model.Cvr;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

namespace CluedIn.ExternalSearch.Providers.CVR.Client
{
    public partial class CvrClient
    {
        public Result<CvrOrganization> GetCompanyByCvrNumber(int cvrNumber)
        {
            return CallCvr(uri => GetCompanyByCvrNumber(cvrNumber, uri));
        }

        public IEnumerable<Result<CvrOrganization>> GetCompanyByName(string name, bool matchPastNames)
        {
            return CallCvr(uri => GetCompanyByName(name, uri, matchPastNames));
        }

        private T CallCvr<T>(Func<Uri, T> searchFunc)
        {
            // var uri = new Uri("http://CluedIn_CVR_I_SKYEN:cb4821e8-bee2-45fe-8b9d-27cd6c4eff66@distribution.virk.dk/cvr-permanent/_search");

            var endpoint        = ConfigurationManager.AppSettings["Providers.ExternalSearch.CVR.EndPoint"];
            var endpointLive    = ConfigurationManager.AppSettings["Providers.ExternalSearch.CVR.LiveEndPoint"] ?? "http://CluedIn_CVR_I_SKYEN:cb4821e8-bee2-45fe-8b9d-27cd6c4eff66@distribution.virk.dk/cvr-permanent/_search";

            T result = default(T);

            if (!string.IsNullOrEmpty(endpoint))
                result = searchFunc(new Uri(endpoint));

            if (result != null)
                return result;

            if (!string.IsNullOrEmpty(endpointLive))
                result = searchFunc(new Uri(endpointLive));

            return result;
        }

        public Result<CvrOrganization> GetCompanyByCvrNumber(int cvrNumber, Uri endPoint)
        {

            //var body = @"
            //    { ""from"" : 0, ""size"" : 1,
            //      ""query"": {
            //        ""term"": {
            //          ""Vrvirksomhed.cvrNummer"": " + cvrNumber + @"
            //        }
            //      }
            //    }
            //".Trim();

            var body =
                JsonUtility.Serialize(new Body() {
                    From = 0,
                    Size = 1,
                    Query = new Query() {
                        QueryString = new QueryString {
                            Query = cvrNumber,
                            Fields = ["Vrvirksomhed.cvrNummer"]
                        }
                    }
                });

            return GetCompany(body, endPoint, string.Empty, false);
        }

        public IEnumerable<Result<CvrOrganization>> GetCompanyByName(string name, Uri endPoint, bool matchPastNames)
        {
            var body =
                 JsonUtility.Serialize(new Body {
                     From = 0,
                     Size = 50,
                     Query = new Query {
                         QueryString = new QueryString {
                             Query = JsonConvert.ToString(name),
                             Fields =
                             [
                                 matchPastNames
                                     ? "Vrvirksomhed.navne.navn"
                                     : "Vrvirksomhed.virksomhedMetadata.nyesteNavn.navn"
                             ]
                         }
                     }
                 });

            return GetCompanies(body, endPoint, name, matchPastNames);
        }

        private Result<CvrOrganization> GetCompany(string queryBody, Uri endPoint, string name, bool matchPastNames)
        {
            return GetCompanyResult(
                queryBody, 
                endPoint,
                (hits, response, json, _, _) =>
                    {
                        var hit = hits.FirstOrDefault();

                        if (hit == null)
                            return null;

                        var result = CreateCompanyResult(hit, json);

                        if (result != null)
                            result.RawContent = response.Content;

                        return result;
                    },
                name,
                matchPastNames
            );
        }

        private IEnumerable<Result<CvrOrganization>> GetCompanies(string queryBody, Uri endPoint, string name, bool matchPastNames)
        {
            return GetCompanyResult(
                queryBody, 
                endPoint,
                BuildHits,
                name,
                matchPastNames
            );
        }

        private IEnumerable<Result<CvrOrganization>> BuildHits(IEnumerable<Hit> hits, IRestResponse<CompanyResult> response, JObject json, string name, bool matchPastNames)
        {
            if (hits == null || string.IsNullOrEmpty(name)) yield break;

            var hit = matchPastNames ? hits.FirstOrDefault(e => e._source.Vrvirksomhed.navne.Any(navne => navne.Navn.Equals(name, StringComparison.InvariantCultureIgnoreCase))) : hits.FirstOrDefault(e => e._source.Vrvirksomhed.virksomhedMetadata.NyesteNavn.Navn.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            
            yield return CreateCompanyResult(hit, json);
        }

        private T GetCompanyResult<T>(string queryBody, Uri endPoint, Func<IEnumerable<Hit>, IRestResponse<CompanyResult>, JObject, string, bool, T> resultFunc, string name, bool matchPastNames)
        {
            var client = new RestClient(endPoint);

            var request = new RestRequest(Method.POST);

            var userInfo = endPoint.UserInfo;
            if (!string.IsNullOrEmpty(userInfo))
            {
                var parts = userInfo.Split(':');

                request.Credentials = new NetworkCredential(parts[0], parts[1]);
            }

            var body = queryBody.Trim();

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            var response = client.Execute<CompanyResult>(request);

            if (response.Data is { hits: not null })

            {
                var json = JObject.Parse(response.Content);

                return resultFunc(response.Data?.hits?.hits, response, json, name, matchPastNames);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;

            throw new Exception(
                $"Could not get cvr company - StatusCode: {response.StatusCode}; Message: {response.ErrorMessage}");
        }

        private DateTime? GetLastUpdated(JObject vrvirksomhedNode)
        {
            var lastUpdatedNodes = vrvirksomhedNode.SelectTokens("$..sidstOpdateret");

            var lastUpdated = lastUpdatedNodes.Select(
                t =>
                    {
                        if (!string.IsNullOrEmpty(t.Value<string>()) && DateTime.TryParse(t.Value<string>(), out var date))
                             return date as DateTime?;

                        return null;
                    })
                .OrderByDescending(v => v)
                .FirstOrDefault();

            return lastUpdated;
        }

        private JObject GetOrganizationJsonToken(JObject json, int cvrNumber)
        {
            var vrvirksomhedNode = json.SelectTokens("$..Vrvirksomhed")
                .FirstOrDefault(t => t is JObject jObject && (int)jObject.Property("cvrNummer")?.Value == cvrNumber);

            return vrvirksomhedNode as JObject;
        }

        private Result<CvrOrganization> CreateCompanyResult(Hit hit, JObject responseJson)
        {
            var org                 = hit?._source?.Vrvirksomhed;
            if (org == null) return null;

            var vrvirksomhedNode    = GetOrganizationJsonToken(responseJson, org.cvrNummer);
            var lastUpdated         = GetLastUpdated(vrvirksomhedNode);

            // TODO
            return new Result<CvrOrganization>(responseJson.ToString(), CreateCvrResult(org, lastUpdated));
        }

        private CvrOrganization CreateCvrResult(Vrvirksomhed org, DateTimeOffset? lastUpdated)
        {
            var result = new CvrOrganization
            {
                CvrNumber = org.cvrNummer,
                Name = org.virksomhedMetadata.NyesteNavn.Navn,
                ModifiedDate = lastUpdated
            };

            foreach (var name in org.navne ?? [])
                result.AlternateNames.Add(name.Navn);

            foreach (var name in org.attributter.Where(a => a.Type == "NAVN_IDENTITET").SelectMany(a => a.Vaerdier))
                result.AlternateNames.Add(name.Vaerdi);

            if (result.Name != null)
                result.AlternateNames.Remove(result.Name);

            var date = DateTime.UtcNow;
            var life = org.livsforloeb.OrderByDescending(l => l.periode.GyldigFra).FirstOrDefault();

            if (org.virksomhedMetadata.NyesteStatus != null)
            {
                result.CreditStatusCode = org.virksomhedMetadata.NyesteStatus.Kreditoplysningkode;
                result.CreditStatusText = org.virksomhedMetadata.NyesteStatus.Kreditoplysningtekst;
            }

            result.Status                       = org.virksomhedMetadata.SammensatStatus;
            result.FoundingDate                 = org.virksomhedMetadata.StiftelsesDato;
            result.StartDate                    = life != null ? life.periode.GyldigFra : result.FoundingDate;
            result.EndDate                      = life?.periode.GyldigTil;

            if (result.EndDate != null)
                date = result.EndDate.Value;

            result.Email                        = GetCurrentValue(org.elektroniskPost, e => e.Periode, e => !e.Hemmelig, e => e.Kontaktoplysning, date);
            result.Website                      = GetCurrentValue(org.hjemmeside, e => e.Periode, e => !e.Hemmelig, e => e.Kontaktoplysning, date);
            result.PhoneNumber                  = GetCurrentValue(org.telefonNummer, e => e.Periode, e => !e.Hemmelig, e => e.Kontaktoplysning, date);
            result.FaxNumber                    = GetCurrentValue(org.telefaxNummer, e => e.Periode, e => !e.Hemmelig, e => e.Kontaktoplysning, date);

            result.Address                      = org.virksomhedMetadata.NyesteBeliggenhedsadresse ?? GetCurrentValue(org.beliggenhedsadresse, i => i.Periode, date);
            result.PostalAddress                = GetCurrentValue(org.postadresse, i => i.Periode, date);
            result.Municipality                 = result.Address?.Kommune.kommuneNavn;

            result.OptOutSalesAndAdvertising    = org.reklamebeskyttet;

            result.CompanyTypeCode              = org.virksomhedMetadata.NyesteVirksomhedsform?.Virksomhedsformkode ?? 0;
            result.CompanyTypeLongName          = org.virksomhedMetadata.NyesteVirksomhedsform?.LangBeskrivelse;
            result.CompanyTypeShortName         = org.virksomhedMetadata.NyesteVirksomhedsform?.KortBeskrivelse;

            result.FiscalYearStart              = GetCurrentValue(org.attributter.Where(a => a.Type == "REGNSKABSÅR_START").SelectMany(a => a.Vaerdier), v => v.Periode, _ => true, v => v.Vaerdi, date);
            result.FiscalYearEnd                = GetCurrentValue(org.attributter.Where(a => a.Type == "REGNSKABSÅR_SLUT").SelectMany(a => a.Vaerdier), v => v.Periode, _ => true, v => v.Vaerdi, date);

            result.FirstFiscalYearStart         = GetCurrentValue(org.attributter.Where(a => a.Type == "FØRSTE_REGNSKABSPERIODE_START").SelectMany(a => a.Vaerdier), v => v.Periode, v => DateTimeOffset.TryParse(v.Vaerdi, out _), v => (DateTimeOffset?)DateTimeOffset.Parse(v.Vaerdi), date);
            result.FirstFiscalYearEnd           = GetCurrentValue(org.attributter.Where(a => a.Type == "FØRSTE_REGNSKABSPERIODE_SLUT").SelectMany(a => a.Vaerdier), v => v.Periode, v => DateTimeOffset.TryParse(v.Vaerdi, out _), v => (DateTimeOffset?)DateTimeOffset.Parse(v.Vaerdi), date);

            result.Purpose                      = GetCurrentValue(org.attributter.Where(a => a.Type == "FORMÅL").SelectMany(a => a.Vaerdier), v => v.Periode, _ => true, v => v.Vaerdi, date);
            result.RegisteredCapital            = GetCurrentValue(org.attributter.Where(a => a.Type == "KAPITAL").SelectMany(a => a.Vaerdier), v => v.Periode, _ => true, v => v.Vaerdi, date);
            result.RegisteredCapitalCurrency    = GetCurrentValue(org.attributter.Where(a => a.Type == "KAPITALVALUTA").SelectMany(a => a.Vaerdier), v => v.Periode, _ => true, v => v.Vaerdi, date);
            result.StatutesLastChanged          = GetCurrentValue(org.attributter.Where(a => a.Type == "VEDTÆGT_SENESTE").SelectMany(a => a.Vaerdier), v => v.Periode, _ => true, v => v.Vaerdi, date);
            result.HasShareCapitalClasses       = GetCurrentValue(org.attributter.Where(a => a.Type == "KAPITALKLASSER").SelectMany(a => a.Vaerdier), v => v.Periode, _ => true, v => v.Vaerdi, date);

            result.MainIndustry                 = org.virksomhedMetadata.NyesteHovedbranche != null ? new IndustryDescription(org.virksomhedMetadata.NyesteHovedbranche) : null;
            result.OtherIndustry1               = org.virksomhedMetadata.NyesteBibranche1 != null ? new IndustryDescription(org.virksomhedMetadata.NyesteBibranche1) : null;
            result.OtherIndustry2               = org.virksomhedMetadata.NyesteBibranche2 != null ? new IndustryDescription(org.virksomhedMetadata.NyesteBibranche2) : null;
            result.OtherIndustry3               = org.virksomhedMetadata.NyesteBibranche3 != null ? new IndustryDescription(org.virksomhedMetadata.NyesteBibranche3) : null;

            result.NumberOfEmployees            = GetEmploymentRange(org);

            return result;
        }

        private Range<int> GetEmploymentRange(Vrvirksomhed org)
        {
            var latestEmployment = DateTime.MinValue;
            Range<int> employmentRange = null;

            if (org.virksomhedMetadata.NyesteAarsbeskaeftigelse != null)
            {
                var employment = org.virksomhedMetadata.NyesteAarsbeskaeftigelse;
                var date = new DateTime(employment.Aar, 1, 1);

                var rangeText = employment.IntervalKodeAntalAnsatte;
                var rangeMatch = Regex.Match(rangeText, @"^ANTAL_(?<from>\d+)_(?<to>\d+)$");

                if (rangeMatch.Success && date > latestEmployment)
                {
                    employmentRange  = new Range<int>(int.Parse(rangeMatch.Groups["from"].Value), int.Parse(rangeMatch.Groups["to"].Value));
                    latestEmployment = date;
                }
            }

            if (org.virksomhedMetadata.NyesteKvartalsbeskaeftigelse != null)
            {
                var employment = org.virksomhedMetadata.NyesteKvartalsbeskaeftigelse;
                var date = new DateTime(employment.Aar, employment.Kvartal * 3, 1);

                var rangeText = employment.IntervalKodeAntalAnsatte;
                if (rangeText != null)
                {
                    var rangeMatch = Regex.Match(rangeText, @"^ANTAL_(?<from>\d+)_(?<to>\d+)$");

                    if (rangeMatch.Success && date > latestEmployment)
                    {
                        employmentRange  = new Range<int>(int.Parse(rangeMatch.Groups["from"].Value), int.Parse(rangeMatch.Groups["to"].Value));
                        latestEmployment = date;
                    }
                }
            }

            if (org.virksomhedMetadata.NyesteMaanedsbeskaeftigelse != null)
            {
                var employment = org.virksomhedMetadata.NyesteMaanedsbeskaeftigelse;
                var date = new DateTime(employment.Aar, employment.Maaned, 1);

                var rangeText = employment.IntervalKodeAntalAnsatte;
                var rangeMatch = Regex.Match(rangeText, @"^ANTAL_(?<from>\d+)_(?<to>\d+)$");

                if (rangeMatch.Success && date > latestEmployment)
                {
                    employmentRange  = new Range<int>(int.Parse(rangeMatch.Groups["from"].Value), int.Parse(rangeMatch.Groups["to"].Value));
                }
            }

            return employmentRange;
        }

        private T2 GetCurrentValue<T, T2>(IEnumerable<T> items, Func<T, Periode> periodSelector, Func<T, bool> filter, Func<T, T2> selector, DateTime date)
            where T : class
        {
            return GetCurrentValue(items, periodSelector, filter, selector, default, date);
        }

        private T2 GetCurrentValue<T, T2>(IEnumerable<T> items, Func<T, Periode> periodSelector, Func<T, bool> filter, Func<T, T2> selector, T2 defaultValue, DateTime date)
            where T : class
        {
            var currentValue = GetCurrentValue(items, periodSelector, date);

            if (currentValue == null || !filter(currentValue))
                return defaultValue;

            return selector(currentValue);
        }

        private T GetCurrentValue<T>(IEnumerable<T> items, Func<T, Periode> selector, DateTime date)
            where T : class
        {
            var currentValue = items?.FirstOrDefault(i => IsCurrent(selector(i), date));

            return currentValue;
        }

        private bool IsCurrent(Periode period, DateTime date)
        {
            return period.GyldigFra <= date && (!period.GyldigTil.HasValue || period.GyldigTil.Value >= date);
        }
    }
}
