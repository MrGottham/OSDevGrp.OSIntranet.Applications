using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Security
{
    public interface IRequestReader
    {
        AuthenticationHeaderValue GetBasicAuthenticationHeader(HttpRequest request);
    }
}
