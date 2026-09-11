// © CluedIn ApS. All rights reserved. CluedIn® is a registered trademark of CluedIn ApS.

using System;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;

namespace CluedIn.ExternalSearch.Providers.CVR.Client;

// RestSharp's API changed significantly between the version CluedIn 4.7/4.8 (net6.0) resolves
// (106.15.0: uppercase Method enum, IRestResponse<T>, no RestClientOptions/Credentials) and the
// version CluedIn 5.0+ (net10.0) resolves (114.0.0: PascalCase Method enum, RestResponse<T>,
// RestClientOptions.Credentials). Centralized here so call sites don't repeat the #if.
internal static class RestSharpCompat
{
#if CLUEDIN_V50
    public const Method HttpGet = Method.Get;
    public const Method HttpPost = Method.Post;
#else
    public const Method HttpGet = Method.GET;
    public const Method HttpPost = Method.POST;
#endif

    public static RestClient CreateClient(Uri endpoint, NetworkCredential credentials)
    {
#if CLUEDIN_V50
        return new RestClient(new RestClientOptions(endpoint) { Credentials = credentials });
#else
        var client = new RestClient(endpoint);
        if (credentials != null)
            client.Authenticator = new HttpBasicAuthenticator(credentials.UserName, credentials.Password);
        return client;
#endif
    }
}
