using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Mvc.Helpers
{
    internal static class UrlHelperExtensions
    {
        internal static string AbsoluteAction(this IUrlHelper urlHelper, string action)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper))
                .NotNullOrWhiteSpace(action, nameof(action));

            HttpRequest request = urlHelper.ActionContext.HttpContext.Request;

            return $"{request.Scheme}://{request.Host}{request.PathBase}{urlHelper.Action(action)}";
        }

        internal static string AbsoluteAction(this IUrlHelper urlHelper, string action, object values)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper))
                .NotNullOrWhiteSpace(action, nameof(action));

            HttpRequest request = urlHelper.ActionContext.HttpContext.Request;

            return $"{request.Scheme}://{request.Host}{request.PathBase}{urlHelper.Action(action, values)}";
        }

        internal static string AbsoluteContent(this IUrlHelper urlHelper, string contentPath)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper))
                .NotNullOrWhiteSpace(contentPath, nameof(contentPath));

            HttpRequest request = urlHelper.ActionContext.HttpContext.Request;

            string contentResult = null;
            if (contentPath.StartsWith("~/"))
            {
                contentResult = urlHelper.Content(contentPath);
            } else if (contentPath.StartsWith("/"))
            {
                contentResult = urlHelper.Content($"~{contentPath}");
            }
            else
            {
                contentResult = urlHelper.Content($"~/{contentPath}");
            }

            return $"{request.Scheme}://{request.Host}{request.PathBase}{contentResult}";
        }
    }
}