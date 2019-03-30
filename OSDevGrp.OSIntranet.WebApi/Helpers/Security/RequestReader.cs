using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.HttpSys;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Security
{
    public class RequestReader : IRequestReader
    {
        #region Properties

        public AuthenticationHeaderValue GetBasicAuthenticationHeader(HttpRequest request)
        {
            NullGuard.NotNull(request, nameof(request));

            if (request.Headers == null || string.IsNullOrWhiteSpace(request.Headers["Authorization"]))
            {
                return null;
            }

            if (AuthenticationHeaderValue.TryParse(request.Headers["Authorization"], out var authenticationHeader) == false)
            {
                return null;
            }

            return string.Compare(authenticationHeader.Scheme, AuthenticationSchemes.Basic.ToString(), StringComparison.Ordinal) == 0
                ? authenticationHeader
                : null;
        }

        #endregion
    }
}
