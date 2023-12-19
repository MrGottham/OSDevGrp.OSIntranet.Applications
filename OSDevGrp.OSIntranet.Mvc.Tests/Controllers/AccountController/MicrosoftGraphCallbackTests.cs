using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
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
        private Mock<ITrustedDomainHelper> _trustedDomainHelperMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
		private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _trustedDomainHelperMock = new Mock<ITrustedDomainHelper>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void MicrosoftGraphCallback_WhenCodeIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MicrosoftGraphCallback(null, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("code"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void MicrosoftGraphCallback_WhenCodeIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MicrosoftGraphCallback(string.Empty, _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("code"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void MicrosoftGraphCallback_WhenCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MicrosoftGraphCallback(" ", _fixture.Create<string>()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("code"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void MicrosoftGraphCallback_WhenStateIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MicrosoftGraphCallback(_fixture.Create<string>(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("state"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void MicrosoftGraphCallback_WhenStateIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MicrosoftGraphCallback(_fixture.Create<string>(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("state"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void MicrosoftGraphCallback_WhenStateIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MicrosoftGraphCallback(_fixture.Create<string>(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("state"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsNotGuid_AssertAcquireTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            string state = $"{_fixture.Create<string>()} {_fixture.Create<string>()}";
            await sut.MicrosoftGraphCallback(_fixture.Create<string>(), state);

            _tokenHelperFactoryMock.Verify(m => m.AcquireTokenAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<object[]>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsNotGuid_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        string state = $"{_fixture.Create<string>()} {_fixture.Create<string>()}";
	        IActionResult result = await sut.MicrosoftGraphCallback(_fixture.Create<string>(), state);

	        Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsNotGuid_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            string state = $"{_fixture.Create<string>()} {_fixture.Create<string>()}";
            IActionResult result = await sut.MicrosoftGraphCallback(_fixture.Create<string>(), state);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsGuid_AssertAcquireTokenAsyncWasCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            Guid stateIdentifier = Guid.NewGuid();
            string code = $"{_fixture.Create<string>()} {_fixture.Create<string>()}";
            string state = stateIdentifier.ToString();
            await sut.MicrosoftGraphCallback(code, state);

            _tokenHelperFactoryMock.Verify(m => m.AcquireTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.Is<object[]>(value => value != null && value.Length == 2 && (Guid) value[0] == stateIdentifier && string.CompareOrdinal((string) value[1], state) == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MicrosoftGraphCallback_WhenStateIsGuid_ReturnActionResultFromAcquireTokenAsyncOnTokenHelperFactory()
        {
            IActionResult actionResult = new Mock<IActionResult>().Object;
            Controller sut = CreateSut(actionResult);

            string code = $"{_fixture.Create<string>()} {_fixture.Create<string>()}";
            string state = Guid.NewGuid().ToString();
            IActionResult result = await sut.MicrosoftGraphCallback(code, state);

            Assert.That(result, Is.EqualTo(actionResult));
        }

        private Controller CreateSut(IActionResult actionResult = null)
        {
            _tokenHelperFactoryMock.Setup(m => m.AcquireTokenAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<object[]>()))
                .Returns(Task.Run(() => actionResult ?? new Mock<IActionResult>().Object));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _trustedDomainHelperMock.Object, _tokenHelperFactoryMock.Object, _dataProtectionProviderMock.Object);
        }
    }
}