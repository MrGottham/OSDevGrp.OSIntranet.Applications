using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using System;
using Microsoft.AspNetCore.Http;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Extensions
{
    internal static class UrlHelperExtensions
    {
        #region Methods

        internal static Uri AbsoluteAction(this IUrlHelper urlHelper, string action, string controller)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper))
                .NotNullOrWhiteSpace(action, nameof(action))
                .NotNullOrWhiteSpace(controller, nameof(controller));

            HttpRequest httpRequest = urlHelper.ActionContext.HttpContext.Request;

            return httpRequest.ToAbsoluteUri(urlHelper.Action(action, controller));
        }

        #endregion
    }
}