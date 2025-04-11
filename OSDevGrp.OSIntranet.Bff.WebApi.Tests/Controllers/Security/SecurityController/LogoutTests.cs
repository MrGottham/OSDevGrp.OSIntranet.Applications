using AutoFixture;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.ErrorHandling.ProblemDetailsFactory;
using System.Net;
using System.Net.Mime;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Security.SecurityController;

[TestFixture]
public class LogoutTests : SecurityControllerTestBase
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
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("XYZ")]
    [TestCase("https://localhost:xyz")]
    public void Logout_WhenReturnUrlIsNonAbsoluteUrl_AssertCreateProblemDetailsForBadRequestWasCalledOnProblemDetailsFactoryWithHttpRequestFromHttpContext(string returnUrl)
    {
        HttpContext httpContext = CreateHttpContext();
        WebApi.Controllers.Security.SecurityController sut = CreateSut(httpContext: httpContext);

        sut.Logout(returnUrl);

        _problemDetailsFactoryMock!.Verify(m => m.CreateProblemDetailsForBadRequest(
                It.Is<HttpRequest>(value => value == httpContext.Request), 
                It.IsAny<string>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("XYZ")]
    [TestCase("https://localhost:xyz")]
    public void Logout_WhenReturnUrlIsNonAbsoluteUrl_AssertCreateProblemDetailsForBadRequestWasCalledOnProblemDetailsFactoryWithSpecificDetailText(string returnUrl)
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        sut.Logout(returnUrl);

        _problemDetailsFactoryMock!.Verify(m => m.CreateProblemDetailsForBadRequest(
                It.IsAny<HttpRequest>(), 
                It.Is<string>(value => value == "The given return URL is not an absolute URL.")), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("XYZ")]
    [TestCase("https://localhost:xyz")]
    public void Logout_WhenReturnUrlIsNonAbsoluteUrl_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver(string returnUrl)
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        sut.Logout(returnUrl);

        _trustedDomainResolverMock!.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("XYZ")]
    [TestCase("https://localhost:xyz")]
    public void Logout_WhenReturnUrlIsNonAbsoluteUrl_ReturnsBadRequestObjectResult(string returnUrl)
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        IActionResult result = sut.Logout(returnUrl);

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("XYZ")]
    [TestCase("https://localhost:xyz")]
    public void Logout_WhenReturnUrlIsNonAbsoluteUrl_ReturnsBadRequestObjectResultWhereStatusCodeIsEqualToBadRequest(string returnUrl)
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        BadRequestObjectResult result = (BadRequestObjectResult) sut.Logout(returnUrl);

        Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("XYZ")]
    [TestCase("https://localhost:xyz")]
    public void Logout_WhenReturnUrlIsNonAbsoluteUrl_ReturnsBadRequestObjectResultWhereValueIsProblemDetails(string returnUrl)
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        BadRequestObjectResult result = (BadRequestObjectResult) sut.Logout(returnUrl);

        Assert.That(result.Value, Is.TypeOf<ProblemDetails>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("XYZ")]
    [TestCase("https://localhost:xyz")]
    public void Logout_WhenReturnUrlIsNonAbsoluteUrl_ReturnsBadRequestObjectResultWhereValueIsEqualToProblemDetailsCreatedByProblemDetailsFactory(string returnUrl)
    {
        ProblemDetails problemDetails = _fixture!.CreateProblemDetails();
        WebApi.Controllers.Security.SecurityController sut = CreateSut(problemDetails: problemDetails);

        BadRequestObjectResult result = (BadRequestObjectResult) sut.Logout(returnUrl);

        Assert.That(result.Value, Is.EqualTo(problemDetails));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("XYZ")]
    [TestCase("https://localhost:xyz")]
    public void Logout_WhenReturnUrlIsNonAbsoluteUrl_ReturnsBadRequestObjectResultWhereContentTypesContainsContentTypeForApplicationProlemJson(string returnUrl)
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        BadRequestObjectResult result = (BadRequestObjectResult) sut.Logout(returnUrl);

        Assert.That(result.ContentTypes.Contains(MediaTypeNames.Application.ProblemJson), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsAbsoluteUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUriForGivenReturnUrl()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        sut.Logout(returnUrl);

        _trustedDomainResolverMock!.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value.AbsoluteUri == returnUrl)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsUntrustedAbsoluteUrl_AssertCreateProblemDetailsForBadRequestWasCalledOnProblemDetailsFactoryWithHttpRequestFromHttpContext()
    {
        HttpContext httpContext = CreateHttpContext();
        WebApi.Controllers.Security.SecurityController sut = CreateSut(httpContext: httpContext, isTrustedDomain: false);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        sut.Logout(returnUrl);

        _problemDetailsFactoryMock!.Verify(m => m.CreateProblemDetailsForBadRequest(
                It.Is<HttpRequest>(value => value == httpContext.Request), 
                It.IsAny<string>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsUntrustedAbsoluteUrl_AssertCreateProblemDetailsForBadRequestWasCalledOnProblemDetailsFactoryWithSpecificDetailText()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut(isTrustedDomain: false);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        sut.Logout(returnUrl);

        _problemDetailsFactoryMock!.Verify(m => m.CreateProblemDetailsForBadRequest(
                It.IsAny<HttpRequest>(), 
                It.Is<string>(value => value == "The given return URL is not a valid URL in this context.")), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsUntrustedAbsoluteUrl_ReturnsBadRequestObjectResult()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut(isTrustedDomain: false);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        IActionResult result = sut.Logout(returnUrl);

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsUntrustedAbsoluteUrl_ReturnsBadRequestObjectResultWhereStatusCodeIsEqualToBadRequest()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut(isTrustedDomain: false);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        BadRequestObjectResult result = (BadRequestObjectResult) sut.Logout(returnUrl);

        Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsUntrustedAbsoluteUrl_ReturnsBadRequestObjectResultWhereValueIsProblemDetails()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut(isTrustedDomain: false);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        BadRequestObjectResult result = (BadRequestObjectResult) sut.Logout(returnUrl);

        Assert.That(result.Value, Is.TypeOf<ProblemDetails>());
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsUntrustedAbsoluteUrl_ReturnsBadRequestObjectResultWhereValueIsEqualToProblemDetailsCreatedByProblemDetailsFactory()
    {
        ProblemDetails problemDetails = _fixture!.CreateProblemDetails();
        WebApi.Controllers.Security.SecurityController sut = CreateSut(problemDetails: problemDetails, isTrustedDomain: false);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        BadRequestObjectResult result = (BadRequestObjectResult) sut.Logout(returnUrl);

        Assert.That(result.Value, Is.EqualTo(problemDetails));
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsUntrustedAbsoluteUrl_ReturnsBadRequestObjectResultWhereContentTypesContainsContentTypeForApplicationProlemJson()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut(isTrustedDomain: false);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        BadRequestObjectResult result = (BadRequestObjectResult) sut.Logout(returnUrl);

        Assert.That(result.ContentTypes.Contains(MediaTypeNames.Application.ProblemJson), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsSignOutResult()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut(isTrustedDomain: true);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        IActionResult result = sut.Logout(returnUrl);

        Assert.That(result, Is.TypeOf<SignOutResult>());
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsSignOutResultWhereAuthenticationSchemesContainsTwoScheme()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut(isTrustedDomain: true);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        SignOutResult result = (SignOutResult) sut.Logout(returnUrl);

        Assert.That(result.AuthenticationSchemes.Count, Is.EqualTo(2));
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsSignOutResultWhereAuthenticationSchemesContainsSchemeForOpenIdConnect()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut(isTrustedDomain: true);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        SignOutResult result = (SignOutResult) sut.Logout(returnUrl);

        Assert.That(result.AuthenticationSchemes.Contains(OpenIdConnectDefaults.AuthenticationScheme), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsSignOutResultWhereAuthenticationSchemesContainsSchemeForInternal()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut(isTrustedDomain: true);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        SignOutResult result = (SignOutResult) sut.Logout(returnUrl);

        Assert.That(result.AuthenticationSchemes.Contains(Schemes.Internal), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsSignOutResultWherePropertiesIsNotNull()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut(isTrustedDomain: true);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        SignOutResult result = (SignOutResult) sut.Logout(returnUrl);

        Assert.That(result.Properties, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsSignOutResultWhereRedirectUriInPropertiesIsNotNull()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut(isTrustedDomain: true);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        SignOutResult result = (SignOutResult) sut.Logout(returnUrl);

        Assert.That(result.Properties!.RedirectUri, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void Logout_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsSignOutResultWhereRedirectUriInPropertiesIsEqualToGivenReturnUrl()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut(isTrustedDomain: true);

        string returnUrl = CreateAbsoluteReturnUrl(_fixture!);
        SignOutResult result = (SignOutResult) sut.Logout(returnUrl);

        Assert.That(result.Properties!.RedirectUri, Is.EqualTo(returnUrl));
    }

    protected override WebApi.Controllers.Security.SecurityController CreateSut(HttpContext? httpContext = null, ProblemDetails? problemDetails = null, bool isTrustedDomain = true)
    {
        return CreateSut(_problemDetailsFactoryMock!, _trustedDomainResolverMock!,  _fixture!, httpContext, problemDetails, isTrustedDomain);
    }
}