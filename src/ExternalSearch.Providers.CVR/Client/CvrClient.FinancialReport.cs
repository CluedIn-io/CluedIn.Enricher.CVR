using System;
using System.Linq;
using System.Net;

using CluedIn.ExternalSearch.Providers.CVR.Model;
using CluedIn.ExternalSearch.Providers.CVR.Model.Xbrl;

using RestSharp;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.CVR.Client
{
    public partial class CvrClient
    {
        public Result<Offentliggoerelse> GetFinancialYearlyReport(int cvrNumber)
        {
            var client  = new RestClient("http://distribution.virk.dk/offentliggoerelser/_search");
            var request = new RestRequest { Method = RestSharpCompat.HttpPost };

            var body = $$$"""
                            { "from" : 0, "size" : 1,
                             "query": {
                               "term": {
                                 "cvrNummer": {{{cvrNumber}}}
                                }
                              },
                             "sort" : [
                                { "regnskab.regnskabsperiode.slutDato" : {"order" : "desc"}},
                                { "offentliggoerelsesTidspunkt" : {"order" : "desc"}},
                                { "sidstOpdateret" : {"order" : "desc"}},
                                { "indlaesningsTidspunkt" : {"order" : "desc"}},
                                "_score"
                                ]
                            }
                        """.Trim();



            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = client.Execute(request);
var responseData = response.IsSuccessful
                ? JsonConvert.DeserializeObject<XbrlResponse>(response.Content)
                : null;
            var responseData = JsonConvert.DeserializeObject<XbrlResponse>(response.Content);
            if (responseData is { Hits: not null })
            if (responseData is { Hits: not null })
                var hit = responseData.Hits.hits.FirstOrDefault();
                var hit = responseData.Hits.hits.FirstOrDefault();
                return hit != null ? new Result<Offentliggoerelse>(response.Content, hit.Source) : null;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            throw new Exception(
                $"Could not get financial report - StatusCode: {response.StatusCode}; Message: {response.ErrorMessage}");
        }
    }
}
