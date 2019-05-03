using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers.RefreshTokenForMicrosoftGraphCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.RefreshTokenForMicrosoftGraphCommandHandler
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

            Mock<IRefreshTokenForMicrosoftGraphCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.RedirectUri, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertRefreshableTokenWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IRefreshTokenForMicrosoftGraphCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.RefreshableToken, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertRefreshTokenAsyncWasCalledOnMicrosoftGraphRepository()
        {
            CommandHandler sut = CreateSut();

            Uri redirectUri = CreateUri();
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock().Object;
            IRefreshTokenForMicrosoftGraphCommand command = CreateCommandMock(redirectUri, refreshableToken).Object;
            await sut.ExecuteAsync(command);

            _microsoftGraphRepositoryMock.Verify(m => m.RefreshTokenAsync(It.Is<Uri>(value => value == redirectUri), It.Is<IRefreshableToken>(value => value == refreshableToken)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_ReturnRefreshableTokenFromMicrosoftGraphRepository()
        {
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock().Object;
            CommandHandler sut = CreateSut(refreshableToken);

            IRefreshTokenForMicrosoftGraphCommand command = CreateCommandMock().Object;
            IRefreshableToken result = await sut.ExecuteAsync(command);

            Assert.That(result, Is.EqualTo(refreshableToken));
        }

        private CommandHandler CreateSut(IRefreshableToken refreshableToken = null)
        {
            _microsoftGraphRepositoryMock.Setup(m => m.RefreshTokenAsync(It.IsAny<Uri>(), It.IsAny<IRefreshableToken>()))
                .Returns(Task.Run(() => refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object));

            return new CommandHandler(_microsoftGraphRepositoryMock.Object);
        }

        private Mock<IRefreshTokenForMicrosoftGraphCommand> CreateCommandMock(Uri redirectUri = null, IRefreshableToken refreshableToken = null)
        {
            Mock<IRefreshTokenForMicrosoftGraphCommand> commandMock = new Mock<IRefreshTokenForMicrosoftGraphCommand>();
            commandMock.Setup(m => m.RedirectUri)
                .Returns(redirectUri ?? CreateUri());
            commandMock.Setup(m => m.RefreshableToken)
                .Returns(refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object);
            return commandMock;
        }

        private Uri CreateUri()
        {
            return new Uri($"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}");
        }
    }
}
