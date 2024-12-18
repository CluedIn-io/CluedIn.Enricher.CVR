﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CVRExternalSearchProvider.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the CvrExternalSearchProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Data.Vocabularies;
using CluedIn.Core.ExternalSearch;
using CluedIn.Core.Providers;
using CluedIn.Crawling.Helpers;
using CluedIn.ExternalSearch.Filters;
using CluedIn.ExternalSearch.Providers.CVR.Client;
using CluedIn.ExternalSearch.Providers.CVR.Model;
using CluedIn.ExternalSearch.Providers.CVR.Model.Cvr;
using CluedIn.ExternalSearch.Providers.CVR.Net;
using CluedIn.ExternalSearch.Providers.CVR.Vocabularies;
using CluedIn.Processing.EntityResolution;

using EntityType = CluedIn.Core.Data.EntityType;

namespace CluedIn.ExternalSearch.Providers.CVR
{
    /// <summary>The CVR external search provider</summary>
    /// <seealso cref="CluedIn.ExternalSearch.ExternalSearchProviderBase" />
    public class CvrExternalSearchProvider : ExternalSearchProviderBase, IExtendedEnricherMetadata, IConfigurableExternalSearchProvider
    {
        private static readonly EntityType[] DefaultAcceptedEntityTypes = { };

        /**********************************************************************************************************
         * CONSTRUCTORS
         **********************************************************************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="CvrExternalSearchProvider" /> class.
        /// </summary>
        public CvrExternalSearchProvider()
            : base(Constants.ProviderId, DefaultAcceptedEntityTypes)
        {
        }

        /**********************************************************************************************************
         * METHODS
         **********************************************************************************************************/

        public override IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query) => throw new NotSupportedException();

