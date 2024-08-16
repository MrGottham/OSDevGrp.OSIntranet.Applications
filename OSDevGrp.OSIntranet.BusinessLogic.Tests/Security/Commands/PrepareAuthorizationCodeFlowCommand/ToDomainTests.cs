using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.PrepareAuthorizationCodeFlowCommand
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Mock<IAuthorizationStateFactory> _authorizationStateFactoryMock;
        private Mock<IAuthorizationStateBuilder> _authorizationStateBuilderMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _authorizationStateFactoryMock = new Mock<IAuthorizationStateFactory>();
            _authorizationStateBuilderMock = new Mock<IAuthorizationStateBuilder>();
            _fixture = new Fixture();
            _random = new Random();
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAuthorizationStateFactoryIsNull_ThrowsArgumentNullException()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authorizationStateFactory"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertCreateWasCalledOnAuthorizationStateFactoryWithResponseTypeFromPrepareAuthorizationCodeFlowCommand()
        {
            string responseType = _fixture.Create<string>();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(responseType: responseType);

            sut.ToDomain(_authorizationStateFactoryMock.Object);

            _authorizationStateFactoryMock.Verify(m => m.Create(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, responseType) == 0),
                    It.IsAny<string>(),
                    It.IsAny<Uri>(),
                    It.IsAny<IEnumerable<string>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertCreateWasCalledOnAuthorizationStateFactoryWithClientIdFromPrepareAuthorizationCodeFlowCommand()
        {
            string clientId = _fixture.Create<string>();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(clientId: clientId);

            sut.ToDomain(_authorizationStateFactoryMock.Object);

            _authorizationStateFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, clientId) == 0),
                    It.IsAny<Uri>(),
                    It.IsAny<IEnumerable<string>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertCreateWasCalledOnAuthorizationStateFactoryWithRedirectUriFromPrepareAuthorizationCodeFlowCommand()
        {
            Uri redirectUri = _fixture.CreateEndpoint();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(redirectUri: redirectUri);

            sut.ToDomain(_authorizationStateFactoryMock.Object);

            _authorizationStateFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<Uri>(value => value != null && value == redirectUri),
                    It.IsAny<IEnumerable<string>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertCreateWasCalledOnAuthorizationStateFactoryWithScopesFromPrepareAuthorizationCodeFlowCommand()
        {
            string[] scopes = CreateScopes();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(scopes: scopes);

            sut.ToDomain(_authorizationStateFactoryMock.Object);

            _authorizationStateFactoryMock.Verify(m => m.Create(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Uri>(),
                    It.Is<IEnumerable<string>>( value => value != null && value == scopes)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenStateIsSetOnPrepareAuthorizationCodeFlowCommand_AssertWithExternalStateWasCalledOnAuthorizationStateBuilderWithStateFromPrepareAuthorizationCodeFlowCommand()
        {
            string state = _fixture.Create<string>();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(hasState: true, state: state);

            sut.ToDomain(_authorizationStateFactoryMock.Object);

            _authorizationStateBuilderMock.Verify(m => m.WithExternalState(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, state) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenStateIsSetOnPrepareAuthorizationCodeFlowCommand_AssertBuildWasCalledOnAuthorizationStateBuilder()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(hasState: true);

            sut.ToDomain(_authorizationStateFactoryMock.Object);

            _authorizationStateBuilderMock.Verify(m => m.Build(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenStateIsNotSetOnPrepareAuthorizationCodeFlowCommand_AssertWithExternalStateWasNotCalledOnAuthorizationStateBuilder()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(hasState: false);

            sut.ToDomain(_authorizationStateFactoryMock.Object);

            _authorizationStateBuilderMock.Verify(m => m.WithExternalState(It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenStateIsNotSetOnPrepareAuthorizationCodeFlowCommand_AssertBuildWasCalledOnAuthorizationStateBuilder()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(hasState: false);

            sut.ToDomain(_authorizationStateFactoryMock.Object);

            _authorizationStateBuilderMock.Verify(m => m.Build(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalled_ReturnsNotNull(bool hasState)
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(hasState: hasState);

            IAuthorizationState result = sut.ToDomain(_authorizationStateFactoryMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalled_ReturnsAuthorizationStateFromAuthorizationStateBuilder(bool hasState)
        {
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock().Object;
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(hasState: hasState, authorizationState: authorizationState);

            IAuthorizationState result = sut.ToDomain(_authorizationStateFactoryMock.Object);

            Assert.That(result, Is.EqualTo(authorizationState));
        }

        private IPrepareAuthorizationCodeFlowCommand CreateSut(string responseType = null, string clientId = null, Uri redirectUri = null, string[] scopes = null, bool hasState = true, string state = null, IAuthorizationState authorizationState = null)
        {
            _authorizationStateFactoryMock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Uri>(), It.IsAny<IEnumerable<string>>()))
                .Returns(_authorizationStateBuilderMock.Object);

            _authorizationStateBuilderMock.Setup(m => m.WithExternalState(It.IsAny<string>()))
                .Returns(_authorizationStateBuilderMock.Object);
            _authorizationStateBuilderMock.Setup(m => m.Build())
                .Returns(authorizationState ?? _fixture.BuildAuthorizationStateMock().Object);

            return new BusinessLogic.Security.Commands.PrepareAuthorizationCodeFlowCommand(
                responseType ?? _fixture.Create<string>(),
                clientId ?? _fixture.Create<string>(),
                redirectUri ?? _fixture.CreateEndpoint(),
                scopes ?? CreateScopes(),
                hasState ? state ?? _fixture.Create<string>() : state,
                bytes => bytes);
        }

        private string[] CreateScopes()
        {
            return _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
        }
    }
}