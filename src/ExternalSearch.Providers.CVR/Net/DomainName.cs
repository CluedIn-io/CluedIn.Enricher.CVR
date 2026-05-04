// © CluedIn ApS. All rights reserved. CluedIn® is a registered trademark of CluedIn ApS.

using System.Diagnostics.CodeAnalysis;
using Nager.PublicSuffix;
using Nager.PublicSuffix.Exceptions;
using Nager.PublicSuffix.RuleProviders;

namespace CluedIn.ExternalSearch.Providers.CVR.Net;

internal static class DomainName
{
    private static readonly DomainParser domainParser = new(new SimpleHttpRuleProvider());

    public static bool TryParse(string domain, [NotNullWhen(true)] out DomainInfo? domainInfo)
    {
        try
        {
            domainInfo = domainParser.Parse(domain);
            return domainInfo != null;
        }
        catch (ParseException)
        {
            domainInfo = null;
            return false;
        }
    }
}
