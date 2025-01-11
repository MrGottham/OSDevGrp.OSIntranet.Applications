using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class AcquireTokenTests : AcquireTokenTestBase
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<IDataProtector> _dataProtectorMock;
        private Mock<TimeProvider> _timeProviderMock;
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private Fixture _fixture;

        #endregion

        #region Properties

        protected override Mock<ICommandBus> CommandBusMock => _commandBusMock;

        protected override Mock<IQueryBus> QueryBusMock => _queryBusMock;

        protected override Mock<IDataProtectionProvider> DataProtectionProviderMock => _dataProtectionProviderMock;

        protected override Mock<IDataProtector> DataProtectorMock => _dataProtectorMock;

        protected override Mock<TimeProvider> TimeProviderMock => _timeProviderMock;

        protected override Mock<IAuthenticationService> AuthenticationServiceMock => _authenticationServiceMock;

        protected override Fixture Fixture => _fixture;

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
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsNull_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(null);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsNull_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(null);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsNull_AssertSignOutAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(null);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsNull_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(null);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsNull_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(null);

            result.AssertExpectedErrorResponseModel("unsupported_grant_type", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "grant_type"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsEmpty_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(string.Empty);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsEmpty_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(string.Empty);

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsEmpty_AssertSignOutAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(string.Empty);

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsEmpty_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(string.Empty);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsEmpty_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(string.Empty);

            result.AssertExpectedErrorResponseModel("unsupported_grant_type", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "grant_type"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsWhiteSpace_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(" ");

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsWhiteSpace_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(" ");

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsWhiteSpace_AssertSignOutAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(" ");

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(" ");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsWhiteSpace_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(" ");

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsWhiteSpace_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(" ");

            result.AssertExpectedErrorResponseModel("unsupported_grant_type", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "grant_type"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeDoesNotMatchPatternForGrantType_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(_fixture.Create<string>());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeDoesNotMatchPatternForGrantType_AssertSignInAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(_fixture.Create<string>());

            _authenticationServiceMock.Verify(m => m.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeDoesNotMatchPatternForGrantType_AssertSignOutAsyncWasNotCalledOnAuthenticationService()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(_fixture.Create<string>());

            _authenticationServiceMock.Verify(m => m.SignOutAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeDoesNotMatchPatternForGrantType_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeDoesNotMatchPatternForGrantType_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcquireToken(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeDoesNotMatchPatternForGrantType_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            BadRequestObjectResult result = (BadRequestObjectResult) await sut.AcquireToken(_fixture.Create<string>());
            result.AssertExpectedErrorResponseModel("unsupported_grant_type", ErrorDescriptionResolver.Resolve(ErrorCode.ValueShouldMatchPattern, "grant_type", GrantTypePattern), null, null);
        }
    }
}