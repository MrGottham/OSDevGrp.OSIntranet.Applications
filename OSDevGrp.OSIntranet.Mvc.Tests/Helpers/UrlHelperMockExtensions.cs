using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers
{
    internal static class UrlHelperMockExtensions
    {
        internal static void Setup(this Mock<IUrlHelper> urlHelperMock, Fixture fixture, string scheme = null, string host = null, string pathBase = null, string absolutePath = null)
        {
            NullGuard.NotNull(urlHelperMock, nameof(urlHelperMock))
                .NotNull(fixture, nameof(fixture));

            HttpContext httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = scheme ?? "http";
            httpContext.Request.Host = new HostString(host ?? "localhost");
            httpContext.Request.PathBase = pathBase ?? $"/{fixture.Create<string>()}";

            ActionContext actionContext = new ActionContext
            {
                HttpContext = httpContext
            };

            urlHelperMock.Setup(m => m.ActionContext)
                .Returns(actionContext);

            urlHelperMock.Setup(m => m.Action(It.IsAny<UrlActionContext>()))
                .Returns(absolutePath ?? $"/{fixture.Create<string>()}");

            urlHelperMock.Setup(m => m.Content(It.IsAny<string>()))
                .Returns(absolutePath ?? $"/{fixture.Create<string>()}");
        }
    }
}