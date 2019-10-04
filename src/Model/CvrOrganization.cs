using System;
using System.Collections.Generic;

using CluedIn.Core;
using CluedIn.ExternalSearch.Providers.CVR.Model.Cvr;

namespace CluedIn.ExternalSearch.Providers.CVR.Model
{
    public class CvrOrganization
    {
        public CvrOrganization()
        {
            this.AlternateNames = new HashSet<string>();
        }

        public int CvrNumber { get; set; }
        public string Name { get; set; }
        public HashSet<string> AlternateNames { get; set; }

        public string Email { get; set; }
        public string Website { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }

        public string Municipality { get; set; }

        public Adresse Address { get; set; }
        public Adresse PostalAddress { get; set; }

        public bool OptOutSalesAndAdvertising { get; set; }

        public int CompanyTypeCode { get; set; } // 80
        public string CompanyTypeShortName { get; set; } // Aps
        public string CompanyTypeLongName { get; set; } // Anpartsselskab

        public string Status { get; set; } // NORMAL
        public int? CreditStatusCode { get; set; } // 3
        public string CreditStatusText { get; set; }

        public bool IsBankrupt
        {
            get
            {
                return this.CreditStatusCode != null && this.CreditStatusCode.Value == 3;
            }
        }

        public DateTime? FoundingDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string FiscalYearStart { get; set; }
        public string FiscalYearEnd { get; set; }

        public DateTimeOffset? FirstFiscalYearStart { get; set; }
        public DateTimeOffset? FirstFiscalYearEnd { get; set; }

        public IndustryDescription MainIndustry { get; set; }
        public IndustryDescription OtherIndustry1 { get; set; }
        public IndustryDescription OtherIndustry2 { get; set; }
        public IndustryDescription OtherIndustry3 { get; set; }

        public string Purpose { get; set; }
        public string StatutesLastChanged { get; set; }
        public string RegisteredCapital { get; set; }
        public string RegisteredCapitalCurrency { get; set; }
        public string HasShareCapitalClasses { get; set; }

        public Range<int> NumberOfEmployees { get; set; }

        public int? ProductionUnitCount { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
