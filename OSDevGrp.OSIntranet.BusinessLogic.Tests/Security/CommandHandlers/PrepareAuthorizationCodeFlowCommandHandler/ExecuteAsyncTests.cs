using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.PrepareAuthorizationCodeFlowCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<ISupportedScopesProvider> _supportedScopesProviderMock;
        private Mock<IAuthorizationStateFactory> _authorizationStateFactoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
            _supportedScopesProviderMock = new Mock<ISupportedScopesProvider>();
            _authorizationStateFactoryMock = new Mock<IAuthorizationStateFactory>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
        {
            ICommandHandler<IPrepareAuthorizationCodeFlowCommand, string> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("command"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnPrepareAuthorizationCodeFlowCommand()
        {
            ICommandHandler<IPrepareAuthorizationCodeFlowCommand, string> sut = CreateSut();

            Mock<IPrepareAuthorizationCodeFlowCommand> prepareAuthorizationCodeFlowCommandMock = CreatePrepareAuthorizationCodeFlowCommandMock();
            await sut.ExecuteAsync(prepareAuthorizationCodeFlowCommandMock.Object);

            prepareAuthorizationCodeFlowCommandMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.Is<ISecurityRepository>(value => value != null && value == _securityRepositoryMock.Object),
                    It.Is<ITrustedDomainResolver>(value => value != null && value == _trustedDomainResolverMock.Object),
                    It.Is<ISupportedScopesProvider>(value => value != null && value == _supportedScopesProviderMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnPrepareAuthorizationCodeFlowCommand()
        {
            ICommandHandler<IPrepareAuthorizationCodeFlowCommand, string> sut = CreateSut();

            Mock<IPrepareAuthorizationCodeFlowCommand> prepareAuthorizationCodeFlowCommandMock = CreatePrepareAuthorizationCodeFlowCommandMock();
            await sut.ExecuteAsync(prepareAuthorizationCodeFlowCommandMock.Object);

            prepareAuthorizationCodeFlowCommandMock.Verify(m => m.ToDomain(It.Is<IAuthorizationStateFactory>(value => value != null && value == _authorizationStateFactoryMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertProtectorWasCalledOnPrepareAuthorizationCodeFlowCommand()
        {
            ICommandHandler<IPrepareAuthorizationCodeFlowCommand, string> sut = CreateSut();

            Mock<IPrepareAuthorizationCodeFlowCommand> prepareAuthorizationCodeFlowCommandMock = CreatePrepareAuthorizationCodeFlowCommandMock();
            await sut.ExecuteAsync(prepareAuthorizationCodeFlowCommandMock.Object);

            prepareAuthorizationCodeFlowCommandMock.Verify(m => m.Protector, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToStringWasCalledAuthorizationStateCreatedByPrepareAuthorizationCodeFlowCommand()
        {
            ICommandHandler<IPrepareAuthorizationCodeFlowCommand, string> sut = CreateSut();

            Func<byte[], byte[]> protector = bytes => bytes;
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IPrepareAuthorizationCodeFlowCommand prepareAuthorizationCodeFlowCommand = CreatePrepareAuthorizationCodeFlowCommandMock(protector, authorizationStateMock.Object).Object;
            await sut.ExecuteAsync(prepareAuthorizationCodeFlowCommand);

            authorizationStateMock.Verify(m => m.ToString(It.Is < Func<byte[], byte[]>>(value => value != null && value == protector)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_ReturnsNotNull()
        {
            ICommandHandler<IPrepareAuthorizationCodeFlowCommand, string> sut = CreateSut();

            string result = await sut.ExecuteAsync(CreatePrepareAuthorizationCodeFlowCommand());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_ReturnsNotEmpty()
        {
            ICommandHandler<IPrepareAuthorizationCodeFlowCommand, string> sut = CreateSut();

            string result = await sut.ExecuteAsync(CreatePrepareAuthorizationCodeFlowCommand());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_ReturnsNonEmptyStringForAuthorizationState()
        {
            ICommandHandler<IPrepareAuthorizationCodeFlowCommand, string> sut = CreateSut();

            string authorizationStateAsString = _fixture.Create<string>();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(toStringValue: authorizationStateAsString).Object;
            IPrepareAuthorizationCodeFlowCommand prepareAuthorizationCodeFlowCommand = CreatePrepareAuthorizationCodeFlowCommandMock(authorizationState: authorizationState).Object;
            string result = await sut.ExecuteAsync(prepareAuthorizationCodeFlowCommand);

            Assert.That(result, Is.EqualTo(authorizationStateAsString));
        }

        private ICommandHandler<IPrepareAuthorizationCodeFlowCommand, string> CreateSut()
        {
            return new BusinessLogic.Security.CommandHandlers.PrepareAuthorizationCodeFlowCommandHandler(_validatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object, _authorizationStateFactoryMock.Object);
        }

        private IPrepareAuthorizationCodeFlowCommand CreatePrepareAuthorizationCodeFlowCommand(Func<byte[], byte[]> protector = null, IAuthorizationState authorizationState = null)
        {
            return CreatePrepareAuthorizationCodeFlowCommandMock(protector, authorizationState).Object;
        }

        private Mock<IPrepareAuthorizationCodeFlowCommand> CreatePrepareAuthorizationCodeFlowCommandMock(Func<byte[], byte[]> protector = null, IAuthorizationState authorizationState = null)
        {
            Mock<IPrepareAuthorizationCodeFlowCommand> prepareAuthorizationCodeFlowCommandMock = new Mock<IPrepareAuthorizationCodeFlowCommand>();
            prepareAuthorizationCodeFlowCommandMock.Setup(m => m.Protector)
                .Returns(protector ?? (bytes => bytes));
            prepareAuthorizationCodeFlowCommandMock.Setup(m => m.Validate(It.IsAny<IValidator>(), It.IsAny<ISecurityRepository>(), It.IsAny<ITrustedDomainResolver>(), It.IsAny<ISupportedScopesProvider>()))
                .Returns(_validatorMock.Object);
            prepareAuthorizationCodeFlowCommandMock.Setup(m => m.ToDomain(It.IsAny<IAuthorizationStateFactory>()))
                .Returns(authorizationState ?? _fixture.BuildAuthorizationStateMock().Object);
            return prepareAuthorizationCodeFlowCommandMock;
        }
    }
}