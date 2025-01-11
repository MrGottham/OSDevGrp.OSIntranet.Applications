using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Security;
using OSDevGrp.OSIntranet.Mvc.Tests.Helpers;
using System;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.AccountController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountController
{
    [TestFixture]
    public class LogoutTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<IUrlHelper> _urlHelperMock;
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        [TestCase("https://localhost/users/me")]
        public async Task Logout_WhenCalled_AssertHandleLogoutAsyncWasCalledOnTokenHelperFactory(string returnUrl)
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.Logout();

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.Is<HttpContext>(value => value != null && value == httpContext)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        [TestCase("https://localhost/users/me")]
        public async Task Logout_WhenCalled_AssertHandleLogoutAsyncWasCalledOnAuthenticationServiceWithInternalScheme(string returnUrl)
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.Logout();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        [TestCase("https://localhost/users/me")]
        public async Task Logout_WhenCalled_AssertHandleLogoutAsyncWasCalledOnAuthenticationServiceWithExternalScheme(string returnUrl)
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.Logout();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNull_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.Logout();

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNull_AssertActionWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.Logout();

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNull_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            Controller sut = CreateSut();

            await sut.Logout();

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Logout();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNull_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Logout();

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNull_ReturnsRedirectToActionResultWhereControllerNameIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout();

            Assert.That(result.ControllerName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNull_ReturnsRedirectToActionResultWhereControllerNameIsNotEmpty()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout();

            Assert.That(result.ControllerName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNull_ReturnsRedirectToActionResultWhereControllerNameIsEqualToHome()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout();

            Assert.That(result.ControllerName, Is.EqualTo("Home"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNull_ReturnsRedirectToActionResultWhereActionNameIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout();

            Assert.That(result.ActionName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNull_ReturnsRedirectToActionResultWhereActionNameIsNotEmpty()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout();

            Assert.That(result.ActionName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNull_ReturnsRedirectToActionResultWhereActionNameIsEqualToIndex()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout();

            Assert.That(result.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsEmpty_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.Logout(string.Empty);

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsEmpty_AssertActionWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.Logout(string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsEmpty_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            Controller sut = CreateSut();

            await sut.Logout(string.Empty);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Logout(string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsEmpty_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Logout(string.Empty);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsEmpty_ReturnsRedirectToActionResultWhereControllerNameIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout(string.Empty);

            Assert.That(result.ControllerName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsEmpty_ReturnsRedirectToActionResultWhereControllerNameIsNotEmpty()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout(string.Empty);

            Assert.That(result.ControllerName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsEmpty_ReturnsRedirectToActionResultWhereControllerNameIsEqualToHome()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout(string.Empty);

            Assert.That(result.ControllerName, Is.EqualTo("Home"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsEmpty_ReturnsRedirectToActionResultWhereActionNameIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout(string.Empty);

            Assert.That(result.ActionName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsEmpty_ReturnsRedirectToActionResultWhereActionNameIsNotEmpty()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout(string.Empty);

            Assert.That(result.ActionName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsEmpty_ReturnsRedirectToActionResultWhereActionNameIsEqualToIndex()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout(string.Empty);

            Assert.That(result.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsWhiteSpace_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.Logout(" ");

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsWhiteSpace_AssertActionWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.Logout(" ");

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsWhiteSpace_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            Controller sut = CreateSut();

            await sut.Logout(" ");

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Logout(" ");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsWhiteSpace_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Logout(" ");

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsWhiteSpace_ReturnsRedirectToActionResultWhereControllerNameIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout(" ");

            Assert.That(result.ControllerName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsWhiteSpace_ReturnsRedirectToActionResultWhereControllerNameIsNotEmpty()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout(" ");

            Assert.That(result.ControllerName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsWhiteSpace_ReturnsRedirectToActionResultWhereControllerNameIsEqualToHome()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout(" ");

            Assert.That(result.ControllerName, Is.EqualTo("Home"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsWhiteSpace_ReturnsRedirectToActionResultWhereActionNameIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout(" ");

            Assert.That(result.ActionName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsWhiteSpace_ReturnsRedirectToActionResultWhereActionNameIsNotEmpty()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout(" ");

            Assert.That(result.ActionName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsWhiteSpace_ReturnsRedirectToActionResultWhereActionNameIsEqualToIndex()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.Logout(" ");

            Assert.That(result.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me", "users/me", "~/users/me")]
        [TestCase("/users/me", "users/me", "~/users/me")]
        [TestCase("~/users/me", "users/me", "~/users/me")]
        public async Task Logout_WhenReturnUrlIsRelativeUrl_AssertContentWasCalledOnUrlHelperWithRelativeUrlForReturnUrl(string returnUrl, string pathBase, string expected)
        {
            Controller sut = CreateSut(pathBase: pathBase);

            await sut.Logout(returnUrl);

            _urlHelperMock.Verify(m => m.Content(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, expected) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task Logout_WhenReturnUrlIsRelativeUrl_AssertActionWasNotCalledOnUrlHelper(string returnUrl)
        {
            Controller sut = CreateSut();

            await sut.Logout(returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task Logout_WhenReturnUrlIsRelativeUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUrlForReturnUrl(string returnUrl)
        {
            string domainName = _fixture.CreateDomainName();
            string pathBase = _fixture.Create<string>();
            Controller sut = CreateSut(domainName: domainName, pathBase: pathBase);

            await sut.Logout(returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, $"https://{domainName}/{pathBase}/users/me") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task Logout_WhenReturnUrlIsNonTrustedRelativeUrl_ReturnsNotNull(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = await sut.Logout(returnUrl);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task Logout_WhenReturnUrlIsNonTrustedRelativeUrl_ReturnsBadRequestResult(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = await sut.Logout(returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task Logout_WhenReturnUrlIsTrustedRelativeUrl_ReturnsNotNull(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: true);

            IActionResult result = await sut.Logout(returnUrl);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task Logout_WhenReturnUrlIsTrustedRelativeUrl_ReturnsRedirectResult(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: true);

            IActionResult result = await sut.Logout(returnUrl);

            Assert.That(result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task Logout_WhenReturnUrlIsTrustedRelativeUrl_ReturnsRedirectResultWhereUrlIsNotNull(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: true);

            RedirectResult result = (RedirectResult) await sut.Logout(returnUrl);

            Assert.That(result.Url, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task Logout_WhenReturnUrlIsTrustedRelativeUrl_ReturnsRedirectResultWhereUrlIsNotEmpty(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: true);

            RedirectResult result = (RedirectResult) await sut.Logout(returnUrl);

            Assert.That(result.Url, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task Logout_WhenReturnUrlIsTrustedRelativeUrl_ReturnsRedirectResultWhereUrIsEqualToAbsolutePathForRelativeUrl(string returnUrl)
        {
            string domainName = _fixture.CreateDomainName();
            string pathBase = _fixture.Create<string>();
            Controller sut = CreateSut(domainName: domainName, pathBase: pathBase, isTrustedDomain: true);

            RedirectResult result = (RedirectResult) await sut.Logout(returnUrl);

            Assert.That(result.Url, Is.EqualTo($"https://{domainName}/{pathBase}/users/me"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsAbsoluteUrl_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.Logout(_fixture.CreateEndpointString(domainName: "localhost"));

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsAbsoluteUrl_AssertActionWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.Logout(_fixture.CreateEndpointString(domainName: "localhost"));

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsAbsoluteUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUrl()
        {
            Controller sut = CreateSut();

            string absoluteUrl = _fixture.CreateEndpointString(domainName: "localhost");
            await sut.Logout(absoluteUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, absoluteUrl) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNonTrustedAbsoluteUrl_ReturnsNotNull()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = await sut.Logout(_fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNonTrustedAbsoluteUrl_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = await sut.Logout(_fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsNotNull()
        {
            Controller sut = CreateSut(isTrustedDomain: true);

            IActionResult result = await sut.Logout(_fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsRedirectResult()
        {
            Controller sut = CreateSut(isTrustedDomain: true);

            IActionResult result = await sut.Logout(_fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsRedirectResultWhereUrlIsNotNull()
        {
            Controller sut = CreateSut(isTrustedDomain: true);

            RedirectResult result = (RedirectResult) await sut.Logout(_fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result.Url, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsRedirectResultWhereUrlIsNotEmpty()
        {
            Controller sut = CreateSut(isTrustedDomain: true);

            RedirectResult result = (RedirectResult) await sut.Logout(_fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result.Url, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsTrustedRelativeUrl_ReturnsRedirectResultWhereUrIsEqualToAbsoluteUrl()
        {
            Controller sut = CreateSut(isTrustedDomain: true);

            string absoluteUrl = _fixture.CreateEndpointString(domainName: "localhost");
            RedirectResult result = (RedirectResult) await sut.Logout(absoluteUrl);

            Assert.That(result.Url, Is.EqualTo(absoluteUrl));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.Logout(GetNonAbsoluteNorRelativeUrl());

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertActionWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.Logout(GetNonAbsoluteNorRelativeUrl());

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertIsTrustedDomainWasNonCalledOnTrustedDomainResolver()
        {
            Controller sut = CreateSut();

            await sut.Logout(GetNonAbsoluteNorRelativeUrl());

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Logout(GetNonAbsoluteNorRelativeUrl());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Logout_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Logout(GetNonAbsoluteNorRelativeUrl());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        private Controller CreateSut(HttpContext httpContext = null, string domainName = null, string pathBase = null, bool isTrustedDomain = true)
        {
            _trustedDomainResolverMock.Setup(m => m.IsTrustedDomain(It.IsAny<Uri>()))
                .Returns(isTrustedDomain);

            _urlHelperMock.Setup(_fixture, host: domainName, pathBase: pathBase);

            _authenticationServiceMock.Setup(m => m.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            _tokenHelperFactoryMock.Setup(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()))
                .Returns(Task.CompletedTask);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _trustedDomainResolverMock.Object, _tokenHelperFactoryMock.Object, _dataProtectionProviderMock.Object)
            {
                ControllerContext =
                {
                    HttpContext = httpContext ?? CreateHttpContext()
                },
                Url = _urlHelperMock.Object
            };
        }

        private HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext
            {
                RequestServices = CreateServiceProvider()
            };
        }

        private IServiceProvider CreateServiceProvider()
        {
            return CreateServiceCollection().BuildServiceProvider();
        }

        private IServiceCollection CreateServiceCollection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient(_ => _authenticationServiceMock.Object);
            return serviceCollection;
        }

        private string GetNonAbsoluteNorRelativeUrl()
        {
            return $"https://localhost:xyz/{_fixture.Create<string>()}";
        }
    }
}