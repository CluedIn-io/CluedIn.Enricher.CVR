using System;

using CluedIn.ExternalSearch.Providers.CVR.Model.Cvr;

namespace CluedIn.ExternalSearch.Providers.CVR.Model
{
    public class IndustryDescription
    {
        public IndustryDescription()
        {
        }

        public IndustryDescription(Branche branch)
        {
            if (branch == null)
                throw new ArgumentNullException(nameof(branch));

            this.Code        = branch.Branchekode;
            this.Description = branch.Branchetekst;
        }

        public int Code { get; set; } // 582900

        public string Description { get; set; } // Anden udgivelse af software
    }
}