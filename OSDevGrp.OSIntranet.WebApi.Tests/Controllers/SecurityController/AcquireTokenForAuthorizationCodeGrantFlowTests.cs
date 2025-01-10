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
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;
using OSDevGrp.OSIntranet.WebApi.Security;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class AcquireTokenForAuthorizationCodeGrantFlowTests : AcquireTokenTestBase
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<IDataProtector> _dataProtectorMock;
        private Mock<TimeProvider> _timeProviderMock;
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        #region Properties

        protected override Mock<ICommandBus> CommandBusMock => _commandBusMock;

        protected override Mock<IQueryBus> QueryBusMock => _queryBusMock;

        protected override Mock<IDataProtectionProvider> DataProtectionProviderMock => _dataProtectionProviderMock;

        protected override Mock<IDataProtector> DataProtectorMock => _dataProtectorMock;

        protected override Mock<TimeProvider> TimeProviderMock => _timeProviderMock;

        protected override Mock<IAuthenticationService> AuthenticationServiceMock => _authenticationServiceMock;

        protected override Fixture Fixture => _fixture;

        private string GrantType => "authorization_code";

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _dataProtectorMock = new Mock<IDataProtector>();
            _timeProviderMock = new Mock<TimeProvider>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsNull_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: null, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsNull_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: null, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsNull_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: null, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsNull_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: null, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsNull_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: null, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: null, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsNull_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: null, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsNull_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: null, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "code"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsEmpty_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: string.Empty, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsEmpty_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: string.Empty, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsEmpty_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: string.Empty, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsEmpty_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: string.Empty, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsEmpty_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: string.Empty, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: string.Empty, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsEmpty_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: string.Empty, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsEmpty_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: string.Empty, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "code"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsWhiteSpace_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: " ", clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsWhiteSpace_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: " ", clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsWhiteSpace_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: " ", clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsWhiteSpace_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: " ", clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsWhiteSpace_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: " ", clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: " ", clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsWhiteSpace_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: " ", clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenCodeIsWhiteSpace_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: " ", clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "code"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsNull_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: null, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsNull_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: null, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsNull_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: null, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsNull_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: null, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsNull_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: null, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: null, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsNull_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: null, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsNull_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: null, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "client_id"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsEmpty_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: string.Empty, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsEmpty_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: string.Empty, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsEmpty_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: string.Empty, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsEmpty_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: string.Empty, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsEmpty_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: string.Empty, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: string.Empty, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsEmpty_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: string.Empty, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsEmpty_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: string.Empty, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "client_id"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsWhiteSpace_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: " ", clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsWhiteSpace_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: " ", clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsWhiteSpace_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: " ", clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsWhiteSpace_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: " ", clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsWhiteSpace_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: " ", clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: " ", clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsWhiteSpace_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: " ", clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientIdIsWhiteSpace_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: " ", clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "client_id"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsNull_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: null, redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsNull_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: null, redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsNull_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: null, redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsNull_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: null, redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsNull_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: null, redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: null, redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsNull_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: null, redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsNull_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: null, redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "client_secret"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsEmpty_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: string.Empty, redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsEmpty_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: string.Empty, redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsEmpty_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: string.Empty, redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsEmpty_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: string.Empty, redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsEmpty_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: string.Empty, redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: string.Empty, redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsEmpty_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: string.Empty, redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsEmpty_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: string.Empty, redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "client_secret"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsWhiteSpace_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: " ", redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsWhiteSpace_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: " ", redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsWhiteSpace_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: " ", redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsWhiteSpace_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: " ", redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsWhiteSpace_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: " ", redirectUri: _fixture.CreateEndpointString());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: " ", redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsWhiteSpace_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: " ", redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClientSecretIsWhiteSpace_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: " ", redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "client_secret"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNull_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: null);

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNull_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: null);

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNull_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: null);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNull_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: null);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNull_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: null);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNull_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: null);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNull_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: null);

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "redirect_uri"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsEmpty_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: string.Empty);

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsEmpty_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: string.Empty);

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsEmpty_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: string.Empty);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsEmpty_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: string.Empty);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsEmpty_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: string.Empty);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsEmpty_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: string.Empty);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsEmpty_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: string.Empty);

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "redirect_uri"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsWhiteSpace_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: " ");

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsWhiteSpace_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: " ");

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsWhiteSpace_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: " ");

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsWhiteSpace_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: " ");

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsWhiteSpace_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: " ");

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: " ");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsWhiteSpace_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: " ");

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsWhiteSpace_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: " ");

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "redirect_uri"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNonAbsoluteUri_AssertPublishAsyncWasNotCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.Create<string>());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNonAbsoluteUri_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.Create<string>());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNonAbsoluteUri_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.Create<string>());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNonAbsoluteUri_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.Create<string>());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNonAbsoluteUri_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.Create<string>());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, Schemes.Internal) == 0),
                    It.Is<AuthenticationProperties>(value => value == null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNonAbsoluteUri_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNonAbsoluteUri_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenRedirectUriIsNonAbsoluteUri_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.Create<string>());

            result.AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueShouldBeKnown, "redirect_uri"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNeededValuesAreValid_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateAuthorizationCodeCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNeededValuesAreValid_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateAuthorizationCodeCommandWhereAuthorizationCodeIsEqualToCodeFromArguments()
        {
            Controller sut = CreateSut();

            string code = _fixture.Create<string>();
            await sut.AcquireToken(GrantType, code: code, clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.Is<IAuthenticateAuthorizationCodeCommand>(value => value != null && string.IsNullOrWhiteSpace(value.AuthorizationCode) == false && string.CompareOrdinal(value.AuthorizationCode, code) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNeededValuesAreValid_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateAuthorizationCodeCommandWhereClientIdIsEqualToClientIdFromArguments()
        {
            Controller sut = CreateSut();

            string clientId = _fixture.Create<string>();
            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: clientId, clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.Is<IAuthenticateAuthorizationCodeCommand>(value => value != null && string.IsNullOrWhiteSpace(value.ClientId) == false && string.CompareOrdinal(value.ClientId, clientId) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNeededValuesAreValid_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateAuthorizationCodeCommandWhereClientSecretIsEqualToClientSecretFromArguments()
        {
            Controller sut = CreateSut();

            string clientSecret = _fixture.Create<string>();
            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: clientSecret, redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.Is<IAuthenticateAuthorizationCodeCommand>(value => value != null && string.IsNullOrWhiteSpace(value.ClientSecret) == false && string.CompareOrdinal(value.ClientSecret, clientSecret) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNeededValuesAreValid_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateAuthorizationCodeCommandWhereRedirectUriIsEqualToRedirectUriFromArguments()
        {
            Controller sut = CreateSut();

            string redirectUri = _fixture.CreateEndpointString();
            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: redirectUri);

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.Is<IAuthenticateAuthorizationCodeCommand>(value => value != null && value.RedirectUri != null && string.CompareOrdinal(value.RedirectUri.AbsoluteUri, redirectUri) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNeededValuesAreValid_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateAuthorizationCodeCommandWhereOnIdTokenResolvedIsNotNull()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.Is<IAuthenticateAuthorizationCodeCommand>(value => value != null && value.OnIdTokenResolved != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNeededValuesAreValid_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateAuthorizationCodeCommandWhereClaimsIsNotNull()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.Is<IAuthenticateAuthorizationCodeCommand>(value => value != null && value.Claims != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNeededValuesAreValid_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateAuthorizationCodeCommandWhereClaimsIsEmpty()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.Is<IAuthenticateAuthorizationCodeCommand>(value => value != null && value.Claims != null && value.Claims.Any() == false)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNeededValuesAreValid_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateAuthorizationCodeCommandWhereAuthenticationSessionItemsIsNotNull()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.Is<IAuthenticateAuthorizationCodeCommand>(value => value != null && value.AuthenticationSessionItems != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNeededValuesAreValid_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateAuthorizationCodeCommandWhereAuthenticationSessionItemsIsEmpty()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.Is<IAuthenticateAuthorizationCodeCommand>(value => value != null && value.AuthenticationSessionItems != null && value.AuthenticationSessionItems.Any() == false)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNeededValuesAreValid_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateAuthorizationCodeCommandWhereAuthenticationTypeIsEqualToInternalScheme()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.Is<IAuthenticateAuthorizationCodeCommand>(value => value != null && string.IsNullOrWhiteSpace(value.AuthenticationType) == false && string.CompareOrdinal(value.AuthenticationType, Schemes.Internal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNeededValuesAreValid_AssertPublishAsyncWasCalledOnCommandBusWithAuthenticateAuthorizationCodeCommandWhereProtectorIsNotNull()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.Is<IAuthenticateAuthorizationCodeCommand>(value => value != null && value.Protector != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoClaimsPrincipalWasReturnedFromCommandBus_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut(hasClaimsPrincipal: false);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoClaimsPrincipalWasReturnedFromCommandBus_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut(hasClaimsPrincipal: false);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoClaimsPrincipalWasReturnedFromCommandBus_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut(hasClaimsPrincipal: false);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoClaimsPrincipalWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResult()
        {
            Controller sut = CreateSut(hasClaimsPrincipal: false);

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoClaimsPrincipalWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut(hasClaimsPrincipal: false);

            UnauthorizedObjectResult result = (UnauthorizedObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_client", ErrorDescriptionResolver.Resolve(ErrorCode.UnableAuthenticateClient), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithoutClaimsIdentityWasReturnedFromCommandBus_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithoutClaimsIdentityWasReturnedFromCommandBus_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithoutClaimsIdentityWasReturnedFromCommandBus_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithoutClaimsIdentityWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResult()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithoutClaimsIdentityWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaimsIdentity: false);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            UnauthorizedObjectResult result = (UnauthorizedObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_client", ErrorDescriptionResolver.Resolve(ErrorCode.UnableAuthenticateClient), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithNonAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertPublishAsyncWasNotCalledOnCommandBusWithGenerateTokenCommand()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithNonAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithNonAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithNonAuthenticatedClaimsIdentityWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResult()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithNonAuthenticatedClaimsIdentityWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(claimsIdentity: claimsIdentity);
            Controller sut = CreateSut(claimsPrincipal: claimsPrincipal);

            UnauthorizedObjectResult result = (UnauthorizedObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_client", ErrorDescriptionResolver.Resolve(ErrorCode.UnableAuthenticateClient), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertPublishAsyncWasCalledOnCommandBusWithGenerateTokenCommand()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsNotNull<IGenerateTokenCommand>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenClaimsPrincipalWithAuthenticatedClaimsIdentityWasReturnedFromCommandBus_AssertSignInAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            Controller sut = CreateSut(httpContext: httpContext, claimsPrincipal: claimsPrincipal);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoTokenWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResult()
        {
            Controller sut = CreateSut(hasToken: false);

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenNoTokenWasReturnedFromCommandBus_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut(hasToken: false);

            UnauthorizedObjectResult result = (UnauthorizedObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("unauthorized_client", ErrorDescriptionResolver.Resolve(ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedClient), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasReturnedFromCommandBus_AssertSignOutAsyncWasCalledOnAuthenticationService()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasReturnedFromCommandBus_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasReturnedFromCommandBus_ReturnsOkObjectResultWithExpectedAccessTokenModel()
        {
            string accessTokenForIdToken = _fixture.Create<string>();
            IToken idToken = _fixture.BuildTokenMock(accessToken: accessTokenForIdToken).Object;
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            DateTimeOffset expires = DateTimeOffset.Now.AddSeconds(_random.Next(30 * 60, 60 * 60));
            IToken token = _fixture.BuildTokenMock(tokenType: tokenType, accessToken: accessToken, expires.LocalDateTime).Object;
            Controller sut = CreateSut(idToken: idToken, token: token);

            OkObjectResult result = (OkObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedAccessTokenModel(tokenType, accessToken, accessTokenForIdToken, expires.UtcDateTime);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetValidationExceptionIsThrown_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            IntranetValidationException intranetValidationException = CreateIntranetValidationException();
            Controller sut = CreateSut(exception: intranetValidationException);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetValidationExceptionIsThrown_ReturnsBadRequestObjectResult()
        {
            IntranetValidationException intranetValidationException = CreateIntranetValidationException();
            Controller sut = CreateSut(exception: intranetValidationException);

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetValidationExceptionIsThrown_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            string message = _fixture.Create<string>();
            IntranetValidationException intranetValidationException = CreateIntranetValidationException(message: message);
            Controller sut = CreateSut(exception: intranetValidationException);

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_request", message, null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetBusinessExceptionIsThrown_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.UnableAuthenticateClient);
            Controller sut = CreateSut(exception: intranetBusinessException);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetBusinessExceptionIsThrown_ReturnsUnauthorizedObjectResult()
        {
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.UnableAuthenticateClient);
            Controller sut = CreateSut(exception: intranetBusinessException);

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetBusinessExceptionIsThrown_ReturnsUnauthorizedObjectResultWithExpectedErrorResponseModel()
        {
            string message = _fixture.Create<string>();
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.UnableAuthenticateClient, message: message);
            Controller sut = CreateSut(exception: intranetBusinessException);

            UnauthorizedObjectResult result = (UnauthorizedObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("invalid_client", message, null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetExceptionBaseIsThrown_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            IntranetExceptionBase intranetExceptionBase = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: intranetExceptionBase);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetExceptionBaseIsThrown_ReturnsObjectResult()
        {
            IntranetExceptionBase intranetExceptionBase = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: intranetExceptionBase);

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<ObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetExceptionBaseIsThrown_ReturnsObjectResultWithStatusCodeEqualToStatus500InternalServerError()
        {
            IntranetExceptionBase intranetExceptionBase = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: intranetExceptionBase);

            ObjectResult result = (ObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedStatusCode(StatusCodes.Status500InternalServerError);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenIntranetExceptionBaseIsThrown_ReturnsObjectResultWithExpectedErrorResponseModel()
        {
            IntranetExceptionBase intranetExceptionBase = CreateIntranetExceptionBase();
            Controller sut = CreateSut(exception: intranetExceptionBase);

            ObjectResult result = (ObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("server_error", ErrorDescriptionResolver.Resolve(ErrorCode.UnableAuthenticateClient), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenExceptionIsThrown_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

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

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenExceptionIsThrown_ReturnsObjectResult()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            IActionResult result = await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            Assert.That(result, Is.TypeOf<ObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenExceptionIsThrown_ReturnsObjectResultWithStatusCodeEqualToStatus500InternalServerError()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            ObjectResult result = (ObjectResult) await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedStatusCode(StatusCodes.Status500InternalServerError);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenExceptionIsThrown_ReturnsObjectResultWithExpectedErrorResponseModel()
        {
            Exception exception = CreateException();
            Controller sut = CreateSut(exception: exception);

            ObjectResult result = (ObjectResult)await sut.AcquireToken(GrantType, code: _fixture.Create<string>(), clientId: _fixture.Create<string>(), clientSecret: _fixture.Create<string>(), redirectUri: _fixture.CreateEndpointString());

            result.AssertExpectedErrorResponseModel("server_error", ErrorDescriptionResolver.Resolve(ErrorCode.UnableAuthenticateClient), null, null);
        }

        private Controller CreateSut(HttpContext httpContext = null, bool hasClaimsPrincipal = true, ClaimsPrincipal claimsPrincipal = null, IToken idToken = null, bool hasToken = true, IToken token = null, Exception exception = null)
        {
            if (exception == null)
            {
                _commandBusMock.Setup(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()))
                    .Callback<IAuthenticateAuthorizationCodeCommand>(authenticateAuthorizationCodeCommand =>
                    {
                        Assert.That(authenticateAuthorizationCodeCommand, Is.Not.Null);
                        Assert.That(authenticateAuthorizationCodeCommand.OnIdTokenResolved, Is.Not.Null);

                        authenticateAuthorizationCodeCommand.OnIdTokenResolved(idToken ?? _fixture.BuildTokenMock().Object);
                    })
                    .Returns(Task.FromResult(hasClaimsPrincipal ? claimsPrincipal ?? CreateClaimsPrincipal() : null));
            }
            else
            {
                _commandBusMock.Setup(m => m.PublishAsync<IAuthenticateAuthorizationCodeCommand, ClaimsPrincipal>(It.IsAny<IAuthenticateAuthorizationCodeCommand>()))
                    .Throws(exception);
            }

            return CreateSut(httpContext, hasToken, token);
        }
    }
}