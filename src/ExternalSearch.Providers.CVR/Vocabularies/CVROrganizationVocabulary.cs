// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CvrOrganizationVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the CvrOrganizationVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.CVR.Vocabularies
{
    /// <summary>The CVR organization vocabulary</summary>
    /// <seealso cref="CluedIn.Core.Data.Vocabularies.SimpleVocabulary" />
    public class CvrOrganizationVocabulary : SimpleVocabulary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CvrOrganizationVocabulary"/> class.
        /// </summary>
        public CvrOrganizationVocabulary()
        {
            this.VocabularyName = "CVR Organization";
            this.KeyPrefix      = "cvr.organization";
            this.KeySeparator   = ".";
            this.Grouping       = EntityType.Organization;

            this.AddGroup("Metadata", group => 
            {
                this.Name                               = group.Add(new VocabularyKey("name"));
                this.CvrNumber                          = group.Add(new VocabularyKey("cvrNumber",                      VocabularyKeyDataType.Integer));

                this.OptOutSalesAndAdvertising          = group.Add(new VocabularyKey("optOutSalesAndAdvertising",      VocabularyKeyDataType.Boolean))
                                                               .WithDescription("See https://datacvr.virk.dk/data/cvr-hj%C3%A6lp/f%C3%A5-hj%C3%A6lp-til-cvr/reklamebeskyttelse");

                this.NumberOfEmployees                  = group.Add(new VocabularyKey("numberOfEmployees"));

                this.StartDate                          = group.Add(new VocabularyKey("startDate",                      VocabularyKeyDataType.DateTime));
                this.EndDate                            = group.Add(new VocabularyKey("endDate",                        VocabularyKeyDataType.DateTime));
                this.FoundingDate                       = group.Add(new VocabularyKey("foundingDate",                   VocabularyKeyDataType.DateTime));

                this.CompanyTypeCode                    = group.Add(new VocabularyKey("companyTypeCode",                                                                VocabularyKeyVisibility.Hidden));
                this.CompanyTypeShortName               = group.Add(new VocabularyKey("companyTypeShortName",                                                           VocabularyKeyVisibility.Hidden));
                this.CompanyTypeLongName                = group.Add(new VocabularyKey("companyTypeLongName"));

                this.Status                             = group.Add(new VocabularyKey("status"));

                this.CreditStatusCode                   = group.Add(new VocabularyKey("creditStatusCode",               VocabularyKeyDataType.Number,                   VocabularyKeyVisibility.Hidden));
                this.CreditStatusText                   = group.Add(new VocabularyKey("creditStatusText"));
                this.IsBankrupt                         = group.Add(new VocabularyKey("isBankrupt",                     VocabularyKeyDataType.Boolean));
            });

            this.AddGroup("Contact", group => 
            {
                this.PhoneNumber                        = group.Add(new VocabularyKey("phoneNumber",                    VocabularyKeyDataType.PhoneNumber));
                this.FaxNumber                          = group.Add(new VocabularyKey("faxNumber",                      VocabularyKeyDataType.PhoneNumber));
                this.Email                              = group.Add(new VocabularyKey("email",                          VocabularyKeyDataType.Email));
                this.Website                            = group.Add(new VocabularyKey("website",                        VocabularyKeyDataType.Uri));
            });

            this.AddGroup("Location", group => 
            {
                this.Address                            = group.Add(new CvrAddressVocabulary().AsCompositeKey("address"));
                this.PostalAddress                      = group.Add(new CvrAddressVocabulary().AsCompositeKey("postalAddress",  VocabularyKeyVisibility.Hidden));
            });

            this.AddGroup("Details", group => 
            {
                this.Municipality                       = this.Add(new VocabularyKey("municipality",                    VocabularyKeyDataType.GeographyLocation));

                this.MainIndustryCode                   = group.Add(new VocabularyKey("mainIndustryCode",               VocabularyKeyDataType.Number));
                this.MainIndustryDescription            = group.Add(new VocabularyKey("mainIndustryDescription"));
                this.OtherIndustry1Code                 = group.Add(new VocabularyKey("otherIndustry1.code",            VocabularyKeyDataType.Number));
                this.OtherIndustry1Description          = group.Add(new VocabularyKey("otherIndustry1.description"));   
                this.OtherIndustry2Code                 = group.Add(new VocabularyKey("otherIndustry2.code",            VocabularyKeyDataType.Number));
                this.OtherIndustry2Description          = group.Add(new VocabularyKey("otherIndustry2.description"));   
                this.OtherIndustry3Code                 = group.Add(new VocabularyKey("otherIndustry3.code",            VocabularyKeyDataType.Number));
                this.OtherIndustry3Description          = group.Add(new VocabularyKey("otherIndustry3.description"));   

                this.ProductionUnitCount                = group.Add(new VocabularyKey("productionUnitCount",            VocabularyKeyDataType.Integer));
                this.Purpose                            = group.Add(new VocabularyKey("purpose"));

                this.HasShareCapitalClasses             = group.Add(new VocabularyKey("hasShareCapitalClasses",         VocabularyKeyDataType.Boolean));
                this.RegisteredCapital                  = group.Add(new VocabularyKey("registeredCapital",              VocabularyKeyDataType.Money));
                this.RegisteredCapitalCurrency          = group.Add(new VocabularyKey("registeredCapitalCurrency",      VocabularyKeyDataType.Currency));
                this.StatutesLastChanged                = group.Add(new VocabularyKey("statutesLastChanged",            VocabularyKeyDataType.DateTime));
            });

            this.AddGroup("Financial Year", group => 
            {
                this.FiscalYearStart                    = group.Add(new VocabularyKey("fiscalYearStart"))
                                                               .WithDescription("Date in the format MM-DD");
                this.FiscalYearEnd                      = group.Add(new VocabularyKey("fiscalYearEnd"))
                                                               .WithDescription("Date in the format MM-DD");
                this.FirstFiscalYearStart               = group.Add(new VocabularyKey("firstFiscalYearStart",           VocabularyKeyDataType.DateTime));
                this.FirstFiscalYearEnd                 = group.Add(new VocabularyKey("firstFiscalYearEnd",             VocabularyKeyDataType.DateTime));
            });

            this.AddGroup("Financial Summary", group => 
            {
                this.FinancialReportSummaryYear                                         = group.Add(new VocabularyKey("financialReportSummary.year",                                        VocabularyKeyDataType.Number));
                this.FinancialReportSummaryEquity                                       = group.Add(new VocabularyKey("financialReportSummary.equity",                                      VocabularyKeyDataType.Money));
                this.FinancialReportSummaryGrossProfitLoss                              = group.Add(new VocabularyKey("financialReportSummary.grossProfitLoss",                             VocabularyKeyDataType.Money));
                this.FinancialReportSummaryLiabilitiesAndEquity                         = group.Add(new VocabularyKey("financialReportSummary.liabilitiesAndEquity",                        VocabularyKeyDataType.Money));
                this.FinancialReportSummaryProfitLoss                                   = group.Add(new VocabularyKey("financialReportSummary.profitLoss",                                  VocabularyKeyDataType.Money));
                this.FinancialReportSummaryProfitLossFromOrdinaryActivitiesBeforeTax    = group.Add(new VocabularyKey("financialReportSummary.profitLossFromOrdinaryActivitiesBeforeTax",   VocabularyKeyDataType.Money));
                this.FinancialReportSummaryCurrency                                     = group.Add(new VocabularyKey("financialReportSummary.currency",                                    VocabularyKeyDataType.Currency));
                this.FinancialReportSummaryPdfLink                                      = group.Add(new VocabularyKey("financialReportSummary.link",                                        VocabularyKeyDataType.Uri));
            });

        }

        public VocabularyKey CvrNumber { get; protected set; }

        public VocabularyKey CompanyTypeCode { get; protected set; }
        public VocabularyKey CompanyTypeLongName { get; protected set; }
        public VocabularyKey CompanyTypeShortName { get; protected set; }
        public VocabularyKey CreditStatusCode { get; protected set; }
        public VocabularyKey CreditStatusText { get; protected set; }
        public VocabularyKey Email { get; protected set; }
        public VocabularyKey EndDate { get; protected set; }
        public VocabularyKey FaxNumber { get; protected set; }
        public VocabularyKey FirstFiscalYearEnd { get; protected set; }
        public VocabularyKey FirstFiscalYearStart { get; protected set; }
        public VocabularyKey FiscalYearEnd { get; protected set; }
        public VocabularyKey FiscalYearStart { get; protected set; }
        public VocabularyKey FoundingDate { get; protected set; }
        public VocabularyKey IsBankrupt { get; protected set; }
        public VocabularyKey MainIndustryCode { get; protected set; }
        public VocabularyKey MainIndustryDescription { get; protected set; }
        public VocabularyKey Municipality { get; protected set; }
        public VocabularyKey Name { get; protected set; }
        public VocabularyKey NumberOfEmployees { get; protected set; }
        public VocabularyKey OptOutSalesAndAdvertising { get; protected set; }
        public VocabularyKey OtherIndustry1Code { get; protected set; }
        public VocabularyKey OtherIndustry1Description { get; protected set; }
        public VocabularyKey OtherIndustry2Code { get; protected set; }
        public VocabularyKey OtherIndustry2Description { get; protected set; }
        public VocabularyKey OtherIndustry3Code { get; protected set; }
        public VocabularyKey OtherIndustry3Description { get; protected set; }
        public VocabularyKey PhoneNumber { get; protected set; }
        public VocabularyKey ProductionUnitCount { get; protected set; }
        public VocabularyKey Purpose { get; protected set; }
        public VocabularyKey RegisteredCapital { get; protected set; }
        public VocabularyKey RegisteredCapitalCurrency { get; protected set; }
        public VocabularyKey HasShareCapitalClasses { get; protected set; }
        public VocabularyKey StartDate { get; protected set; }
        public VocabularyKey Status { get; protected set; }
        public VocabularyKey StatutesLastChanged { get; protected set; }
        public VocabularyKey Website { get; protected set; }

        public VocabularyKey FinancialReportSummaryYear { get; protected set; }
        public VocabularyKey FinancialReportSummaryEquity { get; protected set; }
        public VocabularyKey FinancialReportSummaryGrossProfitLoss { get; protected set; }
        public VocabularyKey FinancialReportSummaryLiabilitiesAndEquity { get; protected set; }
        public VocabularyKey FinancialReportSummaryProfitLoss { get; protected set; }
        public VocabularyKey FinancialReportSummaryProfitLossFromOrdinaryActivitiesBeforeTax { get; protected set; }
        public VocabularyKey FinancialReportSummaryCurrency { get; protected set; }
        public VocabularyKey FinancialReportSummaryPdfLink { get; protected set; }

        public CvrAddressVocabulary Address { get; protected set; }
        public CvrAddressVocabulary PostalAddress { get; protected set; }
    }
}
