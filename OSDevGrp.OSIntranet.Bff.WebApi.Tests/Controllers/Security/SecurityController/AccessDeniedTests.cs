using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.AccessDeniedContent;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Security.SecurityController;

[TestFixture]
public class AccessDeniedTests : SecurityControllerTestBase<AccessDeniedContentResponse>
{
    #region Private variables

    private Mock<IProblemDetailsFactory>? _problemDetailsFactoryMock;
    private Mock<ITrustedDomainResolver>? _trustedDomainResolverMock;
    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _problemDetailsFactoryMock = new Mock<IProblemDetailsFactory>();
        _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public void AccessDenied_WhenCalled_ReturnsRedirectToPageResult()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        IActionResult result = sut.AccessDenied();

        Assert.That(result, Is.TypeOf<RedirectToPageResult>());
    }

    [Test]
    [Category("UnitTest")]
    public void AccessDenied_WhenCalled_ReturnsRedirectToPageResultWherePageNameIsNotNull()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        RedirectToPageResult result = (RedirectToPageResult) sut.AccessDenied();

        Assert.That(result.PageName, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void AccessDenied_WhenCalled_ReturnsRedirectToPageResultWherePageNameIsEqualToAccessDenied()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        RedirectToPageResult result = (RedirectToPageResult) sut.AccessDenied();

        Assert.That(result.PageName, Is.EqualTo("/AccessDenied"));
    }

    protected override WebApi.Controllers.Security.SecurityController CreateSut(HttpContext? httpContext = null, ProblemDetails? problemDetails = null, bool isTrustedDomain = true, IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null, AccessDeniedContentResponse? accessDeniedContentResponse = null)
    {
        return CreateSut(_problemDetailsFactoryMock!, _trustedDomainResolverMock!, _securityContextProviderMock!, _fixture!, _random!, httpContext, problemDetails, isTrustedDomain, formatProvider, securityContext);
    }
}