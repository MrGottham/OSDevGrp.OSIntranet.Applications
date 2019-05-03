using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers.AcquireTokenForMicrosoftGraphCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.AcquireTokenForMicrosoftGraphCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IMicrosoftGraphRepository> _microsoftGraphRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _microsoftGraphRepositoryMock = new Mock<IMicrosoftGraphRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
        {
            CommandHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("command"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertRedirectUriWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IAcquireTokenForMicrosoftGraphCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.RedirectUri, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCodeWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IAcquireTokenForMicrosoftGraphCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.Code, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertAcquireTokenAsyncWasCalledOnMicrosoftGraphRepository()
        {
            CommandHandler sut = CreateSut();

            Uri redirectUri = CreateUri();
            string code = _fixture.Create<string>();
            IAcquireTokenForMicrosoftGraphCommand command = CreateCommandMock(redirectUri, code).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.AcquireTokenAsync(It.Is<Uri>(value => value == redirectUri), It.Is<string>(value => string.Compare(value, code, StringComparison.Ordinal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_ReturnRefreshableTokenFromMicrosoftGraphRepository()
        {
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock().Object;
            CommandHandler sut = CreateSut(refreshableToken);

            IAcquireTokenForMicrosoftGraphCommand command = CreateCommandMock().Object;
            IRefreshableToken result = await sut.ExecuteAsync(command);

            Assert.That(result, Is.EqualTo(refreshableToken));
        }

        private CommandHandler CreateSut(IRefreshableToken refreshableToken = null)
        {
            _microsoftGraphRepositoryMock.Setup(m => m.AcquireTokenAsync(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(Task.Run(() => refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object));

            return new CommandHandler(_microsoftGraphRepositoryMock.Object);
        }

        private Mock<IAcquireTokenForMicrosoftGraphCommand> CreateCommandMock(Uri redirectUri = null, string code = null)
        {
            Mock<IAcquireTokenForMicrosoftGraphCommand> commandMock = new Mock<IAcquireTokenForMicrosoftGraphCommand>();
            commandMock.Setup(m => m.RedirectUri)
                .Returns(redirectUri ?? CreateUri());
            commandMock.Setup(m => m.Code)
                .Returns(code ?? _fixture.Create<string>());
            return commandMock;
        }

        private Uri CreateUri()
        {
            return new Uri($"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}");
        }
    }
}
