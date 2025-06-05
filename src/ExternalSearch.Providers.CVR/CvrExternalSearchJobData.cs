using System.Collections.Generic;
using CluedIn.Core.Crawling;

namespace CluedIn.ExternalSearch.Providers.CVR
{
    public class CvrExternalSearchJobData : CrawlJobData
    {
        public CvrExternalSearchJobData(IDictionary<string, object> configuration)
        {
            AcceptedEntityType = GetValue<string>(configuration, Constants.KeyName.AcceptedEntityType);
            OrgNameKey = GetValue<string>(configuration, Constants.KeyName.OrgNameKey);
            OrgNameNormalization = GetValue<bool>(configuration, Constants.KeyName.OrgNameNormalization);
            OrgMatchPastNames = GetValue<bool>(configuration, Constants.KeyName.OrgMatchPastNames);
            CVRKey = GetValue<string>(configuration, Constants.KeyName.CVRKey);
            CountryKey = GetValue<string>(configuration, Constants.KeyName.CountryKey);
            WebsiteKey = GetValue<string>(configuration, Constants.KeyName.WebsiteKey);
        }

        public IDictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>() { 
                { Constants.KeyName.AcceptedEntityType, AcceptedEntityType },
                { Constants.KeyName.OrgNameKey, OrgNameKey },
                { Constants.KeyName.OrgNameNormalization, OrgNameNormalization },
                { Constants.KeyName.OrgMatchPastNames, OrgMatchPastNames },
                { Constants.KeyName.CVRKey, CVRKey },
                { Constants.KeyName.CountryKey, CountryKey },
                { Constants.KeyName.WebsiteKey, WebsiteKey },
            };
        }
        public string AcceptedEntityType { get; set; }
        public string OrgNameKey { get; set; }
        public bool OrgNameNormalization { get; set; }
        public bool OrgMatchPastNames { get; set; }
        public string CVRKey { get; set; }
        public string CountryKey { get; set; }
        public string WebsiteKey { get; set; }
    }
}
