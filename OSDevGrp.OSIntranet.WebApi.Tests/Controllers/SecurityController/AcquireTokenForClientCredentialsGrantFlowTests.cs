using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;
using OSDevGrp.OSIntranet.WebApi.Security;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class AcquireTokenForClientCredentialsGrantFlowTests : AcquireTokenTestBase
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

        #region Properties

        protected override Mock<ICommandBus> CommandBusMock => _commandBusMock;

        protected override Mock<IQueryBus> QueryBusMock => _queryBusMock;

        protected override Mock<IDataProtectionProvider> DataProtectionProviderMock => _dataProtectionProviderMock;

        protected override Mock<IDataProtector> DataProtectorMock => _dataProtectorMock;

        protected override Mock<IAuthenticationService> AuthenticationServiceMock => _authenticationServiceMock;

        protected override Fixture Fixture => _fixture;

        protected override Random Random => _random;

        protected string GrantType => "client_credentials";

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
        public async Task AcquireToken_WhenAuthorizationIsNull_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateClientSecretCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: null);

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateClientSecretCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsNull_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: null);

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsNull_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: null);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsNull_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: null);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsNull_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, authorization: null);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, authorization: null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsNull_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, authorization: null);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsNull_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, authorization: null);

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "authorization"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsEmpty_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateClientSecretCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: string.Empty);

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateClientSecretCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsEmpty_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: string.Empty);

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsEmpty_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: string.Empty);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsEmpty_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: string.Empty);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsEmpty_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, authorization: string.Empty);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, authorization: string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsEmpty_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, authorization: string.Empty);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsEmpty_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, authorization: string.Empty);

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "authorization"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsWhiteSpace_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateClientSecretCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: " ");

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateClientSecretCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsWhiteSpace_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: " ");

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsWhiteSpace_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: " ");

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsWhiteSpace_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: " ");

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsWhiteSpace_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, authorization: " ");

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, authorization: " ");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsWhiteSpace_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, authorization: " ");

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationIsWhiteSpace_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, authorization: " ");

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "authorization"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForAuthorization_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateClientSecretCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: _fixture.Create<string>());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateClientSecretCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForAuthorization_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: _fixture.Create<string>());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForAuthorization_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: _fixture.Create<string>());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForAuthorization_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, authorization: _fixture.Create<string>());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForAuthorization_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, authorization: _fixture.Create<string>());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForAuthorization_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, authorization: _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForAuthorization_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, authorization: _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForAuthorization_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, authorization: _fixture.Create<string>());

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueShouldMatchPattern, "authorization", AuthorizationPattern), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForClientIdAndClientSecret_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateClientSecretCommand()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization(value: _fixture.Create<string>());
            await sut.AcquireToken(GrantType, authorization: authorization);

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateClientSecretCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForClientIdAndClientSecret_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization(value: _fixture.Create<string>());
            await sut.AcquireToken(GrantType, authorization: authorization);

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForClientIdAndClientSecret_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization(value: _fixture.Create<string>());
            await sut.AcquireToken(GrantType, authorization: authorization);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForClientIdAndClientSecret_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization(value: _fixture.Create<string>());
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForClientIdAndClientSecret_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            string authorization = CreateAuthorization(value: _fixture.Create<string>());
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForClientIdAndClientSecret_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization(value: _fixture.Create<string>());
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForClientIdAndClientSecret_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization(value: _fixture.Create<string>());
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationDoesNotMatchPatternForClientIdAndClientSecret_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization(value: _fixture.Create<string>());
            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, authorization: authorization);

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueShouldMatchPattern, "authorization", AuthorizationParameterForClientIdAndClientSecretPattern), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationMatchesPatternForClientIdAndClientSecret_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateClientSecretCommand()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(It.IsNotNull<IAuthenticateClientSecretCommand>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationMatchesPatternForClientIdAndClientSecret_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateClientSecretCommandWhereClientIdIsEqualToClientIdFromAuthorizationParameter()
        {
            Controller sut = CreateSut();

            string clientId = CreateClientId();
            string authorization = CreateAuthorization(value: CreateAuthorizationParameterForClientIdAndClientSecret(clientId: clientId));
            await sut.AcquireToken(GrantType, authorization: authorization);

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(It.Is<IAuthenticateClientSecretCommand>(value => value != null && string.IsNullOrWhiteSpace(value.ClientId) == false && string.CompareOrdinal(value.ClientId, clientId) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationMatchesPatternForClientIdAndClientSecret_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateClientSecretCommandWhereClientSecretIsEqualToClientSecretFromAuthorizationParameter()
        {
            Controller sut = CreateSut();

            string clientSecret = CreateClientSecret();
            string authorization = CreateAuthorization(value: CreateAuthorizationParameterForClientIdAndClientSecret(clientSecret: clientSecret));
            await sut.AcquireToken(GrantType, authorization: authorization);

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(It.Is<IAuthenticateClientSecretCommand>(value => value != null && string.IsNullOrWhiteSpace(value.ClientSecret) == false && string.CompareOrdinal(value.ClientSecret, clientSecret) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationMatchesPatternForClientIdAndClientSecret_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateClientSecretCommandWhereAuthenticationTypeIsEqualToInternalScheme()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(It.Is<IAuthenticateClientSecretCommand>(value => value != null && string.IsNullOrWhiteSpace(value.AuthenticationType) == false && string.CompareOrdinal(value.AuthenticationType, Schemes.Internal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenAuthorizationMatchesPatternForClientIdAndClientSecret_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateClientSecretCommandWhereProtectorIsNotNull()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(It.Is<IAuthenticateClientSecretCommand>(value => value != null && value.Protector != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoClaimsPrincipalWasReturnedFromCommandBus_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut(hasClaimsPrincipal: false);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoClaimsPrincipalWasReturnedFromCommandBus_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut(hasClaimsPrincipal: false);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoClaimsPrincipalWasReturnedFromCommandBus_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut(hasClaimsPrincipal: false);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoClaimsPrincipalWasReturnedFromCommandBus_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext, hasClaimsPrincipal: false);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoClaimsPrincipalWasReturnedFromCommandBus_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasClaimsPrincipal: false);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoClaimsPrincipalWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResult()
        {
            Controller sut = CreateSut(hasClaimsPrincipal: false);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoClaimsPrincipalWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut(hasClaimsPrincipal: false);

            string authorization = CreateAuthorization();
            UnauthorizedObjectResult result = (UnauthorizedObjectResult) await sut.AcquireToken(GrantType, authorization: authorization);

            result.AssertExpectedErrorResponseModel("invalid_client", ErrorDescriptionResolver.Resolve(ErrorCode.UnableAuthenticateClient), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithoutClaimsIdentityWasReturnedFromCommandBus_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithoutClaimsIdentityWasReturnedFromCommandBus_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithoutClaimsIdentityWasReturnedFromCommandBus_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithoutClaimsIdentityWasReturnedFromCommandBus_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            Controller sut = CreateSut(httpContext: httpContext, claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithoutClaimsIdentityWasReturnedFromCommandBus_ReturnsNotNull()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithoutClaimsIdentityWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResult()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithoutClaimsIdentityWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            UnauthorizedObjectResult result = (UnauthorizedObjectResult) await sut.AcquireToken(GrantType, authorization: authorization);

            result.AssertExpectedErrorResponseModel("invalid_client", ErrorDescriptionResolver.Resolve(ErrorCode.UnableAuthenticateClient), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithNonAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithNonAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithNonAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithNonAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(httpContext: httpContext, claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithNonAuthenticatedClaimsIdentityWasReturnedFromCommandBus_ReturnsNotNull()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithNonAuthenticatedClaimsIdentityWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResult()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithNonAuthenticatedClaimsIdentityWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            UnauthorizedObjectResult result = (UnauthorizedObjectResult) await sut.AcquireToken(GrantType, authorization: authorization);

            result.AssertExpectedErrorResponseModel("invalid_client", ErrorDescriptionResolver.Resolve(ErrorCode.UnableAuthenticateClient), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertPublishAsyncWasCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsNotNull<IGenerateTokenCommand>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertSignInAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            Controller sut = CreateSut(httpContext: httpContext, claimsPrincipal: claimsPrincipal);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<ClaimsPrincipal>(value => value != null && value == claimsPrincipal),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoTokenWasReturnedFromCommandBus_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext, hasToken: false);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoTokenWasReturnedFromCommandBus_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasToken: false);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoTokenWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResult()
        {
            Controller sut = CreateSut(hasToken: false);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoTokenWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut(hasToken: false);

            string authorization = CreateAuthorization();
            UnauthorizedObjectResult result = (UnauthorizedObjectResult) await sut.AcquireToken(GrantType, authorization: authorization);

            result.AssertExpectedErrorResponseModel("unauthorized_client", ErrorDescriptionResolver.Resolve(ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedClient), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasReturnedFromCommandBus_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasReturnedFromCommandBus_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasReturnedFromCommandBus_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasReturnedFromCommandBus_ReturnsOkObjectResultWithExpectedAccessTokenModel()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            DateTimeOffset expires = DateTimeOffset.Now.AddSeconds(_random.Next(30 * 60, 60 * 60));
            IToken token = _fixture.BuildTokenMock(tokenType: tokenType, accessToken: accessToken, expires.LocalDateTime).Object;
            Controller sut = CreateSut(token: token);

            string authorization = CreateAuthorization();
            OkObjectResult result = (OkObjectResult) await sut.AcquireToken(GrantType, authorization: authorization);

            result.AssertExpectedAccessTokenModel(tokenType, accessToken, expires.UtcDateTime);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetValidationExceptionIsThrown_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            IntranetValidationException intranetValidationException = CreateIntranetValidationException();
            Controller sut = CreateSut(exception: intranetValidationException);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetValidationExceptionIsThrown_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            IntranetValidationException intranetValidationException = CreateIntranetValidationException();
            Controller sut = CreateSut(httpContext: httpContext, exception: intranetValidationException);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetValidationExceptionIsThrown_ReturnsNotNull()
        {
            IntranetValidationException intranetValidationException = CreateIntranetValidationException();
            Controller sut = CreateSut(exception: intranetValidationException);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetValidationExceptionIsThrown_ReturnsBadRequestObjectResult()
        {
            IntranetValidationException intranetValidationException = CreateIntranetValidationException();
            Controller sut = CreateSut(exception: intranetValidationException);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetValidationExceptionIsThrown_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            string message = _fixture.Create<string>();
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(message: message);
            Controller sut = CreateSut(exception: intranetValidationException);

            string authorization = CreateAuthorization();
            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, authorization: authorization);

            result.AssertExpectedErrorResponseModel("invalid_request", message, null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetBusinessExceptionIsThrown_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.UnableAuthenticateClient);
            Controller sut = CreateSut(exception: intranetBusinessException);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetBusinessExceptionIsThrown_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.UnableAuthenticateClient);
            Controller sut = CreateSut(httpContext: httpContext, exception: intranetBusinessException);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetBusinessExceptionIsThrown_ReturnsNotNull()
        {
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.UnableAuthenticateClient);
            Controller sut = CreateSut(exception: intranetBusinessException);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetBusinessExceptionIsThrown_ReturnsUnauthorizedObjectResult()
        {
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.UnableAuthenticateClient);
            Controller sut = CreateSut(exception: intranetBusinessException);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetBusinessExceptionIsThrown_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            string message = _fixture.Create<string>();
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.UnableAuthenticateClient, message: message);
            Controller sut = CreateSut(exception: intranetBusinessException);

            string authorization = CreateAuthorization();
            UnauthorizedObjectResult result = (UnauthorizedObjectResult) await sut.AcquireToken(GrantType, authorization: authorization);

            result.AssertExpectedErrorResponseModel("invalid_client", message, null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetExceptionBaseIsThrown_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            IntranetExceptionBase intranetExceptionBase = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: intranetExceptionBase);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetExceptionBaseIsThrown_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            IntranetExceptionBase intranetExceptionBase = CreateIntranetExceptionBase();
            Controller sut = CreateSut(httpContext: httpContext, exception: intranetExceptionBase);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetExceptionBaseIsThrown_ReturnsNotNull()
        {
            IntranetExceptionBase intranetExceptionBase = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: intranetExceptionBase);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetExceptionBaseIsThrown_ReturnsObjectResult()
        {
            IntranetExceptionBase intranetExceptionBase = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: intranetExceptionBase);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.TypeOf<ObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetExceptionBaseIsThrown_ReturnsObjectResultWithStatusCodeEqualToStatus500InternalServerError()
        {
            IntranetExceptionBase intranetExceptionBase = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: intranetExceptionBase);

            string authorization = CreateAuthorization();
            ObjectResult result = (ObjectResult) await sut.AcquireToken(GrantType, authorization: authorization);

            result.AssertExpectedStatusCode(StatusCodes.Status500InternalServerError);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetExceptionBaseIsThrown_ReturnsObjectResultWithExpectedErrorResponseModel()
        {
            IntranetExceptionBase intranetExceptionBase = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: intranetExceptionBase);

            string authorization = CreateAuthorization();
            ObjectResult result = (ObjectResult) await sut.AcquireToken(GrantType, authorization: authorization);

            result.AssertExpectedErrorResponseModel("server_error", ErrorDescriptionResolver.Resolve(ErrorCode.UnableAuthenticateClient), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenExceptionIsThrown_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenExceptionIsThrown_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Exception exception = CreateException();
            Controller sut = CreateSut(httpContext: httpContext, exception: exception);

            string authorization = CreateAuthorization();
            await sut.AcquireToken(GrantType, authorization: authorization);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenExceptionIsThrown_ReturnsNotNull()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenExceptionIsThrown_ReturnsObjectResult()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            string authorization = CreateAuthorization();
            IActionResult result = await sut.AcquireToken(GrantType, authorization: authorization);

            Assert.That(result, Is.TypeOf<ObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenExceptionIsThrown_ReturnsObjectResultWithStatusCodeEqualToStatus500InternalServerError()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            string authorization = CreateAuthorization();
            ObjectResult result = (ObjectResult) await sut.AcquireToken(GrantType, authorization: authorization);

            result.AssertExpectedStatusCode(StatusCodes.Status500InternalServerError);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenExceptionIsThrown_ReturnsObjectResultWithExpectedErrorResponseModel()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            string authorization = CreateAuthorization();
            ObjectResult result = (ObjectResult) await sut.AcquireToken(GrantType, authorization: authorization);

            result.AssertExpectedErrorResponseModel("server_error", ErrorDescriptionResolver.Resolve(ErrorCode.UnableAuthenticateClient), null, null);
        }

        private Controller CreateSut(HttpContext httpContext = null, bool hasClaimsPrincipal = true, ClaimsPrincipal claimsPrincipal = null, bool hasToken = true, IToken token = null, Exception exception = null)
        {
            if (exception == null)
            {
                _commandBusMock.Setup(m => m.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateClientSecretCommand>()))
                    .Returns(Task.FromResult(hasClaimsPrincipal ? claimsPrincipal ?? CreateClaimsPrincipal() : null));
            }
            else
            {
                _commandBusMock.Setup(m => m.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateClientSecretCommand>()))
                    .Throws(exception);
            }

            return CreateSut(httpContext, hasToken, token);
        }
    }
}