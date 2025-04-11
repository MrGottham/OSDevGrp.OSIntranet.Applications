using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.ErrorHandling.ProblemDetailsFactory;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TrustedDomainResolver;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Security.SecurityController;

public abstract class SecurityControllerTestBase
{
    #region Methods

    protected abstract WebApi.Controllers.Security.SecurityController CreateSut(HttpContext? httpContext = null, ProblemDetails? problemDetails = null, bool isTrustedDomain = true);

    protected static WebApi.Controllers.Security.SecurityController CreateSut(Mock<IProblemDetailsFactory> problemDetailsFactoryMock, Mock<ITrustedDomainResolver> trustedDomainResolverMock, Fixture fixture, HttpContext? httpContext = null, ProblemDetails? problemDetails = null, bool isTrustedDomain = true)
    {
        problemDetailsFactoryMock.Setup(fixture, problemDetails: problemDetails);
        trustedDomainResolverMock.Setup(isTrustedDomain: isTrustedDomain);

        return new WebApi.Controllers.Security.SecurityController(problemDetailsFactoryMock.Object, trustedDomainResolverMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = httpContext ?? CreateHttpContext()
            }
        };
    }

    protected static HttpContext CreateHttpContext()
    {
        return new DefaultHttpContext();
    }

    protected static string CreateAbsoluteReturnUrl(Fixture fixture)
    {
        return new Uri($"https://{fixture.Create<string>()}.local/{fixture.Create<string>()}", UriKind.Absolute).AbsoluteUri;
    }

    #endregion
}