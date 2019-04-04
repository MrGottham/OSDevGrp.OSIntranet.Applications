using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Security
{
    public interface ISecurityContextReader
    {
        AuthenticationHeaderValue GetBasicAuthenticationHeader(HttpRequest request);
    }
}
