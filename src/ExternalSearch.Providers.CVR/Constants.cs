using System;
using System.Collections.Generic;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;

namespace CluedIn.ExternalSearch.Providers.CVR
{
    public static class Constants
    {
        public const string ComponentName = "CVR";
        public const string ProviderName = "CVR";
        public static readonly Guid ProviderId = Core.Constants.ExternalSearchProviders.CVRId;

        public struct KeyName
        {
            public const string AcceptedEntityType = "acceptedEntityType";
            public const string OrgNameKey = "orgNameKey";
            public const string OrgNameNormalization = "orgNameNormalization";
            public const string OrgMatchPastNames = "orgMatchPastNames";
            public const string CVRKey = "cvrKey";
            public const string CountryKey = "countryKey";
            public const string WebsiteKey = "websiteKey";
        }

        public static string About { get; set; } = "CVR is the Danish state's master register of information about businesses.";
        public static string Icon { get; set; } = "Resources.logo.svg";
        public static string Domain { get; set; } = "https://datacvr.virk.dk/data";

        public static AuthMethods AuthMethods { get; set; } = new AuthMethods
        {
            token = new List<Control>()
            {
                new Control()
                {
                    displayName = "Accepted Entity Type",
                    type = "input",
                    isRequired = true,
                    name = KeyName.AcceptedEntityType
                },
                new Control()
                {
                    displayName = "Organization Name vocab key",
                    type = "input",
                    isRequired = false,
                    name = KeyName.OrgNameKey
                },
                new Control()
                {
                    displayName = "Organization Name normalization",
                    type = "checkbox",
                    isRequired = false,
                    name = KeyName.OrgNameNormalization,
                },
                new Control()
                {
                    displayName = "Match Past Organization Names",
                    type = "checkbox",
                    isRequired = false,
                    name = KeyName.OrgMatchPastNames,
                },
                new Control()
                {
                    displayName = "CVR vocab key",
                    type = "input",
                    isRequired = false,
                    name = KeyName.CVRKey
                },
                new Control()
                {
                    displayName = "Country vocab key",
                    type = "input",
                    isRequired = false,
                    name = KeyName.CountryKey
                },
                new Control()
                {
                    displayName = "Website vocab key",
                    type = "input",
                    isRequired = false,
                    name = KeyName.WebsiteKey
                }
            }
        };

        public static IEnumerable<Control> Properties { get; set; } = new List<Control>()
        {
            // NOTE: Leaving this commented as an example - BF
            //new()
            //{
            //    displayName = "Some Data",
            //    type = "input",
            //    isRequired = true,
            //    name = "someData"
            //}
        };

        public static Guide Guide { get; set; } = null;
        public static IntegrationType IntegrationType { get; set; } = IntegrationType.Enrichment;
    }
}