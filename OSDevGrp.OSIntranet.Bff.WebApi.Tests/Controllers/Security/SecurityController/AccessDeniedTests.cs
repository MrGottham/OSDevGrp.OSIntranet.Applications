using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Security.SecurityController;

[TestFixture]
public class AccessDeniedTests : SecurityControllerTestBase
{
    #region Private variables

    private Mock<IProblemDetailsFactory>? _problemDetailsFactoryMock;
    private Mock<ITrustedDomainResolver>? _trustedDomainResolverMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _problemDetailsFactoryMock = new Mock<IProblemDetailsFactory>();
        _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
        _fixture = new Fixture();
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

    protected override WebApi.Controllers.Security.SecurityController CreateSut(HttpContext? httpContext = null, ProblemDetails? problemDetails = null, bool isTrustedDomain = true)
    {
        return CreateSut(_problemDetailsFactoryMock!, _trustedDomainResolverMock!, _fixture!, httpContext, problemDetails, isTrustedDomain);
    }
}