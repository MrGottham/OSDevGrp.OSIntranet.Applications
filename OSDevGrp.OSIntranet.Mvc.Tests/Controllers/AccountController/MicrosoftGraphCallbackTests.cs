using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using System;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.AccountController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountController
{
    [TestFixture]
    public class MicrosoftGraphCallbackTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
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
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenCodeIsNull_AssertAcquireTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.MicrosoftGraphCallback(null, Guid.NewGuid().ToString());

            _tokenHelperFactoryMock.Verify(m => m.AcquireTokenAsync(
                    It.IsAny<TokenType>(),
                    It.IsAny<HttpContext>(),
                    It.IsAny<object[]>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenCodeIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(null, Guid.NewGuid().ToString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenCodeIsNull_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(null, Guid.NewGuid().ToString());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenCodeIsEmpty_AssertAcquireTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.MicrosoftGraphCallback(string.Empty, Guid.NewGuid().ToString());

            _tokenHelperFactoryMock.Verify(m => m.AcquireTokenAsync(
                    It.IsAny<TokenType>(),
                    It.IsAny<HttpContext>(),
                    It.IsAny<object[]>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenCodeIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(string.Empty, Guid.NewGuid().ToString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenCodeIsEmpty_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(string.Empty, Guid.NewGuid().ToString());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenCodeIsWhiteSpace_AssertAcquireTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.MicrosoftGraphCallback(" ", Guid.NewGuid().ToString());

            _tokenHelperFactoryMock.Verify(m => m.AcquireTokenAsync(
                    It.IsAny<TokenType>(),
                    It.IsAny<HttpContext>(),
                    It.IsAny<object[]>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenCodeIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(" ", Guid.NewGuid().ToString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenCodeIsWhiteSpace_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(" ", Guid.NewGuid().ToString());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsNull_AssertAcquireTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.MicrosoftGraphCallback(_fixture.Create<string>(), null);

            _tokenHelperFactoryMock.Verify(m => m.AcquireTokenAsync(
                    It.IsAny<TokenType>(),
                    It.IsAny<HttpContext>(),
                    It.IsAny<object[]>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(_fixture.Create<string>(), null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsNull_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(_fixture.Create<string>(), null);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsEmpty_AssertAcquireTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.MicrosoftGraphCallback(_fixture.Create<string>(), string.Empty);

            _tokenHelperFactoryMock.Verify(m => m.AcquireTokenAsync(
                    It.IsAny<TokenType>(),
                    It.IsAny<HttpContext>(),
                    It.IsAny<object[]>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(_fixture.Create<string>(), string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsEmpty_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(_fixture.Create<string>(), string.Empty);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsWhiteSpace_AssertAcquireTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.MicrosoftGraphCallback(_fixture.Create<string>(), " ");

            _tokenHelperFactoryMock.Verify(m => m.AcquireTokenAsync(
                    It.IsAny<TokenType>(),
                    It.IsAny<HttpContext>(),
                    It.IsAny<object[]>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(_fixture.Create<string>(), " ");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsWhiteSpace_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(_fixture.Create<string>(), " ");

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsNonGuid_AssertAcquireTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.MicrosoftGraphCallback(_fixture.Create<string>(), $"{_fixture.Create<string>()} {_fixture.Create<string>()}");

            _tokenHelperFactoryMock.Verify(m => m.AcquireTokenAsync(
                    It.IsAny<TokenType>(),
                    It.IsAny<HttpContext>(),
                    It.IsAny<object[]>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsNonGuid_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(_fixture.Create<string>(), $"{_fixture.Create<string>()} {_fixture.Create<string>()}");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsNonGuid_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(_fixture.Create<string>(), $"{_fixture.Create<string>()} {_fixture.Create<string>()}");

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsGuid_AssertAcquireTokenAsyncWasCalledOnTokenHelperFactory()
        {
            HttpContext httpContext = CreateHttpContext();
            Controller sut = CreateSut(httpContext: httpContext);

            string code = _fixture.Create<string>();
            Guid state = Guid.NewGuid();
            await sut.MicrosoftGraphCallback(code, state.ToString());

            _tokenHelperFactoryMock.Verify(m => m.AcquireTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.Is<object[]>(value =>
                        value != null && value.Length == 2 && (Guid) value[0] == state &&
                        string.Compare((string) value[1], code) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsGuid_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.MicrosoftGraphCallback(_fixture.Create<string>(), Guid.NewGuid().ToString());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsGuid_ReturnsActionResultFromAcquireTokenAsyncOnTokenHelperFactory()
        {
            IActionResult actionResult = new Mock<IActionResult>().Object;
            Controller sut = CreateSut(actionResult: actionResult);

            IActionResult result = await sut.MicrosoftGraphCallback(_fixture.Create<string>(), Guid.NewGuid().ToString());

            Assert.That(result, Is.EqualTo(actionResult));
        }

        private Controller CreateSut(HttpContext httpContext = null, IActionResult actionResult = null)
        {
            _tokenHelperFactoryMock.Setup(m => m.AcquireTokenAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<object[]>()))
                .Returns(Task.Run(() => actionResult ?? new Mock<IActionResult>().Object));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _trustedDomainResolverMock.Object, _tokenHelperFactoryMock.Object, _dataProtectionProviderMock.Object)
            {
                ControllerContext =
                {
                    HttpContext = httpContext ?? CreateHttpContext()
                }
            };
        }

        private static HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext();
        }
    }
}