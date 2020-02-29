using Microsoft.AspNetCore.Http;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Mvc.Helpers
{
    internal static class HttpRequestExtensions
    {
        internal static string ToAbsoluteUri(this HttpRequest request, string path)
        {
            NullGuard.NotNull(request, nameof(request))
                .NotNullOrWhiteSpace(path, nameof(path));

            return $"{request.Scheme}://{request.Host}{request.PathBase}{path}";
        }
    }
}