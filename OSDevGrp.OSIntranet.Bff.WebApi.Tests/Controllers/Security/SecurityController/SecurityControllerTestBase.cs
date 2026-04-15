using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.ErrorHandling.ProblemDetailsFactory;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.SecurityContextProvider;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TrustedDomainResolver;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Security.SecurityController;

public abstract class SecurityControllerTestBase<TQueryFeatureResponse> where TQueryFeatureResponse : class, IResponse
{
    #region Methods

    protected abstract WebApi.Controllers.Security.SecurityController CreateSut(HttpContext? httpContext = null, ProblemDetails? problemDetails = null, bool isTrustedDomain = true, IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null, TQueryFeatureResponse? queryFeatureResponse = null);

    protected static WebApi.Controllers.Security.SecurityController CreateSut(Mock<IProblemDetailsFactory> problemDetailsFactoryMock, Mock<ITrustedDomainResolver> trustedDomainResolverMock, Mock<ISecurityContextProvider> securityContextProviderMock, Fixture fixture, HttpContext? httpContext = null, ProblemDetails? problemDetails = null, bool isTrustedDomain = true, IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null)
    {
        problemDetailsFactoryMock.Setup(fixture, problemDetails: problemDetails);
        trustedDomainResolverMock.Setup(isTrustedDomain: isTrustedDomain);
        securityContextProviderMock!.Setup(fixture, securityContext: securityContext);

        return new WebApi.Controllers.Security.SecurityController(problemDetailsFactoryMock.Object, trustedDomainResolverMock.Object, formatProvider ?? CultureInfo.InvariantCulture, securityContextProviderMock.Object)
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