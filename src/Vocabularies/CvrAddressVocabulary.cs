// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CvrAddressVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the CvrAddressVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.CVR.Vocabularies
{
    /// <summary>The CVR address vocabulary</summary>
    /// <seealso cref="CluedIn.Core.Data.Vocabularies.SimpleVocabulary" />
    public class CvrAddressVocabulary : SimpleVocabulary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CvrAddressVocabulary"/> class.
        /// </summary>
        public CvrAddressVocabulary()
        {
            this.VocabularyName = "CVR Address";
            this.KeyPrefix      = "cvr.address";
            this.KeySeparator   = ".";
            this.Grouping       = EntityType.Geography;

            this.AddressId          = this.Add(new VocabularyKey("addressId",                                                           VocabularyKeyVisibility.Hidden));
            this.StreetLetterFrom   = this.Add(new VocabularyKey("streetLetterFrom"));
            this.StreetLetterTo     = this.Add(new VocabularyKey("streetLetterTo"));
            this.City               = this.Add(new VocabularyKey("city",                    VocabularyKeyDataType.GeographyCity));
            this.CoName             = this.Add(new VocabularyKey("coName"));
            this.Floor              = this.Add(new VocabularyKey("floor"));
            this.FreeText           = this.Add(new VocabularyKey("freeText"));
            this.CountryCode        = this.Add(new VocabularyKey("countryCode",             VocabularyKeyDataType.GeographyCountry));
            this.PoBox              = this.Add(new VocabularyKey("poBox"));
            this.District           = this.Add(new VocabularyKey("district"));
            this.Door               = this.Add(new VocabularyKey("door"));
            this.StreetName         = this.Add(new VocabularyKey("streetName"));
            this.StreetNumberFrom   = this.Add(new VocabularyKey("streetNumberFrom"));
            this.StreetNumberTo     = this.Add(new VocabularyKey("streetNumberTo"));
            this.MunicipalityName   = this.Add(new VocabularyKey("municipalityName"));
            this.MunicipalityCode   = this.Add(new VocabularyKey("municipalityCode",                                                    VocabularyKeyVisibility.Hidden));
            this.PostalCode         = this.Add(new VocabularyKey("postalCode"));
            this.StreetCode         = this.Add(new VocabularyKey("streetCode"));
            this.Formatted          = this.Add(new VocabularyKey("formatted"));
        }

        public VocabularyKey AddressId { get; protected set; }
        public VocabularyKey StreetLetterFrom { get; protected set; }
        public VocabularyKey StreetLetterTo { get; protected set; }
        public VocabularyKey City { get; protected set; }
        public VocabularyKey CoName { get; protected set; }
        public VocabularyKey Floor { get; protected set; }
        public VocabularyKey FreeText { get; protected set; }
        public VocabularyKey CountryCode { get; protected set; }
        public VocabularyKey PoBox { get; protected set; }
        public VocabularyKey District { get; protected set; }
        public VocabularyKey Door { get; protected set; }
        public VocabularyKey StreetName { get; protected set; }
        public VocabularyKey StreetNumberFrom { get; protected set; }
        public VocabularyKey StreetNumberTo { get; protected set; }
        public VocabularyKey MunicipalityName { get; protected set; }
        public VocabularyKey MunicipalityCode { get; protected set; }
        public VocabularyKey PostalCode { get; protected set; }
        public VocabularyKey StreetCode { get; protected set; }
        public VocabularyKey Formatted { get; protected set; }

    }
}
