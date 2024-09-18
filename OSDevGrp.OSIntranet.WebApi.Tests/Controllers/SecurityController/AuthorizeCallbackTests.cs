using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;
using OSDevGrp.OSIntranet.WebApi.Security;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class AuthorizeCallbackTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<IDataProtector> _dataProtectorMock;
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _dataProtectorMock = new Mock<IDataProtector>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenCalled_AssertAuthenticateAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.AuthenticateAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateAuthorizationCodeCommand()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsAny<IGenerateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoResultWasReturnedFromAuthenticationService_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoResultWasReturnedFromAuthenticationService_ReturnsNotNull()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoResultWasReturnedFromAuthenticationService_ReturnsUnauthorizedObjectResult()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoResultWasReturnedFromAuthenticationService_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateNoResult();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            ((UnauthorizedObjectResult) result).AssertExpectedErrorResponseModel("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenFailWasReturnedFromAuthenticationService_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenFailWasReturnedFromAuthenticationService_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenFailWasReturnedFromAuthenticationService_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateAuthorizationCodeCommand()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsAny<IGenerateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenFailWasReturnedFromAuthenticationService_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenFailWasReturnedFromAuthenticationService_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenFailWasReturnedFromAuthenticationService_ReturnsNotNull()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenFailWasReturnedFromAuthenticationService_ReturnsUnauthorizedObjectResult()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenFailWasReturnedFromAuthenticationService_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            AuthenticateResult authenticateResult = CreateAuthenticateFail();
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            ((UnauthorizedObjectResult) result).AssertExpectedErrorResponseModel("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateAuthorizationCodeCommand()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsAny<IGenerateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_ReturnsNotNull()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_ReturnsUnauthorizedObjectResult()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasNoClaimsIdentity_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            ((UnauthorizedObjectResult) result).AssertExpectedErrorResponseModel("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateAuthorizationCodeCommand()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsAny<IGenerateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_ReturnsNotNull()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalUnauthenticatedClaimsIdentity_ReturnsUnauthorizedObjectResult()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalHasUnauthenticatedClaimsIdentity_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            ((UnauthorizedObjectResult) result).AssertExpectedErrorResponseModel("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateAuthorizationCodeCommand()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsAny<IGenerateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingNoEmailClaim_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingNoEmailClaim_ReturnsNotNull()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingNoEmailClaim_ReturnsUnauthorizedObjectResult()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingNoEmailClaim_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            ((UnauthorizedObjectResult) result).AssertExpectedErrorResponseModel("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateAuthorizationCodeCommand()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsAny<IGenerateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal, authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_ReturnsNotNull()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_ReturnsUnauthorizedObjectResult()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereClaimsPrincipalAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: true, claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            ((UnauthorizedObjectResult) result).AssertExpectedErrorResponseModel("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNoItems_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNoItems_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNoItems_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateAuthorizationCodeCommand()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsAny<IGenerateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNoItems_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNoItems_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNoItems_ReturnsNotNull()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNoItems_ReturnsUnauthorizedObjectResult()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNoItems_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            ((UnauthorizedObjectResult) result).AssertExpectedErrorResponseModel("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsNotContainingAuthorizationStateKey_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsNotContainingAuthorizationStateKey_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsNotContainingAuthorizationStateKey_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateAuthorizationCodeCommand()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsAny<IGenerateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsNotContainingAuthorizationStateKey_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsNotContainingAuthorizationStateKey_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsNotContainingAuthorizationStateKey_ReturnsNotNull()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsNotContainingAuthorizationStateKey_ReturnsUnauthorizedObjectResult()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsNotContainingAuthorizationStateKey_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            ((UnauthorizedObjectResult) result).AssertExpectedErrorResponseModel("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsContainingAuthorizationStateKeyWithoutValue_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsContainingAuthorizationStateKeyWithoutValue_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateUserCommand()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsContainingAuthorizationStateKeyWithoutValue_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateAuthorizationCodeCommand()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsAny<IGenerateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsContainingAuthorizationStateKeyWithoutValue_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsContainingAuthorizationStateKeyWithoutValue_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsContainingAuthorizationStateKeyWithoutValue_ReturnsNotNull()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsContainingAuthorizationStateKeyWithoutValue_ReturnsUnauthorizedObjectResult()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWhereAuthenticationPropertiesHasNonEmptyItemsContainingAuthorizationStateKeyWithoutValue_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(hasItems: true, hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: false);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            IActionResult result = await sut.AuthorizeCallback();

            ((UnauthorizedObjectResult) result).AssertExpectedErrorResponseModel("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWithExpectedValuesForAuthorization_AssertCreateProtectorWasNotCalledOnDataProtectionProviderWithTokenProtection()
        {
            Controller sut = CreateSut();

            await sut.AuthorizeCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "TokenProtection") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWithExpectedValuesForAuthorization_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateUserCommand()
        {
            Controller sut = CreateSut();

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsNotNull<IAuthenticateUserCommand>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWithExpectedValuesForAuthorization_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateUserCommandWhereExternalUserIdentifierIsEqualToMailAddressFromExternalUser()
        {
            string mailAddress = _fixture.Create<string>();
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(valueInEmailClaim: mailAddress);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.Is<IAuthenticateUserCommand>(value => value != null && string.IsNullOrWhiteSpace(value.ExternalUserIdentifier) == false && string.CompareOrdinal(value.ExternalUserIdentifier, mailAddress) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWithExpectedValuesForAuthorization_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateUserCommandWhereClaimsIsEqualToClaimsFromExternalUser()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(claimsPrincipal: claimsPrincipal);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.Is<IAuthenticateUserCommand>(value => value != null && value.Claims != null && value.Claims.Count == claimsIdentity.Claims.Count() && claimsIdentity.Claims.All(claim => value.Claims.Contains(claim)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWithExpectedValuesForAuthorization_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateUserCommandWhereWhereAuthenticationTypeIsEqualToInternalScheme()
        {
            Controller sut = CreateSut();

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.Is<IAuthenticateUserCommand>(value => value != null && string.IsNullOrWhiteSpace(value.AuthenticationType) == false && string.CompareOrdinal(value.AuthenticationType, Schemes.Internal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWithExpectedValuesForAuthorization_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateUserCommandWhereWhereAuthenticationSessionItemsIsEqualToItemsFromAuthenticationProperties()
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.Is<IAuthenticateUserCommand>(value => value != null && value.AuthenticationSessionItems != null && value.AuthenticationSessionItems.All(keyValuePair => authenticationProperties.Items.Contains(keyValuePair)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenSuccessWasReturnedFromAuthenticationServiceWithExpectedValuesForAuthorization_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateUserCommandWhereWhereProtectorIsNotNull()
        {
            Controller sut = CreateSut();

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.Is<IAuthenticateUserCommand>(value => value != null && value.Protector != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoInternalClaimsPrincipalWasReturnedFromCommandBus_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut(hasInternalClaimsPrincipal: false);

            await sut.AuthorizeCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoInternalClaimsPrincipalWasReturnedFromCommandBus_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut(hasInternalClaimsPrincipal: false);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsAny<IGenerateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoInternalClaimsPrincipalWasReturnedFromCommandBus_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut(hasInternalClaimsPrincipal: false);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoInternalClaimsPrincipalWasReturnedFromCommandBus_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult, hasInternalClaimsPrincipal: false);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoInternalClaimsPrincipalWasReturnedFromCommandBus_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasInternalClaimsPrincipal: false);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoInternalClaimsPrincipalWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResult()
        {
            Controller sut = CreateSut(hasInternalClaimsPrincipal: false);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoInternalClaimsPrincipalWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut(hasInternalClaimsPrincipal: false);

            IActionResult result = await sut.AuthorizeCallback();

            ((UnauthorizedObjectResult) result).AssertExpectedErrorResponseModel("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenInternalClaimsPrincipalWasReturnedFromCommandBus_AssertCreateProtectorWasCalledOnDataProtectionProviderWithAuthorizationStateProtection()
        {
            Controller sut = CreateSut();

            await sut.AuthorizeCallback();

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "AuthorizationStateProtection") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenInternalClaimsPrincipalWasReturnedFromCommandBus_AssertPublishAsyncWasCalledOnCommandBusWithGenerateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsNotNull<IGenerateAuthorizationCodeCommand>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenInternalClaimsPrincipalWasReturnedFromCommandBus_AssertPublishAsyncWasCalledOnCommandBusWithGenerateAuthorizationCodeCommandWhereAuthorizationStateIsEqualToValueForAuthorizationStateKeyFromAuthenticationProperties()
        {
            string valueForAuthorizationStateKey = _fixture.Create<string>();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(valueForAuthorizationStateKey: valueForAuthorizationStateKey);
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.Is<IGenerateAuthorizationCodeCommand>(value => value != null && string.IsNullOrWhiteSpace(value.AuthorizationState) == false && string.CompareOrdinal(value.AuthorizationState, valueForAuthorizationStateKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenInternalClaimsPrincipalWasReturnedFromCommandBus_AssertPublishAsyncWasCalledOnCommandBusWithGenerateAuthorizationCodeCommandWhereClaimsIsEqualToClaimsFromInternalClaimsPrincipalGivenByCommandBus()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity();
            ClaimsPrincipal internalClaimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(internalClaimsPrincipal: internalClaimsPrincipal);

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.Is<IGenerateAuthorizationCodeCommand>(value => value != null && value.Claims != null && claimsIdentity.Claims.All(claim => value.Claims.Contains(claim)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenInternalClaimsPrincipalWasReturnedFromCommandBus_AssertPublishAsyncWasCalledOnCommandBusWithGenerateAuthorizationCodeCommandWhereUnprotectIsNotNull()
        {
            Controller sut = CreateSut();

            await sut.AuthorizeCallback();

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.Is<IGenerateAuthorizationCodeCommand>(value => value != null && value.Unprotect != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenInternalClaimsPrincipalWasReturnedFromCommandBus_AssertSignInAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            ClaimsPrincipal internalClaimsPrincipal = CreateClaimsPrincipal();
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult, internalClaimsPrincipal: internalClaimsPrincipal);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<ClaimsPrincipal>(value => value != null && value == internalClaimsPrincipal),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult, hasAuthorizationStateWithAuthorizationCode: false);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasAuthorizationStateWithAuthorizationCode: false);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResult()
        {
            Controller sut = CreateSut(hasAuthorizationStateWithAuthorizationCode: false);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenNoAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut(hasAuthorizationStateWithAuthorizationCode: false);

            IActionResult result = await sut.AuthorizeCallback();

            ((UnauthorizedObjectResult) result).AssertExpectedErrorResponseModel("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_AssertAuthorizationCodeWasCalledOnAuthorizationStateGeneratedByCommandBus()
        {
            Mock<IAuthorizationState> authorizationStateWithAuthorizationCodeMock = _fixture.BuildAuthorizationStateMock(hasAuthorizationCode: true);
            Controller sut = CreateSut(authorizationStateWithAuthorizationCode: authorizationStateWithAuthorizationCodeMock.Object);

            await sut.AuthorizeCallback();

            authorizationStateWithAuthorizationCodeMock.Verify(m => m.AuthorizationCode, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_AssertValueWasCalledOnAuthorizationCodeInAuthorizationStateGeneratedByCommandBus()
        {
            Mock<IAuthorizationCode> authorizationCodeMock = _fixture.BuildAuthorizationCodeMock();
            IAuthorizationState authorizationStateWithAuthorizationCode = _fixture.BuildAuthorizationStateMock(hasAuthorizationCode: true, authorizationCode: authorizationCodeMock.Object).Object;
            Controller sut = CreateSut(authorizationStateWithAuthorizationCode: authorizationStateWithAuthorizationCode);

            await sut.AuthorizeCallback();

            authorizationCodeMock.Verify(m => m.Value, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_AssertGenerateRedirectUriWithAuthorizationCodeWasCalledOnAuthorizationStateGeneratedByCommandBus()
        {
            Mock<IAuthorizationState> authorizationStateWithAuthorizationCodeMock = _fixture.BuildAuthorizationStateMock(hasAuthorizationCode: true);
            Controller sut = CreateSut(authorizationStateWithAuthorizationCode: authorizationStateWithAuthorizationCodeMock.Object);

            await sut.AuthorizeCallback();

            authorizationStateWithAuthorizationCodeMock.Verify(m => m.GenerateRedirectUriWithAuthorizationCode(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_ReturnsRedirectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_ReturnsRedirectResultWhereUrlIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectResult result = (RedirectResult) await sut.AuthorizeCallback();

            Assert.That(result.Url, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_ReturnsRedirectResultWhereUrlIsNotEmpty()
        {
            Controller sut = CreateSut();

            RedirectResult result = (RedirectResult) await sut.AuthorizeCallback();

            Assert.That(result.Url, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_ReturnsRedirectResultWhereUrlIsEqualToRedirectUriWithAuthorizationCode()
        {
            KeyValuePair<string, string>[] queryParameters =
            {
                _fixture.CreateQueryParameter("code"),
                _fixture.CreateQueryParameter("state")
            };
            Uri redirectUriWithAuthorizationCode = _fixture.CreateEndpoint(queryParameters: queryParameters);
            IAuthorizationState authorizationStateWithAuthorizationCode = _fixture.BuildAuthorizationStateMock(hasAuthorizationCode: true, redirectUriWithAuthorizationCode: redirectUriWithAuthorizationCode).Object;
            Controller sut = CreateSut(authorizationStateWithAuthorizationCode: authorizationStateWithAuthorizationCode);

            RedirectResult result = (RedirectResult) await sut.AuthorizeCallback();

            Assert.That(result.Url, Is.EqualTo(redirectUriWithAuthorizationCode.ToString()));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_ReturnsRedirectResultWherePermanentIsTrue()
        {
            Controller sut = CreateSut();

            RedirectResult result = (RedirectResult) await sut.AuthorizeCallback();

            Assert.That(result.Permanent, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenAuthorizationStateWithAuthorizationCodeWasReturnedFromCommandBus_ReturnsRedirectResultWherePreserveMethodIsFalse()
        {
            Controller sut = CreateSut();

            RedirectResult result = (RedirectResult) await sut.AuthorizeCallback();

            Assert.That(result.PreserveMethod, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetValidationExceptionIsThrown_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            IntranetValidationException exception = CreateIntranetValidationException();
            Controller sut = CreateSut(exception: exception);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetValidationExceptionIsThrown_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            IntranetValidationException exception = CreateIntranetValidationException();
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult, exception: exception);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetValidationExceptionIsThrown_ReturnsNotNull()
        {
            IntranetValidationException exception = CreateIntranetValidationException();
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetValidationExceptionIsThrown_ReturnsBadRequestObjectResult()
        {
            IntranetValidationException exception = CreateIntranetValidationException();
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetValidationExceptionIsThrown_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            string errorDescription = _fixture.Create<string>();
            IntranetValidationException exception = CreateIntranetValidationException(message: errorDescription);
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", errorDescription, null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetBusinessExceptionIsThrown_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            IntranetBusinessException exception = CreateIntranetBusinessException(errorCode: ErrorCode.UnableToAuthorizeUser);
            Controller sut = CreateSut(exception: exception);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetBusinessExceptionIsThrown_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            IntranetBusinessException exception = CreateIntranetBusinessException(errorCode: ErrorCode.UnableToAuthorizeUser);
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult, exception: exception);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetBusinessExceptionIsThrown_ReturnsNotNull()
        {
            IntranetBusinessException exception = CreateIntranetBusinessException(errorCode: ErrorCode.UnableToAuthorizeUser);
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetBusinessExceptionIsThrown_ReturnsUnauthorizedObjectResult()
        {
            IntranetBusinessException exception = CreateIntranetBusinessException(errorCode: ErrorCode.UnableToAuthorizeUser);
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetBusinessExceptionIsThrown_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            string errorDescription = _fixture.Create<string>();
            IntranetBusinessException exception = CreateIntranetBusinessException(errorCode: ErrorCode.UnableToAuthorizeUser, message: errorDescription);
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            ((UnauthorizedObjectResult) result).AssertExpectedErrorResponseModel("access_denied", errorDescription, null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetExceptionBaseIsThrown_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            IntranetExceptionBase exception = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: exception);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetExceptionBaseIsThrown_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            IntranetExceptionBase exception = CreateIntranetExceptionBase();
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult, exception: exception);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetExceptionBaseIsThrown_ReturnsNotNull()
        {
            IntranetExceptionBase exception = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetExceptionBaseIsThrown_ReturnsObjectResult()
        {
            IntranetExceptionBase exception = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<ObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetExceptionBaseIsThrown_ReturnsObjectResultWithStatusCodeEqualToStatus500InternalServerError()
        {
            IntranetExceptionBase exception = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            ((ObjectResult) result).AssertExpectedStatusCode(StatusCodes.Status500InternalServerError);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenIntranetExceptionBaseIsThrown_ReturnsObjectResultWithExpectedErrorResponseModel()
        {
            IntranetExceptionBase exception = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            ((ObjectResult) result).AssertExpectedErrorResponseModel("server_error", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenExceptionIsThrown_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenExceptionIsThrown_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            AuthenticationProperties authenticationProperties = CreateAuthenticationProperties();
            AuthenticationTicket authenticationTicket = CreateAuthenticationTicket(authenticationProperties: authenticationProperties);
            AuthenticateResult authenticateResult = CreateAuthenticateSuccess(authenticationTicket: authenticationTicket);
            Exception exception = CreateException();
            Controller sut = CreateSut(httpContext: httpContext, authenticateResult: authenticateResult, exception: exception);

            await sut.AuthorizeCallback();

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value != null && value == authenticationProperties)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenExceptionIsThrown_ReturnsNotNull()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenExceptionIsThrown_ReturnsObjectResult()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            Assert.That(result, Is.TypeOf<ObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenExceptionIsThrown_ReturnsObjectResultWithStatusCodeEqualToStatus500InternalServerError()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            ((ObjectResult) result).AssertExpectedStatusCode(StatusCodes.Status500InternalServerError);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeCallback_WhenExceptionIsThrown_ReturnsObjectResultWithExpectedErrorResponseModel()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AuthorizeCallback();

            ((ObjectResult) result).AssertExpectedErrorResponseModel("server_error", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null);
        }

        private Controller CreateSut(HttpContext httpContext = null, AuthenticateResult authenticateResult = null, bool hasInternalClaimsPrincipal = true, ClaimsPrincipal internalClaimsPrincipal = null, bool hasAuthorizationStateWithAuthorizationCode = true, IAuthorizationState authorizationStateWithAuthorizationCode = null, Exception exception = null)
        {
            _authenticationServiceMock.Setup(m => m.AuthenticateAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
                .Returns(Task.FromResult(authenticateResult ?? CreateAuthenticateSuccess()));
            _authenticationServiceMock.Setup(m => m.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);
            _authenticationServiceMock.Setup(m => m.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            _dataProtectionProviderMock.Setup(m => m.CreateProtector(It.IsAny<string>()))
                .Returns(_dataProtectorMock.Object);

            if (exception == null)
            {
                _commandBusMock.Setup(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()))
                    .Returns(Task.FromResult(hasInternalClaimsPrincipal ? internalClaimsPrincipal ?? CreateClaimsPrincipal() : null));
                _commandBusMock.Setup(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsAny<IGenerateAuthorizationCodeCommand>()))
                    .Returns(Task.FromResult(hasAuthorizationStateWithAuthorizationCode ? authorizationStateWithAuthorizationCode ?? _fixture.BuildAuthorizationStateMock(hasClientSecret: true, hasExternalState: true, hasAuthorizationCode: true).Object : null));
            }
            else
            {
                _commandBusMock.Setup(m => m.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateUserCommand>()))
                    .Throws(exception);
                _commandBusMock.Setup(m => m.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(It.IsAny<IGenerateAuthorizationCodeCommand>()))
                    .Throws(exception);
            }

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _dataProtectionProviderMock.Object)
            {
                ControllerContext =
                {
                    HttpContext = httpContext ?? CreateHttpContext()
                }
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

        private AuthenticationProperties CreateAuthenticationProperties(bool hasItems = true, bool hasAuthorizationStateKey = true, bool hasValueForAuthorizationStateKey = true, string valueForAuthorizationStateKey = null)
        {
            if (hasItems == false)
            {
                return new AuthenticationProperties();
            }

            IDictionary<string, string> items = new Dictionary<string, string>
            {
                {_fixture.Create<string>(), _fixture.Create<string>()},
                {_fixture.Create<string>(), _fixture.Create<string>()},
                {_fixture.Create<string>(), _fixture.Create<string>()}
            };

            if (hasAuthorizationStateKey)
            {
                items.Add(KeyNames.AuthorizationStateKey, hasValueForAuthorizationStateKey ? valueForAuthorizationStateKey ?? _fixture.Create<string>() : null);
            }

            return new AuthenticationProperties(items);
        }

        private IntranetValidationException CreateIntranetValidationException(string message = null, string validatingField = null)
        {
            return new IntranetValidationException(_fixture.Create<ErrorCode>(), message ?? _fixture.Create<string>())
            {
                ValidatingField = validatingField ?? _fixture.Create<string>()
            };
        }

        private IntranetBusinessException CreateIntranetBusinessException(ErrorCode? errorCode = null, string message = null)
        {
            return new IntranetBusinessException(errorCode ?? _fixture.Create<ErrorCode>(), message ?? _fixture.Create<string>());
        }

        private IntranetExceptionBase CreateIntranetExceptionBase()
        {
            return new IntranetSystemException(_fixture.Create<ErrorCode>(), _fixture.Create<string>());
        }

        private Exception CreateException()
        {
            return new Exception(_fixture.Create<string>());
        }
    }
}