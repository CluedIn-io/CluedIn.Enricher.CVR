using System;
using System.Linq;
using System.Net;

using CluedIn.ExternalSearch.Providers.CVR.Model;
using CluedIn.ExternalSearch.Providers.CVR.Model.Xbrl;

using RestSharp;

namespace CluedIn.ExternalSearch.Providers.CVR.Client
{
    public partial class CvrClient
    {
        public Result<Offentliggoerelse> GetFinancialYearlyReport(int cvrNumber)
        {
            var client  = new RestClient("http://distribution.virk.dk/offentliggoerelser/_search");
            var request = new RestRequest(Method.POST);

            var body = @"
                { ""from"" : 0, ""size"" : 1,
                  ""query"": {
                    ""term"": {
                      ""cvrNummer"": " + cvrNumber + @"
                    }
                  },
                  ""sort"" : [
                      { ""regnskab.regnskabsperiode.slutDato"" : {""order"" : ""desc""}},
                      { ""offentliggoerelsesTidspunkt"" : {""order"" : ""desc""}},
                      { ""sidstOpdateret"" : {""order"" : ""desc""}},
                      { ""indlaesningsTidspunkt"" : {""order"" : ""desc""}},
                      ""_score""
                  ]
                }
            ".Trim();

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            var response = client.Execute<XbrlResponse>(request);

            if (response.Data != null && response.Data.Hits != null)
            {
                var hit = response.Data.Hits.hits.FirstOrDefault();
                return hit != null ? new Result<Offentliggoerelse>(response.Content, hit.Source) : null;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
                return null;
            else
                throw new Exception(string.Format("Could not get financial report - StatusCode: {0}; Message: {1}", response.StatusCode, response.ErrorMessage));
        }
    }
}
