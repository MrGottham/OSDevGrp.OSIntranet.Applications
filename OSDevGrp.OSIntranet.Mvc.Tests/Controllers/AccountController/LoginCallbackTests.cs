using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using OSDevGrp.OSIntranet.Mvc.Security;
using OSDevGrp.OSIntranet.Mvc.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.AccountController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountController
{
    [TestFixture]
    public class LoginCallbackTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<IDataProtector> _dataProtectorMock;
        private Mock<IUrlHelper> _urlHelperMock;
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _dataProtectorMock = new Mock<IDataProtector>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNull_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback();

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNull_AssertActionWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback();

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNull_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback();

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNull_AssertAuthenticateAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.AuthenticateAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNull_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsEmpty_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(string.Empty);

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsEmpty_AssertActionWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsEmpty_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(string.Empty);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsEmpty_AssertAuthenticateAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.LoginCallback(string.Empty);

            _authenticationServiceMock.Verify(m => m.AuthenticateAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsEmpty_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.LoginCallback(string.Empty);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsWhiteSpace_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(" ");

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsWhiteSpace_AssertActionWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(" ");

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsWhiteSpace_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(" ");

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsWhiteSpace_AssertAuthenticateAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.LoginCallback(" ");

            _authenticationServiceMock.Verify(m => m.AuthenticateAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsWhiteSpace_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.LoginCallback(" ");

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me", "users/me", "~/users/me")]
        [TestCase("/users/me", "users/me", "~/users/me")]
        [TestCase("~/users/me", "users/me", "~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsRelativeUrl_AssertContentWasCalledOnUrlHelperWithRelativeUrlForReturnUrl(string returnUrl, string pathBase, string expected)
        {
            Controller sut = CreateSut(pathBase: pathBase);

            await sut.LoginCallback(returnUrl);

            _urlHelperMock.Verify(m => m.Content(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, expected) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsRelativeUrl_AssertActionWasNotCalledOnUrlHelper(string returnUrl)
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsRelativeUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUrlForReturnUrl(string returnUrl)
        {
            string domainName = _fixture.CreateDomainName();
            string pathBase = _fixture.Create<string>();
            Controller sut = CreateSut(domainName: domainName, pathBase: pathBase);

            await sut.LoginCallback(returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, $"https://{domainName}/{pathBase}/users/me") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsTrustedRelativeUrl_AssertAuthenticateAsyncWasCalledOnAuthenticationServiceWithExternalScheme(string returnUrl)
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext, isTrustedDomain: true);

            await sut.LoginCallback(returnUrl);

            _authenticationServiceMock.Verify(m => m.AuthenticateAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsTrustedRelativeUrl_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme(string returnUrl)
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, isTrustedDomain: true, authenticateResult: authenticateResult);

            await sut.LoginCallback(returnUrl);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedRelativeUrl_AssertAuthenticateAsyncWasNotCalledOnAuthenticationServiceWithExternalScheme(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(returnUrl);

            _authenticationServiceMock.Verify(m => m.AuthenticateAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedRelativeUrl_AssertCreateProtectorWasNotCalledOnDataProtectionProvider(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(returnUrl);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedRelativeUrl_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(returnUrl);

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedRelativeUrl_AssertSignInAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(returnUrl);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedRelativeUrl_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMicrosoftTokenQuery(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(returnUrl);

            _queryBusMock.Verify(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.IsAny<IGetMicrosoftTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedRelativeUrl_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithMicrosoftGraphToken(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(returnUrl);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedRelativeUrl_AssertQueryAsyncWasNotCalledOnQueryBusWithGetGoogleTokenQuery(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(returnUrl);

            _queryBusMock.Verify(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.IsAny<IGetGoogleTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedRelativeUrl_AssertHandleLogoutAsyncWasNotCalledOnTokenHelperFactory(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(returnUrl);

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedRelativeUrl_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(returnUrl);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedRelativeUrl_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithExternalScheme(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(returnUrl);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedRelativeUrl_ReturnsNotNull(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = await sut.LoginCallback(returnUrl);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedRelativeUrl_ReturnsBadRequestResult(string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = await sut.LoginCallback(returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsAbsoluteUrl_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsAbsoluteUrl_AssertActionWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsAbsoluteUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithReturnUrl()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString(domainName: "localhost");
            await sut.LoginCallback(returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, returnUrl) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsTrustedAbsoluteUrl_AssertAuthenticateAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext, isTrustedDomain: true);

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _authenticationServiceMock.Verify(m => m.AuthenticateAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsTrustedAbsoluteUrl_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, isTrustedDomain: true, authenticateResult: authenticateResult);

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertAuthenticateAsyncWasNotCalledOnAuthenticationServiceWithExternalScheme()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _authenticationServiceMock.Verify(m => m.AuthenticateAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertSignInAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMicrosoftTokenQuery()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _queryBusMock.Verify(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.IsAny<IGetMicrosoftTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithMicrosoftGraphToken()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertQueryAsyncWasNotCalledOnQueryBusWithGetGoogleTokenQuery()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _queryBusMock.Verify(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.IsAny<IGetGoogleTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertHandleLogoutAsyncWasNotCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithExternalScheme()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedAbsoluteUrl_ReturnsNotNull()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonTrustedAbsoluteUrl_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertActionWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolverWithReturnUrl()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertAuthenticateAsyncWasNotCalledOnAuthenticationServiceWithExternalScheme()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _authenticationServiceMock.Verify(m => m.AuthenticateAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertSignInAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMicrosoftTokenQuery()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _queryBusMock.Verify(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.IsAny<IGetMicrosoftTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithMicrosoftGraphToken()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertQueryAsyncWasNotCalledOnQueryBusWithGetGoogleTokenQuery()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _queryBusMock.Verify(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.IsAny<IGetGoogleTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertHandleLogoutAsyncWasNotCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithExternalScheme()
        {
            Controller sut = CreateSut();

            await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenReturnUrlIsNonRelativeNorAbsoluteUrl_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.LoginCallback(GetNonAbsoluteNorRelativeUrl());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertSignInAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMicrosoftTokenQuery()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.IsAny<IGetMicrosoftTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithMicrosoftGraphToken()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertQueryAsyncWasNotCalledOnQueryBusWithGetGoogleTokenQuery()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.IsAny<IGetGoogleTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertHandleLogoutAsyncWasNotCalledOnTokenHelperFactory()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoResultWasReturnedFromAuthenticationService_ReturnsNotNull()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoResultWasReturnedFromAuthenticationService_ReturnsUnauthorizedResult()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenFailWasReturnedFromAuthenticationService_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenFailWasReturnedFromAuthenticationService_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenFailWasReturnedFromAuthenticationService_AssertSignInAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenFailWasReturnedFromAuthenticationService_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMicrosoftTokenQuery()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.IsAny<IGetMicrosoftTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenFailWasReturnedFromAuthenticationService_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithMicrosoftGraphToken()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenFailWasReturnedFromAuthenticationService_AssertQueryAsyncWasNotCalledOnQueryBusWithGetGoogleTokenQuery()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.IsAny<IGetGoogleTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenFailWasReturnedFromAuthenticationService_AssertHandleLogoutAsyncWasNotCalledOnTokenHelperFactory()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenFailWasReturnedFromAuthenticationService_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenFailWasReturnedFromAuthenticationService_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenFailWasReturnedFromAuthenticationService_ReturnsNotNull()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenFailWasReturnedFromAuthenticationService_ReturnsUnauthorizedResult()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertSignInAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMicrosoftTokenQuery()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.IsAny<IGetMicrosoftTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithMicrosoftGraphToken()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertQueryAsyncWasNotCalledOnQueryBusWithGetGoogleTokenQuery()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.IsAny<IGetGoogleTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertHandleLogoutAsyncWasNotCalledOnTokenHelperFactory()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_ReturnsNotNull()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_ReturnsUnauthorizedResult()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertSignInAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMicrosoftTokenQuery()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.IsAny<IGetMicrosoftTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithMicrosoftGraphToken()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertQueryAsyncWasNotCalledOnQueryBusWithGetGoogleTokenQuery()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.IsAny<IGetGoogleTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertHandleLogoutAsyncWasNotCalledOnTokenHelperFactory()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_ReturnsNotNull()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_ReturnsUnauthorizedResult()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertSignInAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMicrosoftTokenQuery()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.IsAny<IGetMicrosoftTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithMicrosoftGraphToken()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertQueryAsyncWasNotCalledOnQueryBusWithGetGoogleTokenQuery()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.IsAny<IGetGoogleTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertHandleLogoutAsyncWasNotCalledOnTokenHelperFactory()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingNoEmailClaim_ReturnsNotNull()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingNoEmailClaim_ReturnsUnauthorizedResult()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertSignInAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMicrosoftTokenQuery()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.IsAny<IGetMicrosoftTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithMicrosoftGraphToken()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertQueryAsyncWasNotCalledOnQueryBusWithGetGoogleTokenQuery()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.IsAny<IGetGoogleTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertHandleLogoutAsyncWasNotCalledOnTokenHelperFactory()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_ReturnsNotNull()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_ReturnsUnauthorizedResult()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithValue_AssertCreateProtectorWasCalledOnDataProtectionProviderWithTokenProtection()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: true);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "TokenProtection") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithValue_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateUserCommandWhereExternalUserIdentifierIsEqualToValueFromEmailClaim()
        {
            string emailClaimValue = _fixture.Create<string>();
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: true, valueInEmailClaim: emailClaimValue);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.Is<IAuthenticateUserCommand>(value => value != null && string.IsNullOrWhiteSpace(value.ExternalUserIdentifier) == false && string.CompareOrdinal(value.ExternalUserIdentifier, emailClaimValue) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithValue_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateUserCommandWhereClaimsIsEqualToClaimsFromClaimIdentity()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: true);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.Is<IAuthenticateUserCommand>(value => value != null && value.Claims != null && claimsIdentity.Claims.All(claim => value.Claims.Contains(claim)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithValue_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateUserCommandWhereAuthenticationTypeIsEqualToInternalScheme()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: true);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.Is<IAuthenticateUserCommand>(value => value != null && string.IsNullOrWhiteSpace(value.AuthenticationType) == false && string.CompareOrdinal(value.AuthenticationType, Schemes.Internal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithValue_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateUserCommandWhereAuthenticationSessionItemsIsEqualToItemsInAuthenticationProperties()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: true);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.Is<IAuthenticateUserCommand>(value => value != null && value.AuthenticationSessionItems != null && authenticationProperties.Items.All(item => value.AuthenticationSessionItems.ContainsKey(item.Key) && string.IsNullOrWhiteSpace(value.AuthenticationSessionItems[item.Key]) == false && string.CompareOrdinal(value.AuthenticationSessionItems[item.Key], item.Value) == 0))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasAuthenticatedClaimsIdentityContainingEmailClaimWithValue_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateUserCommandWhereProtectorIsNotNull()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: true);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.LoginCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.Is<IAuthenticateUserCommand>(value => value != null && value.Protector != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertSignInAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: false);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertQueryAsyncWasNotCalledOnQueryBusWithGetMicrosoftTokenQuery()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: false);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.IsAny<IGetMicrosoftTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithMicrosoftGraphToken()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: false);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertQueryAsyncWasNotCalledOnQueryBusWithGetGoogleTokenQuery()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: false);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.IsAny<IGetGoogleTokenQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertHandleLogoutAsyncWasNotCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: false);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: false);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult, hasAuthenticatedClaimsPrincipal: false);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: false);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: false);

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertSignInAsyncWasCalledOnAuthenticationServiceWithInternalSchemeAndAuthenticatedClaimsPrincipal()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsPrincipal authenticatedClaimsPrincipal = CreateClaimsPrincipal();
            Controller sut = CreateSut(httpContext: httpContext, hasAuthenticatedClaimsPrincipal: true, authenticatedClaimsPrincipal: authenticatedClaimsPrincipal);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<ClaimsPrincipal>(value => value != null && value == authenticatedClaimsPrincipal),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertQueryAsyncWasCalledOnQueryBusWithGetMicrosoftTokenQueryWhereClaimsPrincipalIsEqualToAuthenticatedClaimsPrincipal()
        {
            ClaimsPrincipal authenticatedClaimsPrincipal = CreateClaimsPrincipal();
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true, authenticatedClaimsPrincipal: authenticatedClaimsPrincipal);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.Is<IGetMicrosoftTokenQuery>(value => value != null && value.ClaimsPrincipal != null && value.ClaimsPrincipal == authenticatedClaimsPrincipal)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertQueryAsyncWasCalledOnQueryBusWithGetMicrosoftTokenQueryWhereUnprotectIsNotNull()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.Is<IGetMicrosoftTokenQuery>(value => value != null && value.Unprotect != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenMicrosoftTokenWasReturnedFromQueryBus_AssertToBase64StringWasCalledOnMicrosoftTokenFromQueryBus()
        {
            Mock<IRefreshableToken> microsoftTokenMock = _fixture.BuildRefreshableTokenMock();
            Controller sut = CreateSut(hasMicrosoftToken: true, microsoftToken: microsoftTokenMock.Object);

            await sut.LoginCallback();

            microsoftTokenMock.Verify(m => m.ToBase64String(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenMicrosoftTokenWasReturnedFromQueryBus_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithMicrosoftGraphTokenAndBase64ForMicrosoftToken()
        {
            HttpContext httpContext = CreateHttpContext();
            string base64 = _fixture.Create<string>();
            IRefreshableToken microsoftToken = _fixture.BuildRefreshableTokenMock(toBase64String: base64).Object;
            Controller sut = CreateSut(httpContext: httpContext, hasMicrosoftToken: true, microsoftToken: microsoftToken);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, base64) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoMicrosoftTokenWasReturnedFromQueryBus_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithMicrosoftGraphToken()
        {
            Controller sut = CreateSut(hasMicrosoftToken: false);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusButQueryForMicrosoftTokenFails_AssertHandleLogoutAsyncWasCalledOnTokenHelperFactory()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext, hasAuthenticatedClaimsPrincipal: true, getMicrosoftTokenFailure: _fixture.Create<Exception>());

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.Is<HttpContext>(value => value != null && value == httpContext)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusButQueryForMicrosoftTokenFails_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithInternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext, hasAuthenticatedClaimsPrincipal: true, getMicrosoftTokenFailure: _fixture.Create<Exception>());

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusButQueryForMicrosoftTokenFails_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult, hasAuthenticatedClaimsPrincipal: true, getMicrosoftTokenFailure: _fixture.Create<Exception>());

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusButQueryForMicrosoftTokenFails_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true, getMicrosoftTokenFailure: _fixture.Create<Exception>());

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusButQueryForMicrosoftTokenFails_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true, getMicrosoftTokenFailure: _fixture.Create<Exception>());

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertQueryAsyncWasCalledOnQueryBusWithGetGoogleTokenQueryWhereClaimsPrincipalIsEqualToAuthenticatedClaimsPrincipal()
        {
            ClaimsPrincipal authenticatedClaimsPrincipal = CreateClaimsPrincipal();
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true, authenticatedClaimsPrincipal: authenticatedClaimsPrincipal);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.Is<IGetGoogleTokenQuery>(value => value != null && value.ClaimsPrincipal != null && value.ClaimsPrincipal == authenticatedClaimsPrincipal)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertQueryAsyncWasCalledOnQueryBusWithGetGoogleTokenQueryWhereUnprotectIsNotNull()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            await sut.LoginCallback();

            _queryBusMock.Verify(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.Is<IGetGoogleTokenQuery>(value => value != null && value.Unprotect != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenGoogleTokenWasReturnedFromQueryBus_ExpectNoError()
        {
            Controller sut = CreateSut(hasGoogleToken: true);

            await sut.LoginCallback();
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenNoGoogleTokenWasReturnedFromQueryBus_ExpectNoError()
        {
            Controller sut = CreateSut(hasGoogleToken: false);

            await sut.LoginCallback();
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusButQueryForGoogleTokenFails_AssertHandleLogoutAsyncWasCalledOnTokenHelperFactory()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext, hasAuthenticatedClaimsPrincipal: true, getGoogleTokenFailure: _fixture.Create<Exception>());

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.Is<HttpContext>(value => value != null && value == httpContext)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusButQueryForGoogleTokenFails_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithInternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext, hasAuthenticatedClaimsPrincipal: true, getGoogleTokenFailure: _fixture.Create<Exception>());

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusButQueryForGoogleTokenFails_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult, hasAuthenticatedClaimsPrincipal: true, getGoogleTokenFailure: _fixture.Create<Exception>());

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusButQueryForGoogleTokenFails_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true, getGoogleTokenFailure: _fixture.Create<Exception>());

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusButQueryForGoogleTokenFails_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true, getGoogleTokenFailure: _fixture.Create<Exception>());

            IActionResult result = await sut.LoginCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertHandleLogoutAsyncWasNotCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            await sut.LoginCallback();

            _tokenHelperFactoryMock.Verify(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertSignOutAsyncWasNotCalledOnAuthenticationServiceWithInternalScheme()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBus_AssertSignOutAsyncWasCalledOnAuthenticationServiceWithExternalScheme()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult, hasAuthenticatedClaimsPrincipal: true);

            await sut.LoginCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.External) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsNullEmptyOrWhiteSpace_ReturnsNotNull(string returnUrl)
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            IActionResult result = await sut.LoginCallback(returnUrl);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsNullEmptyOrWhiteSpace_ReturnsRedirectToActionResult(string returnUrl)
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            IActionResult result = await sut.LoginCallback(returnUrl);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsNullEmptyOrWhiteSpace_ReturnsRedirectToActionResultWhereControllerNameIsNotNull(string returnUrl)
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            RedirectToActionResult result = (RedirectToActionResult) await sut.LoginCallback(returnUrl);

            Assert.That(result.ControllerName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsNullEmptyOrWhiteSpace_ReturnsRedirectToActionResultWhereControllerNameIsEmpty(string returnUrl)
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            RedirectToActionResult result = (RedirectToActionResult) await sut.LoginCallback(returnUrl);

            Assert.That(result.ControllerName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsNullEmptyOrWhiteSpace_ReturnsRedirectToActionResultWhereControllerNameIsEqualToHome(string returnUrl)
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            RedirectToActionResult result = (RedirectToActionResult) await sut.LoginCallback(returnUrl);

            Assert.That(result.ControllerName, Is.EqualTo("Home"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsNullEmptyOrWhiteSpace_ReturnsRedirectToActionResultWhereActionNameIsNotNull(string returnUrl)
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            RedirectToActionResult result = (RedirectToActionResult) await sut.LoginCallback(returnUrl);

            Assert.That(result.ActionName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsNullEmptyOrWhiteSpace_ReturnsRedirectToActionResultWhereActionNameIsEmpty(string returnUrl)
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            RedirectToActionResult result = (RedirectToActionResult) await sut.LoginCallback(returnUrl);

            Assert.That(result.ActionName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsNullEmptyOrWhiteSpace_ReturnsRedirectToActionResultWhereActionNameIsEqualToIndex(string returnUrl)
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            RedirectToActionResult result = (RedirectToActionResult) await sut.LoginCallback(returnUrl);

            Assert.That(result.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsRelativeUrl_ReturnsNotNull(string returnUrl)
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            IActionResult result = await sut.LoginCallback(returnUrl);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsRelativeUrl_ReturnsRedirectResult(string returnUrl)
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            IActionResult result = await sut.LoginCallback(returnUrl);

            Assert.That(result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsRelativeUrl_ReturnsRedirectResultWhereUrlIsNotNull(string returnUrl)
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            RedirectResult result = (RedirectResult) await sut.LoginCallback(returnUrl);

            Assert.That(result.Url, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsRelativeUrl_ReturnsRedirectResultWhereUrlIsNotEmpty(string returnUrl)
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            RedirectResult result = (RedirectResult) await sut.LoginCallback(returnUrl);

            Assert.That(result.Url, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("users/me")]
        [TestCase("/users/me")]
        [TestCase("~/users/me")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsRelativeUrl_ReturnsRedirectResultWhereUrlIsEqualToAbsolutePathForRelativeUrl(string returnUrl)
        {
            string domainName = _fixture.CreateDomainName();
            string pathBase = _fixture.Create<string>();
            Controller sut = CreateSut(domainName: domainName, pathBase: pathBase, hasAuthenticatedClaimsPrincipal: true);

            RedirectResult result = (RedirectResult) await sut.LoginCallback(returnUrl);

            Assert.That(result.Url, Is.EqualTo($"https://{domainName}/{pathBase}/users/me"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsAbsoluteUrl_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            IActionResult result = await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsAbsoluteUrl_ReturnsRedirectResult()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            IActionResult result = await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsAbsoluteUrl_ReturnsRedirectResultWhereUrlIsNotNull()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            RedirectResult result = (RedirectResult) await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result.Url, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsAbsoluteUrl_ReturnsRedirectResultWhereUrlIsNotEmpty()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            RedirectResult result = (RedirectResult) await sut.LoginCallback(_fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result.Url, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoginCallback_WhenAuthenticatedClaimsPrincipalWasReturnedFromCommandBusAndReturnUrlIsAbsoluteUrl_ReturnsRedirectResultWhereUrlIsEqualToReturnUrl()
        {
            Controller sut = CreateSut(hasAuthenticatedClaimsPrincipal: true);

            string returnUrl = _fixture.CreateEndpointString(domainName: "localhost");
            RedirectResult result = (RedirectResult) await sut.LoginCallback(returnUrl);

            Assert.That(result.Url, Is.EqualTo(returnUrl));
        }

        private Controller CreateSut(HttpContext httpContext = null, string domainName = null, string pathBase = null, bool isTrustedDomain = true, AuthenticateResult authenticateResult = null, bool hasAuthenticatedClaimsPrincipal = true, ClaimsPrincipal authenticatedClaimsPrincipal = null, bool hasMicrosoftToken = true, IRefreshableToken microsoftToken = null, Exception getMicrosoftTokenFailure = null, bool hasGoogleToken = true, IToken googleToken = null, Exception getGoogleTokenFailure = null)
        {
            _trustedDomainResolverMock.Setup(m => m.IsTrustedDomain(It.IsAny<Uri>()))
                .Returns(isTrustedDomain);

            _urlHelperMock.Setup(_fixture, host: domainName, pathBase: pathBase);

            _dataProtectionProviderMock.Setup(m => m.CreateProtector(It.IsAny<string>()))
                .Returns(_dataProtectorMock.Object);

            _authenticationServiceMock.Setup(m => m.AuthenticateAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
                .Returns(Task.FromResult(authenticateResult ?? CreateAuthenticateSuccess()));
            _authenticationServiceMock.Setup(m => m.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);
            _authenticationServiceMock.Setup(m => m.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            _commandBusMock.Setup(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()))
                .Returns(Task.FromResult(hasAuthenticatedClaimsPrincipal ? authenticatedClaimsPrincipal ?? CreateClaimsPrincipal() : null));

            if (getMicrosoftTokenFailure == null)
            {
                _queryBusMock.Setup(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.IsAny<IGetMicrosoftTokenQuery>()))
                    .Returns(Task.FromResult(hasMicrosoftToken ? microsoftToken ?? _fixture.BuildRefreshableTokenMock().Object : null));
            }
            else
            {
                _queryBusMock.Setup(m => m.QueryAsync<IGetMicrosoftTokenQuery, IRefreshableToken>(It.IsAny<IGetMicrosoftTokenQuery>()))
                    .Throws(getMicrosoftTokenFailure);
            }

            if (getGoogleTokenFailure == null)
            {
                _queryBusMock.Setup(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.IsAny<IGetGoogleTokenQuery>()))
                    .Returns(Task.FromResult(hasGoogleToken ? googleToken ?? _fixture.BuildTokenMock().Object : null));
            }
            else
            {
                _queryBusMock.Setup(m => m.QueryAsync<IGetGoogleTokenQuery, IToken>(It.IsAny<IGetGoogleTokenQuery>()))
                    .Throws(getGoogleTokenFailure);
            }

            _tokenHelperFactoryMock.Setup(m => m.StoreTokenAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<string>()))
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

        private AuthenticateResult CreateAuthenticateSuccess(AuthenticationTicket authenticationTicket = null)
        {
            return AuthenticateResult.Success(authenticationTicket ?? CreateAuthenticationTicket());
        }

        private AuthenticateResult CreateAuthenticateNoResult()
        {
            return AuthenticateResult.NoResult();
        }

        private AuthenticateResult CreateAuthenticateFail(Exception failure = null)
        {
            return AuthenticateResult.Fail(failure ?? new UnauthorizedAccessException());
        }

        private AuthenticationTicket CreateAuthenticationTicket(ClaimsPrincipal claimsPrincipal = null, AuthenticationProperties authenticationProperties = null)
        {
            return new AuthenticationTicket(
                claimsPrincipal ?? CreateClaimsPrincipal(),
                authenticationProperties ?? CreateAuthenticationProperties(),
                _fixture.Create<string>());
        }

        private ClaimsPrincipal CreateClaimsPrincipal(bool hasClaimsIdentity = true, ClaimsIdentity claimsIdentity = null)
        {
            return hasClaimsIdentity
                ? new ClaimsPrincipal(claimsIdentity ?? CreateClaimsIdentity())
                : new ClaimsPrincipal();
        }

        private ClaimsIdentity CreateClaimsIdentity(bool isAuthenticated = true, bool hasEmailClaim = true, bool hasValueInEmailClaim = true, string valueInEmailClaim = null)
        {
            if (isAuthenticated == false)
            {
                return new ClaimsIdentity();
            }

            Claim[] claims = _fixture.CreateClaims(_random);
            if (hasEmailClaim)
            {
                claims = claims.Concat(_fixture.CreateClaim(ClaimTypes.Email, hasValue: hasValueInEmailClaim, value: valueInEmailClaim));
            }

            return new ClaimsIdentity(claims, _fixture.Create<string>());
        }

        private AuthenticationProperties CreateAuthenticationProperties()
        {
            IDictionary<string, string> items = new Dictionary<string, string>
            {
                {_fixture.Create<string>(), _fixture.Create<string>()},
                {_fixture.Create<string>(), _fixture.Create<string>()},
                {_fixture.Create<string>(), _fixture.Create<string>()}
            };

            return new AuthenticationProperties(items);
        }

        private string GetNonAbsoluteNorRelativeUrl()
        {
            return $"https://localhost:xyz/{_fixture.Create<string>()}";
        }
    }
}