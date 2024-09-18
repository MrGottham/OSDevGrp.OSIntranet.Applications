using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers
{
    internal static class UrlHelperMockExtensions
    {
        internal static void Setup(this Mock<IUrlHelper> urlHelperMock, Fixture fixture, string scheme = null, string host = null, string pathBase = null, string absolutePath = null)
        {
            NullGuard.NotNull(urlHelperMock, nameof(urlHelperMock))
                .NotNull(fixture, nameof(fixture));

            HttpContext httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = scheme ?? "https";
            httpContext.Request.Host = new HostString(host ?? fixture.CreateDomainName());
            httpContext.Request.PathBase = $"/{pathBase ?? fixture.Create<string>()}";

            ActionContext actionContext = new ActionContext
            {
                HttpContext = httpContext
            };

            Uri requestUri = new Uri($"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.PathBase}");

            urlHelperMock.Setup(m => m.ActionContext)
                .Returns(actionContext);

            urlHelperMock.Setup(m => m.Action(It.IsAny<UrlActionContext>()))
                .Returns(absolutePath ?? requestUri.AbsolutePath);

            urlHelperMock.Setup(m => m.Content(It.IsAny<string>()))
                .Returns(absolutePath ?? requestUri.AbsolutePath);
        }
    }
}