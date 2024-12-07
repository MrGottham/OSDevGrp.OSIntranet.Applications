using Microsoft.AspNetCore.Http;
using OSDevGrp.OSIntranet.Core;
using System;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Extensions
{
    internal static class HttpRequestExtensions
    {
        #region Methods

        internal static Uri ToAbsoluteUri(this HttpRequest request, string path)
        {
            NullGuard.NotNull(request, nameof(request))
                .NotNullOrWhiteSpace(path, nameof(path));

            string scheme = request.Scheme;
            if (request.IsHttps && scheme.ToLower().EndsWith("s") == false)
            {
                scheme += "s";
            }

            return new Uri($"{scheme}://{request.Host}{request.PathBase}{path}", UriKind.Absolute);
        }

        #endregion
    }
}