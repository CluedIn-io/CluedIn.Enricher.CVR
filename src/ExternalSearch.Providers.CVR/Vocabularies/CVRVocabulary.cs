// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CVRVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the CvrVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CluedIn.ExternalSearch.Providers.CVR.Vocabularies
{
    /// <summary>The CVR vocabulary</summary>
    public static class CvrVocabulary
    {
        /// <summary>
        /// Initializes static members of the <see cref="CvrVocabulary" /> class.
        /// </summary>
        static CvrVocabulary()
        {
            Organization    = new CvrOrganizationVocabulary();
            Address         = new CvrAddressVocabulary();
        }

        /// <summary>Gets the organization.</summary>
        /// <value>The organization.</value>
        public static CvrOrganizationVocabulary Organization { get; private set; }
        public static CvrAddressVocabulary Address { get; private set; }
    }
}