        public override IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request) => throw new NotSupportedException();

        public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request) => throw new NotSupportedException();

        public override IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request) => throw new NotSupportedException();

        public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request) => throw new NotSupportedException();

        private IEnumerable<IExternalSearchQuery> InternalBuildQueries(ExecutionContext context, IExternalSearchRequest request, IDictionary<string, object> config = null)
        {
            if (!Accepts(config, request.EntityMetaData.EntityType))
                yield break;
            
            var jobData = new CvrExternalSearchJobData(config);

            var existingResults = request.GetQueryResults<CvrResult>(this).ToList();

            var postFixes = new[] { "A/S", "AS", "ApS", "IVS", "I/S", "IS", "K/S", "KS", "G/S", "GS", "P/S", "PS", "Enkeltmandsvirksomhed", "Forening", "Partsrederi", "Selskab", "virksomhed" }.Select(v => v.ToLowerInvariant()).ToHashSet();
            var contains  = new[] { " dk", "dk ", "denmark", "danmark", "dansk", "æ", "ø", "å" }.Select(v => v.ToLowerInvariant()).ToHashSet();

            Func<string, bool> cvrFilter            = value => existingResults.Any(r => string.Equals(r.Data.CvrNumber.ToString(CultureInfo.InvariantCulture), value, StringComparison.InvariantCultureIgnoreCase));
            Func<string, bool> nameFilter           = value => OrganizationFilters.NameFilter(context, value);
            Func<string, bool> namePostFixFilter    = value =>
                {
                    value = value.ToLowerInvariant();

                    if (contains.Any(c => value.Contains(c)))
                        return false;

                    var name = OrganizationName.Parse(value);
                    {
                        if (name == null || string.IsNullOrEmpty(name.Postfix))
                            return true;

                        if (postFixes.Contains(name.Postfix.ToLowerInvariant()))
                            return false;
                    }

                    if (postFixes.Any(v => value.EndsWith(v)))
                        return false;

                    return true;
                };

            // Query Input
            var entityType          = request.EntityMetaData.EntityType;

            var cvrNumber = GetValue(request, config, Constants.KeyName.CVRKey, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesCVR);
            var organizationName = GetValue(request, config, Constants.KeyName.OrgNameKey, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName);
            var country = GetValue(request, config, Constants.KeyName.CountryKey, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCountryCode);
            var website = GetValue(request, config, Constants.KeyName.WebsiteKey, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website);

            if (country != null)
            {
                if (country.Any(v => string.Equals(v, "dk", StringComparison.InvariantCultureIgnoreCase) || 
                                     string.Equals(v, "denmark", StringComparison.InvariantCultureIgnoreCase) ||
                                     string.Equals(v, "danmark", StringComparison.InvariantCultureIgnoreCase)))
                    namePostFixFilter = value => false;
            }
            if (website != null)
            {
                var hosts = website.Where(u => UriUtility.IsValid(u)).Select(u => new Uri(u).Host.ToLowerInvariant()).Distinct();

                if (hosts.Any(h =>
                    {
                        if (!DomainName.TryParse(h, out var domain))
                            return false;

                        return string.Equals(domain.TLD, "dk", StringComparison.InvariantCultureIgnoreCase);
                    }))
                    namePostFixFilter = value => false;
            }

            if (cvrNumber.Any())
            {
                var values = cvrNumber;

                foreach (var value in values.Where(v => !cvrFilter(v)))
                {
                    if (int.TryParse(value, out var result))
                        yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Identifier, result.ToString());
                }
            }
            else if (organizationName != null)
            {
                var values = jobData.OrgNameNormalization ? Enumerable.ToHashSet(organizationName.Select(NameNormalization.Normalize)).Where(v => !nameFilter(v) && !namePostFixFilter(v)) : organizationName;

                foreach (var value in values)
                {
                    yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Name, value);
                }
            }
        }

        private static HashSet<string> GetValue(IExternalSearchRequest request, IDictionary<string, object> config, string keyName, VocabularyKey defaultKey)
        {
            HashSet<string> value;
            if (config.TryGetValue(keyName, out var customVocabKey) && !string.IsNullOrWhiteSpace(customVocabKey?.ToString()))
            {
                value = request.QueryParameters.GetValue<string, HashSet<string>>(customVocabKey.ToString(), new HashSet<string>());
            }
            else
            {
                value = request.QueryParameters.GetValue(defaultKey, new HashSet<string>());
            }

            return value;
        }

        /// <summary>Creates the metadata.</summary>
        /// <param name="resultItem">The result item.</param>
        /// <returns>The metadata.</returns>
        private IEntityMetadata CreateMetadata(IExternalSearchQueryResult<CvrResult> resultItem, IExternalSearchRequest request)
        {
            var metadata = new EntityMetadataPart();

            this.PopulateMetadata(metadata, resultItem, request);

            return metadata;
        }

        /// <summary>Populates the metadata.</summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="resultItem">The result item.</param>
        private void PopulateMetadata(IEntityMetadata metadata, IExternalSearchQueryResult<CvrResult> resultItem, IExternalSearchRequest request)
        {
            metadata.EntityType             = request.EntityMetaData.EntityType;
            metadata.Name                   = request.EntityMetaData.Name;
            metadata.OriginEntityCode       = request.EntityMetaData.OriginEntityCode;

            metadata.Aliases.AddRange(resultItem.Data.Organization.AlternateNames);

            metadata.Properties[CvrVocabulary.Organization.CompanyTypeCode]             = resultItem.Data.Organization.CompanyTypeCode.PrintIfAvailable();
            metadata.Properties[CvrVocabulary.Organization.CompanyTypeLongName]         = resultItem.Data.Organization.CompanyTypeLongName;
            metadata.Properties[CvrVocabulary.Organization.CompanyTypeShortName]        = resultItem.Data.Organization.CompanyTypeShortName;
            metadata.Properties[CvrVocabulary.Organization.CreditStatusCode]            = resultItem.Data.Organization.CreditStatusCode.PrintIfAvailable();
            metadata.Properties[CvrVocabulary.Organization.CreditStatusText]            = resultItem.Data.Organization.CreditStatusText;
            metadata.Properties[CvrVocabulary.Organization.CvrNumber]                   = resultItem.Data.Organization.CvrNumber.PrintIfAvailable();
            metadata.Properties[CvrVocabulary.Organization.Email]                       = resultItem.Data.Organization.Email;
            metadata.Properties[CvrVocabulary.Organization.EndDate]                     = resultItem.Data.Organization.EndDate.PrintIfAvailable();
            metadata.Properties[CvrVocabulary.Organization.FaxNumber]                   = resultItem.Data.Organization.FaxNumber;
            metadata.Properties[CvrVocabulary.Organization.FirstFiscalYearEnd]          = resultItem.Data.Organization.FirstFiscalYearEnd.PrintIfAvailable();
            metadata.Properties[CvrVocabulary.Organization.FirstFiscalYearStart]        = resultItem.Data.Organization.FirstFiscalYearStart.PrintIfAvailable();
            metadata.Properties[CvrVocabulary.Organization.FiscalYearEnd]               = resultItem.Data.Organization.FiscalYearEnd.PrintIfAvailable(v => v.Replace("--", string.Empty));
            metadata.Properties[CvrVocabulary.Organization.FiscalYearStart]             = resultItem.Data.Organization.FiscalYearStart.PrintIfAvailable(v => v.Replace("--", string.Empty));
            metadata.Properties[CvrVocabulary.Organization.FoundingDate]                = resultItem.Data.Organization.FoundingDate.PrintIfAvailable();
            metadata.Properties[CvrVocabulary.Organization.IsBankrupt]                  = resultItem.Data.Organization.IsBankrupt.PrintIfAvailable();
            metadata.Properties[CvrVocabulary.Organization.MainIndustryCode]            = resultItem.Data.Organization.MainIndustry.PrintIfAvailable(v => v.Code);
            metadata.Properties[CvrVocabulary.Organization.MainIndustryDescription]     = resultItem.Data.Organization.MainIndustry.PrintIfAvailable(v => v.Description);
            metadata.Properties[CvrVocabulary.Organization.Municipality]                = resultItem.Data.Organization.Municipality;
            metadata.Properties[CvrVocabulary.Organization.Name]                        = resultItem.Data.Organization.Name;
            metadata.Properties[CvrVocabulary.Organization.NumberOfEmployees]           = resultItem.Data.Organization.NumberOfEmployees.PrintIfAvailable(v => v.Minimum == v.Maximum ? v.Maximum.PrintIfAvailable() : string.Format("{0}-{1}", v.Minimum, v.Maximum));
            metadata.Properties[CvrVocabulary.Organization.OptOutSalesAndAdvertising]   = resultItem.Data.Organization.OptOutSalesAndAdvertising.PrintIfAvailable();
            metadata.Properties[CvrVocabulary.Organization.PhoneNumber]                 = resultItem.Data.Organization.PhoneNumber;
            metadata.Properties[CvrVocabulary.Organization.ProductionUnitCount]         = resultItem.Data.Organization.ProductionUnitCount.PrintIfAvailable();
            metadata.Properties[CvrVocabulary.Organization.Purpose]                     = resultItem.Data.Organization.Purpose;
            metadata.Properties[CvrVocabulary.Organization.RegisteredCapital]           = resultItem.Data.Organization.RegisteredCapital;
            metadata.Properties[CvrVocabulary.Organization.RegisteredCapitalCurrency]   = resultItem.Data.Organization.RegisteredCapitalCurrency;
            metadata.Properties[CvrVocabulary.Organization.HasShareCapitalClasses]      = resultItem.Data.Organization.HasShareCapitalClasses;
            metadata.Properties[CvrVocabulary.Organization.StartDate]                   = resultItem.Data.Organization.StartDate.PrintIfAvailable();
            metadata.Properties[CvrVocabulary.Organization.Status]                      = resultItem.Data.Organization.Status;
            metadata.Properties[CvrVocabulary.Organization.StatutesLastChanged]         = resultItem.Data.Organization.StatutesLastChanged;
            metadata.Properties[CvrVocabulary.Organization.Website]                     = resultItem.Data.Organization.Website;
            metadata.Properties[CvrVocabulary.Organization.OtherIndustry1Code]          = resultItem.Data.Organization.OtherIndustry1.PrintIfAvailable(v => v.Code);
            metadata.Properties[CvrVocabulary.Organization.OtherIndustry1Description]   = resultItem.Data.Organization.OtherIndustry1.PrintIfAvailable(v => v.Description);
            metadata.Properties[CvrVocabulary.Organization.OtherIndustry2Code]          = resultItem.Data.Organization.OtherIndustry2.PrintIfAvailable(v => v.Code);
            metadata.Properties[CvrVocabulary.Organization.OtherIndustry2Description]   = resultItem.Data.Organization.OtherIndustry2.PrintIfAvailable(v => v.Description);
            metadata.Properties[CvrVocabulary.Organization.OtherIndustry3Code]          = resultItem.Data.Organization.OtherIndustry3.PrintIfAvailable(v => v.Code);
            metadata.Properties[CvrVocabulary.Organization.OtherIndustry3Description]   = resultItem.Data.Organization.OtherIndustry3.PrintIfAvailable(v => v.Description);

            if (resultItem.Data.Organization.Address != null)
                PopulateAddress(metadata, CvrVocabulary.Address, resultItem.Data.Organization.Address);

            if (resultItem.Data.Organization.PostalAddress != null)
                PopulateAddress(metadata, CvrVocabulary.Organization.PostalAddress, resultItem.Data.Organization.PostalAddress);

            if (resultItem.Data.FinancialReportSummary != null)
            {
                if (metadata.ModifiedDate == null || resultItem.Data.FinancialReportSummary.DateOfApprovalOfReport > metadata.ModifiedDate)
                    metadata.ModifiedDate = resultItem.Data.FinancialReportSummary.DateOfApprovalOfReport;

                var currency = resultItem.Data.FinancialReportSummary.Currency;
                metadata.Properties[CvrVocabulary.Organization.FinancialReportSummaryYear]                                      = resultItem.Data.FinancialReportSummary.Year.PrintIfAvailable();
                metadata.Properties[CvrVocabulary.Organization.FinancialReportSummaryEquity]                                    = resultItem.Data.FinancialReportSummary.Equity.PrintIfAvailable(a => currency != null ? a.Value.PrintIfAvailable() : a.ToString());
                metadata.Properties[CvrVocabulary.Organization.FinancialReportSummaryGrossProfitLoss]                           = resultItem.Data.FinancialReportSummary.GrossProfitLoss.PrintIfAvailable(a => currency != null ? a.Value.PrintIfAvailable() : a.ToString());
                metadata.Properties[CvrVocabulary.Organization.FinancialReportSummaryLiabilitiesAndEquity]                      = resultItem.Data.FinancialReportSummary.LiabilitiesAndEquity.PrintIfAvailable(a => currency != null ? a.Value.PrintIfAvailable() : a.ToString());
                metadata.Properties[CvrVocabulary.Organization.FinancialReportSummaryProfitLoss]                                = resultItem.Data.FinancialReportSummary.ProfitLoss.PrintIfAvailable(a => currency != null ? a.Value.PrintIfAvailable() : a.ToString());
                metadata.Properties[CvrVocabulary.Organization.FinancialReportSummaryProfitLossFromOrdinaryActivitiesBeforeTax] = resultItem.Data.FinancialReportSummary.ProfitLossFromOrdinaryActivitiesBeforeTax.PrintIfAvailable(a => currency != null ? a.Value.PrintIfAvailable() : a.ToString());
                metadata.Properties[CvrVocabulary.Organization.FinancialReportSummaryCurrency]                                  = currency;

                var financialReportPdf = resultItem.Data.FinancialReport.Dokumenter.FirstOrDefault(d => d.DokumentMimeType == "application/pdf");

                if (financialReportPdf != null)
                    metadata.Properties[CvrVocabulary.Organization.FinancialReportSummaryPdfLink]                   = financialReportPdf.DokumentUrl;

                metadata.Properties[CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AnnualRevenue]  = resultItem.Data.FinancialReportSummary.GrossProfitLoss.PrintIfAvailable(a => a.Value.PrintIfAvailable() + " " + a.UnitClean);
            }
        }

        private static void PopulateAddress(IEntityMetadata metadata, CvrAddressVocabulary vocabulary, Adresse address)
        {
            metadata.Properties[vocabulary.AddressId]           = address.AdresseId;
            metadata.Properties[vocabulary.StreetLetterFrom]    = address.BogstavFra;
            metadata.Properties[vocabulary.StreetLetterTo]      = address.BogstavTil;
            metadata.Properties[vocabulary.City]                = address.Bynavn;
            metadata.Properties[vocabulary.CoName]              = address.Conavn;
            metadata.Properties[vocabulary.Floor]               = address.Etage;
            metadata.Properties[vocabulary.FreeText]            = address.Fritekst;
            metadata.Properties[vocabulary.CountryCode]         = address.Landekode;
            metadata.Properties[vocabulary.PoBox]               = address.Postboks;
            metadata.Properties[vocabulary.District]            = address.Postdistrikt;
            metadata.Properties[vocabulary.Door]                = address.Sidedoer;
            metadata.Properties[vocabulary.StreetName]          = address.Vejnavn;
            metadata.Properties[vocabulary.StreetNumberFrom]    = address.HusnummerFra.PrintIfAvailable();
            metadata.Properties[vocabulary.StreetNumberTo]      = address.HusnummerTil.PrintIfAvailable();
            metadata.Properties[vocabulary.MunicipalityName]    = address.Kommune.PrintIfAvailable(v => v.kommuneNavn);
            metadata.Properties[vocabulary.MunicipalityCode]    = address.Kommune.PrintIfAvailable(v => v.kommuneKode);
            metadata.Properties[vocabulary.PostalCode]          = address.Postnummer.PrintIfAvailable();
            metadata.Properties[vocabulary.StreetCode]          = address.Vejkode.PrintIfAvailable();

            metadata.Properties[vocabulary.Formatted]           = address.ToString();

            if (!string.IsNullOrWhiteSpace(address.Vejnavn))
            {
                var addressLine = address.Vejnavn;
                if (!string.IsNullOrWhiteSpace(address.HusnummerFra.PrintIfAvailable()))
                {
                    addressLine += " " + address.HusnummerFra;
                    if (!string.IsNullOrWhiteSpace(address.HusnummerTil.PrintIfAvailable()))
                    {
                        if (address.HusnummerFra != address.HusnummerFra)
                            addressLine += $"-{address.HusnummerTil.PrintIfAvailable()}";
                    }
                }
                if (!string.IsNullOrWhiteSpace(address.BogstavFra))
                {
                    addressLine += " " + address.BogstavFra;
                    if (!string.IsNullOrWhiteSpace(address.BogstavTil))
                    {
                        if (address.BogstavFra != address.BogstavTil)
                            addressLine += $"-{address.BogstavTil}";
                    }
                }
                metadata.Properties[vocabulary.AddressLine1] = addressLine;
            }
        }

        public IEnumerable<EntityType> Accepts(IDictionary<string, object> config, IProvider provider) => this.Accepts(config);

        private IEnumerable<EntityType> Accepts(IDictionary<string, object> config)
        {
            if (config.TryGetValue(Constants.KeyName.AcceptedEntityType, out var acceptedEntityTypeObj) && acceptedEntityTypeObj is string acceptedEntityType && !string.IsNullOrWhiteSpace(acceptedEntityType))
            {
                return new EntityType[] { acceptedEntityType };
            }

            // Fallback to default accepted entity types
            return DefaultAcceptedEntityTypes;
        }

        private bool Accepts(IDictionary<string, object> config, EntityType entityTypeToEvaluate)
        {
            var configurableAcceptedEntityTypes = this.Accepts(config).ToArray();

            return configurableAcceptedEntityTypes.Any(entityTypeToEvaluate.Is);
        }

        public IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            return InternalBuildQueries(context, request, config);
        }

        public IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query, IDictionary<string, object> config, IProvider provider)
        {
            var cvrExternalSearchJobData = new CvrExternalSearchJobData(config);

            var identifier = query.QueryParameters.GetValue<string, HashSet<string>>(ExternalSearchQueryParameter.Identifier.ToString(), new HashSet<string>()).FirstOrDefault();
            var name = query.QueryParameters.GetValue<string, HashSet<string>>(ExternalSearchQueryParameter.Name.ToString(), new HashSet<string>()).FirstOrDefault();

            var client = new CvrClient();

            if (!string.IsNullOrEmpty(identifier))
            {
                var response = client.GetCvrResult(int.Parse(identifier));

                if (response == null)
                    yield break;
                else if (response.Organization != null)
                    yield return new ExternalSearchQueryResult<CvrResult>(query, response);
                else if (response.ErrorException != null)
                    throw new AggregateException(response.ErrorException.Message, response.ErrorException);
                else
                    throw new ApplicationException("Could not execute external search query");
            }
            else if (!string.IsNullOrEmpty(name))
            {
                var errors = new List<Exception>();

                var response = client.GetCvrResultsByName(name, cvrExternalSearchJobData.OrgMatchPastNames);

                if (response == null)
                    yield break;

                if (response.Count() != 1)
                    yield break;

                foreach (var result in response)
                {
                    if (result.Organization != null)
                    {
                        yield return new ExternalSearchQueryResult<CvrResult>(query, result);
                        yield break;
                    }
                    else if (result.ErrorException != null)
                        errors.Add(result.ErrorException);
                }

                if (errors.Any())
                    throw new AggregateException("Could not execute external search query", errors);
            }
            else
                yield break;
        }

        public IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            var resultItem = result.As<CvrResult>();


            var clue = new Clue(request.EntityMetaData.OriginEntityCode, context.Organization);
            clue.Data.OriginProviderDefinitionId = this.Id;

            this.PopulateMetadata(clue.Data.EntityData, resultItem, request);

            return new[] { clue };
        }

        public IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            var resultItem = result.As<CvrResult>();
            return this.CreateMetadata(resultItem, request);
        }

        public IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            return null;
        }


        public string Icon { get; } = Constants.Icon;
        public string Domain { get; } = Constants.Domain;
        public string About { get; } = Constants.About;

        public AuthMethods AuthMethods { get; } = Constants.AuthMethods;
        public IEnumerable<Control> Properties { get; } = Constants.Properties;
        public Guide Guide { get; } = Constants.Guide;
        public IntegrationType Type { get; } = Constants.IntegrationType;
    }
}
