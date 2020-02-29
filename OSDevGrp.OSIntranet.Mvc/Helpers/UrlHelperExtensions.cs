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

            return request.ToAbsoluteUri(urlHelper.Action(action, urlHelper.Action(action)));
        }

        internal static string AbsoluteAction(this IUrlHelper urlHelper, string action, string controller)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper))
                .NotNullOrWhiteSpace(action, nameof(action))
                .NotNullOrWhiteSpace(controller, nameof(controller));

            HttpRequest request = urlHelper.ActionContext.HttpContext.Request;

            return request.ToAbsoluteUri(urlHelper.Action(action, controller));
        }

        internal static string AbsoluteAction(this IUrlHelper urlHelper, string action, object values)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper))
                .NotNullOrWhiteSpace(action, nameof(action))
                .NotNull(values, nameof(values));

            HttpRequest request = urlHelper.ActionContext.HttpContext.Request;

            return request.ToAbsoluteUri(urlHelper.Action(action, values));
        }

        internal static string AbsoluteAction(this IUrlHelper urlHelper, string action, string controller, object values)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper))
                .NotNullOrWhiteSpace(action, nameof(action))
                .NotNullOrWhiteSpace(controller, nameof(controller))
                .NotNull(values, nameof(values));

            HttpRequest request = urlHelper.ActionContext.HttpContext.Request;

            return request.ToAbsoluteUri(urlHelper.Action(action, controller, values));
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

            return request.ToAbsoluteUri(contentResult);
        }
    }
}