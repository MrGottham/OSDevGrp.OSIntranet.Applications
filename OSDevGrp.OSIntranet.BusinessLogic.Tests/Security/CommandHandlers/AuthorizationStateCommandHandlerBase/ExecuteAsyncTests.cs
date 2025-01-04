using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.AuthorizationStateCommandHandlerBase
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAuthorizationStateFactory> _authorizationStateFactoryMock;
        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<ISupportedScopesProvider> _supportedScopesProviderMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _authorizationStateFactoryMock = new Mock<IAuthorizationStateFactory>();
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
            _supportedScopesProviderMock = new Mock<ISupportedScopesProvider>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenAuthorizationStateCommandIsNull_ThrowsArgumentNullException()
        {
            ICommandHandler<IAuthorizationStateCommand, object> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("command"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnAuthorizationStateCommand()
        {
            ICommandHandler<IAuthorizationStateCommand, object> sut = CreateSut();

            Mock<IAuthorizationStateCommand> authorizationStateCommandMock = CreateAuthorizationStateCommandMock();
            await sut.ExecuteAsync(authorizationStateCommandMock.Object);

            authorizationStateCommandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnAuthorizationStateCommand()
        {
            ICommandHandler<IAuthorizationStateCommand, object> sut = CreateSut();

            Mock<IAuthorizationStateCommand> authorizationStateCommandMock = CreateAuthorizationStateCommandMock();
            await sut.ExecuteAsync(authorizationStateCommandMock.Object);

            authorizationStateCommandMock.Verify(m => m.ToDomain(
                    It.Is<IAuthorizationStateFactory>(value => value != null && value == _authorizationStateFactoryMock.Object),
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.Is<ISecurityRepository>(value => value != null && value == _securityRepositoryMock.Object),
                    It.Is<ITrustedDomainResolver>(value => value != null && value == _trustedDomainResolverMock.Object),
                    It.Is<ISupportedScopesProvider>(value => value != null && value == _supportedScopesProviderMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertHandleAuthorizationStateAsyncWasCalledOnAuthorizationStateCommandHandlerBase()
        {
            ICommandHandler<IAuthorizationStateCommand, object> sut = CreateSut();

            IAuthorizationStateCommand authorizationStateCommand = CreateAuthorizationStateCommand();
            await sut.ExecuteAsync(authorizationStateCommand);

            Assert.That(((MyAuthorizationStateCommandHandler) sut).HandleAuthorizationStateAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertHandleAuthorizationStateAsyncWasCalledOnAuthorizationStateCommandHandlerBaseWithSameAuthorizationStateCommand()
        {
            ICommandHandler<IAuthorizationStateCommand, object> sut = CreateSut();

            IAuthorizationStateCommand authorizationStateCommand = CreateAuthorizationStateCommand();
            await sut.ExecuteAsync(authorizationStateCommand);

            Assert.That(((MyAuthorizationStateCommandHandler) sut).HandleAuthorizationStateAsyncWasCalledWithAuthorizationStateCommand, Is.SameAs(authorizationStateCommand));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertHandleAuthorizationStateAsyncWasCalledOnAuthorizationStateCommandHandlerBaseWithAuthorizationStateResolvedByAuthorizationStateCommand()
        {
            ICommandHandler<IAuthorizationStateCommand, object> sut = CreateSut();

            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock().Object;
            IAuthorizationStateCommand authorizationStateCommand = CreateAuthorizationStateCommand(toDomain: authorizationState);
            await sut.ExecuteAsync(authorizationStateCommand);

            Assert.That(((MyAuthorizationStateCommandHandler) sut).HandleAuthorizationStateAsyncWasCalledWithAuthorizationState, Is.SameAs(authorizationState));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_ReturnsNotNull()
        {
            ICommandHandler<IAuthorizationStateCommand, object> sut = CreateSut();

            IAuthorizationStateCommand authorizationStateCommand = CreateAuthorizationStateCommand();
            object result = await sut.ExecuteAsync(authorizationStateCommand);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_ReturnsResultFromHandleAuthorizationStateAsync()
        {
            object handleAuthorizationStateResult = _fixture.Create<object>();
            ICommandHandler<IAuthorizationStateCommand, object> sut = CreateSut(handleAuthorizationStateResult: handleAuthorizationStateResult);

            IAuthorizationStateCommand authorizationStateCommand = CreateAuthorizationStateCommand();
            object result = await sut.ExecuteAsync(authorizationStateCommand);

            Assert.That(result, Is.EqualTo(handleAuthorizationStateResult));
        }

        private ICommandHandler<IAuthorizationStateCommand, object> CreateSut(object handleAuthorizationStateResult = null)
        {
            return new MyAuthorizationStateCommandHandler(handleAuthorizationStateResult ?? _fixture.Create<object>(), _validatorMock.Object, _authorizationStateFactoryMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);
        }

        private IAuthorizationStateCommand CreateAuthorizationStateCommand(IAuthorizationState toDomain = null)
        {
            return CreateAuthorizationStateCommandMock(toDomain).Object;
        }

        private Mock<IAuthorizationStateCommand> CreateAuthorizationStateCommandMock(IAuthorizationState toDomain = null)
        {
            Mock<IAuthorizationStateCommand> authorizationStateCommandMock = new Mock<IAuthorizationStateCommand>();
            authorizationStateCommandMock.Setup(m => m.Validate(It.IsAny<IValidator>()))
                .Returns(_validatorMock.Object);
            authorizationStateCommandMock.Setup(m => m.ToDomain(It.IsAny<IAuthorizationStateFactory>(), It.IsAny<IValidator>(), It.IsAny<ISecurityRepository>(), It.IsAny<ITrustedDomainResolver>(), It.IsAny<ISupportedScopesProvider>()))
                .Returns(toDomain ?? _fixture.BuildAuthorizationStateMock().Object);
            return authorizationStateCommandMock;
        }

        private class MyAuthorizationStateCommandHandler : BusinessLogic.Security.CommandHandlers.AuthorizationStateCommandHandlerBase<IAuthorizationStateCommand, object>
        {
            #region Private variables

            private readonly object _handleAuthorizationStateResult;

            #endregion

            #region Constructors

            public MyAuthorizationStateCommandHandler(object handleAuthorizationStateResult, IValidator validator, IAuthorizationStateFactory authorizationStateFactory, ISecurityRepository securityRepository, ITrustedDomainResolver trustedDomainResolver, ISupportedScopesProvider supportedScopesProvider)
                : base(validator, authorizationStateFactory, securityRepository, trustedDomainResolver, supportedScopesProvider)
            {
                NullGuard.NotNull(handleAuthorizationStateResult, nameof(handleAuthorizationStateResult));

                _handleAuthorizationStateResult = handleAuthorizationStateResult;
            }

            #endregion

            #region Properties

            public bool HandleAuthorizationStateAsyncWasCalled { get; private set; }

            public IAuthorizationStateCommand HandleAuthorizationStateAsyncWasCalledWithAuthorizationStateCommand { get; private set; }

            public IAuthorizationState HandleAuthorizationStateAsyncWasCalledWithAuthorizationState { get; private set; }

            #endregion

            #region Methods

            protected override Task<object> HandleAuthorizationStateAsync(IAuthorizationStateCommand command, IAuthorizationState authorizationState)
            {
                NullGuard.NotNull(command, nameof(command))
                    .NotNull(authorizationState, nameof(authorizationState));

                HandleAuthorizationStateAsyncWasCalled = true;
                HandleAuthorizationStateAsyncWasCalledWithAuthorizationStateCommand = command;
                HandleAuthorizationStateAsyncWasCalledWithAuthorizationState = authorizationState;

                return Task.FromResult(_handleAuthorizationStateResult);
            }

            #endregion
        }
    }
}