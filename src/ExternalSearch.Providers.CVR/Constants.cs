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
        public const string Instruction = """
            [
              {
                "type": "bulleted-list",
                "children": [
                  {
                    "type": "list-item",
                    "children": [
                      {
                        "text": "Add the entity type to specify the golden records you want to enrich. Only golden records belonging to that entity type will be enriched."
                      }
                    ]
                  },
                  {
                    "type": "list-item",
                    "children": [
                      {
                        "text": "Add the vocabulary keys to provide the input for the enricher to search for additional information. For example, if you provide the website vocabulary key for the Web enricher, it will use specific websites to look for information about companies. In some cases, vocabulary keys are not required. If you don't add them, the enricher will use default vocabulary keys."
                      }
                    ]
                  }
                ]
              }
            ]
            """;

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
            Token = new List<Control>()
            {
                new Control()
                {
                    DisplayName = "Accepted Entity Type",
                    Type = "entityTypeSelector",
                    IsRequired = true,
                    Name = KeyName.AcceptedEntityType,
                    Help = "The entity type that defines the golden records you want to enrich (e.g., /Organization)."
                },
                new Control()
                {
                    DisplayName = "Organization Name Vocabulary Key",
                    Type = "vocabularyKeySelector",
                    IsRequired = false,
                    Name = KeyName.OrgNameKey,
                    Help = "The vocabulary key that contains the names of companies you want to enrich (e.g., organization.name)."
                },
                new Control()
                {
                    DisplayName = "Organization Name normalization",
                    Type = "checkbox",
                    IsRequired = false,
                    Name = KeyName.OrgNameNormalization,
                    Help = "Toggle to control the normalization of company names before searching the CVR register. Normalization removes trailing backslashes (\\), slashes (/), and vertical bars (|). Also, it changes the names to lowercase but will not affect names in CluedIn."
                },
                new Control()
                {
                    DisplayName = "Match Past Organization Names",
                    Type = "checkbox",
                    IsRequired = false,
                    Name = KeyName.OrgMatchPastNames,
                    Help = "Toggle to control if the search text (organization name) should match the latest data or any historical records in the CVR register. When disabled, the search will only consider exact matches to the most recent organization name in the register."
                },
                new Control()
                {
                    DisplayName = "CVR Vocabulary Key",
                    Type = "vocabularyKeySelector",
                    IsRequired = false,
                    Name = KeyName.CVRKey,
                    Help = "The vocabulary key that contains the CVR codes of companies you want to enrich (e.g., organization.cvrnumber)."
                },
                new Control()
                {
                    DisplayName = "Country Vocabulary Key",
                    Type = "vocabularyKeySelector",
                    IsRequired = false,
                    Name = KeyName.CountryKey,
                    Help = "The vocabulary key that contains the countries of companies you want to enrich (e.g., organization.country)."
                },
                new Control()
                {
                    DisplayName = "Website Vocabulary Key",
                    Type = "vocabularyKeySelector",
                    IsRequired = false,
                    Name = KeyName.WebsiteKey,
                    Help = "The vocabulary key that contains the websites of companies you want to enrich (e.g., organization.website)."
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

        public static Guide Guide { get; set; } = new Guide
        {
            Instructions = Instruction
        };
        public static IntegrationType IntegrationType { get; set; } = IntegrationType.Enrichment;
    }
}