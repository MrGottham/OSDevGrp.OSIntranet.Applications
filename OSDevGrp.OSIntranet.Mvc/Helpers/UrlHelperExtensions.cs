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

            return MakeAbsoluteUri(request, urlHelper.Action(action, urlHelper.Action(action)));
        }

        internal static string AbsoluteAction(this IUrlHelper urlHelper, string action, string controller)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper))
                .NotNullOrWhiteSpace(action, nameof(action))
                .NotNullOrWhiteSpace(controller, nameof(controller));

            HttpRequest request = urlHelper.ActionContext.HttpContext.Request;

            return MakeAbsoluteUri(request, urlHelper.Action(action, controller));
        }

        internal static string AbsoluteAction(this IUrlHelper urlHelper, string action, object values)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper))
                .NotNullOrWhiteSpace(action, nameof(action))
                .NotNull(values, nameof(values));

            HttpRequest request = urlHelper.ActionContext.HttpContext.Request;

            return MakeAbsoluteUri(request, urlHelper.Action(action, values));
        }

        internal static string AbsoluteAction(this IUrlHelper urlHelper, string action, string controller, object values)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper))
                .NotNullOrWhiteSpace(action, nameof(action))
                .NotNullOrWhiteSpace(controller, nameof(controller))
                .NotNull(values, nameof(values));

            HttpRequest request = urlHelper.ActionContext.HttpContext.Request;

            return MakeAbsoluteUri(request, urlHelper.Action(action, controller, values));
        }

        internal static string AbsoluteContent(this IUrlHelper urlHelper, string contentPath)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper))
                .NotNullOrWhiteSpace(contentPath, nameof(contentPath));

            HttpRequest request = urlHelper.ActionContext.HttpContext.Request;

            string contentResult;
            if (contentPath.StartsWith("~/"))
            {
                contentResult = urlHelper.Content(contentPath);
            }
            else if (contentPath.StartsWith("/"))
            {
                contentResult = urlHelper.Content($"~{contentPath}");
            }
            else
            {
                contentResult = urlHelper.Content($"~/{contentPath}");
            }

            return MakeAbsoluteUri(request, contentResult);
        }

        private static string MakeAbsoluteUri(HttpRequest request, string path)
        {
            NullGuard.NotNull(request, nameof(request))
                .NotNullOrWhiteSpace(path, nameof(path));

            return $"{request.Scheme}://{request.Host}{request.PathBase}{path}";
        }
    }
}