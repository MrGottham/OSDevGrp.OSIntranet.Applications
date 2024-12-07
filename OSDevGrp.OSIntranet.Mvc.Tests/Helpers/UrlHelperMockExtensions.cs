using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using System.Text;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers
{
    internal static class UrlHelperMockExtensions
    {
        internal static void Setup(this Mock<IUrlHelper> urlHelperMock, Fixture fixture, string scheme = null, string host = null, string pathBase = null)
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

            urlHelperMock.Setup(m => m.ActionContext)
                .Returns(actionContext);

            urlHelperMock.Setup(m => m.Action(It.IsAny<UrlActionContext>()))
                .Returns<UrlActionContext>(GeneratePath);

            urlHelperMock.Setup(m => m.Content(It.IsAny<string>()))
                .Returns<string>(ConvertContentPath);
        }

        private static string GeneratePath(UrlActionContext urlActionContext)
        {
            NullGuard.NotNull(urlActionContext, nameof(urlActionContext));

            StringBuilder pathBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(urlActionContext.Fragment) == false)
            {
                pathBuilder.Append($"/{urlActionContext.Fragment}");
            }
            if (string.IsNullOrWhiteSpace(urlActionContext.Controller) == false)
            {
                pathBuilder.Append($"/{urlActionContext.Controller}");
            }
            if (string.IsNullOrWhiteSpace(urlActionContext.Action) == false)
            {
                pathBuilder.Append($"/{urlActionContext.Action}");
            }

            if (string.IsNullOrWhiteSpace(urlActionContext.Protocol) && string.IsNullOrWhiteSpace(urlActionContext.Host))
            {
                return pathBuilder.ToString();
            }

            return $"{urlActionContext.Protocol}://{urlActionContext.Host}{pathBuilder}";
        }

        private static string ConvertContentPath(string contentPath)
        {
            NullGuard.NotNullOrWhiteSpace(contentPath, nameof(contentPath));

            if (contentPath.StartsWith("~/"))
            {
                return contentPath.Substring(1);
            }

            if (contentPath.StartsWith("~"))
            {
                return $"/{contentPath.Substring(1)}";
            }

            return contentPath;
        }
    }
}