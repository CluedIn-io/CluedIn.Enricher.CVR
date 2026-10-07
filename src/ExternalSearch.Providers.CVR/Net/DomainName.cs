// © CluedIn ApS. All rights reserved. CluedIn® is a registered trademark of CluedIn ApS.

using System.Diagnostics.CodeAnalysis;
using Nager.PublicSuffix;
#if CLUEDIN_V50
using Nager.PublicSuffix.Exceptions;
using Nager.PublicSuffix.RuleProviders;
#endif

namespace CluedIn.ExternalSearch.Providers.CVR.Net;

internal static class DomainName
{
    // Nager.PublicSuffix 2.4.0 (CluedIn < 5.0, net6.0) vs 3.8.0 (CluedIn 5.0+, net10.0): namespaces
    // moved (SimpleHttpRuleProvider now lives under RuleProviders; ParseException under
    // Exceptions), and the HTTP-backed rule provider was renamed WebTldRuleProvider -> SimpleHttpRuleProvider.
#if CLUEDIN_V50
    private static readonly DomainParser domainParser = new(new SimpleHttpRuleProvider());
#else
    private static readonly DomainParser domainParser = new(new WebTldRuleProvider());
#endif

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

    // DomainInfo.TLD (2.4.0) was renamed DomainInfo.TopLevelDomain (3.8.0).
    public static string GetTopLevelDomain(DomainInfo domainInfo) =>
#if CLUEDIN_V50
        domainInfo.TopLevelDomain;
#else
        domainInfo.TLD;
#endif
}
