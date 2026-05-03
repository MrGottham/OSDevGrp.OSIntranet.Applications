using AutoFixture;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.AccessDeniedContent;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Security.SecurityController;

[TestFixture]
public class GetAntiforgeryTokenTests : SecurityControllerTestBase<AccessDeniedContentResponse>
{
    #region Private variables

    private Mock<IProblemDetailsFactory>? _problemDetailsFactoryMock;
    private Mock<ITrustedDomainResolver>? _trustedDomainResolverMock;
    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<IAntiforgery>? _antiforgeryMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _problemDetailsFactoryMock = new Mock<IProblemDetailsFactory>();
        _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _antiforgeryMock = new Mock<IAntiforgery>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public void GetAntiforgeryToken_WhenCalled_AssertGetAndStoreTokensWasCalledOnAntiforgeryWithHttpContext()
    {
        HttpContext httpContext = CreateHttpContext();
        WebApi.Controllers.Security.SecurityController sut = CreateSut(httpContext: httpContext);

        sut.GetAntiforgeryToken();

        _antiforgeryMock!.Verify(m => m.GetAndStoreTokens(It.Is<HttpContext>(value => value == httpContext)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public void GetAntiforgeryToken_WhenCalled_ReturnsOkObjectResult()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        IActionResult result = sut.GetAntiforgeryToken();

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    public void GetAntiforgeryToken_WhenCalled_ReturnsOkObjectResultWhereValueIsAntiforgeryTokenResponseDto()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        OkObjectResult result = (OkObjectResult) sut.GetAntiforgeryToken();

        Assert.That(result.Value, Is.TypeOf<AntiforgeryTokenResponseDto>());
    }

    protected override WebApi.Controllers.Security.SecurityController CreateSut(HttpContext? httpContext = null, ProblemDetails? problemDetails = null, bool isTrustedDomain = true, IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null, AccessDeniedContentResponse? accessDeniedContentResponse = null)
    {
        _antiforgeryMock!.Setup(m => m.GetAndStoreTokens(It.IsAny<HttpContext>()))
            .Returns(new AntiforgeryTokenSet(_fixture!.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()));

        return CreateSut(_problemDetailsFactoryMock!, _trustedDomainResolverMock!, _securityContextProviderMock!, _antiforgeryMock!, _fixture!, httpContext, problemDetails, isTrustedDomain, formatProvider, securityContext);
    }
}