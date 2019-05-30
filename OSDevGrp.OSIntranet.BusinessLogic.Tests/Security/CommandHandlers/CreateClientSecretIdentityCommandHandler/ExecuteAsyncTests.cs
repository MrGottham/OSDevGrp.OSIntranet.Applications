using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers.CreateClientSecretIdentityCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.CreateClientSecretIdentityCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWithoutClientIdAndClientSecretWasCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<ICreateClientSecretIdentityCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWithClientIdAndClientSecretWasNotCalledOnCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<ICreateClientSecretIdentityCommand> commandMock = CreateCommandMock();
            await sut.ExecuteAsync(commandMock.Object);

            commandMock.Verify(m => m.ToDomain(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertCreateClientSecretIdentityAsyncWasCalledOnSecurityRepository()
        {
            CommandHandler sut = CreateSut();

            IClientSecretIdentity clientSecretIdentity = _fixture.BuildClientSecretIdentityMock().Object;
            ICreateClientSecretIdentityCommand command = CreateCommandMock(clientSecretIdentity).Object;
            await sut.ExecuteAsync(command);

            _securityRepositoryMock.Verify(m => m.CreateClientSecretIdentityAsync(It.Is<IClientSecretIdentity>(value => value == clientSecretIdentity)), Times.Once);
        }

        private CommandHandler CreateSut()
        {
            return new CommandHandler(_validatorMock.Object, _securityRepositoryMock.Object);
        }

        private Mock<ICreateClientSecretIdentityCommand> CreateCommandMock(IClientSecretIdentity clientSecretIdentity = null)
        {
            Mock<ICreateClientSecretIdentityCommand> commandMock = new Mock<ICreateClientSecretIdentityCommand>();
            commandMock.Setup(m => m.ToDomain())
                .Returns(clientSecretIdentity ?? _fixture.BuildClientSecretIdentityMock().Object);
            return commandMock;
        }
    }
}