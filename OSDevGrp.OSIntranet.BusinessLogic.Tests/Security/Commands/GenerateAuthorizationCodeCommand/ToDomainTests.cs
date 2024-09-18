using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.GenerateAuthorizationCodeCommand
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Mock<IAuthorizationStateFactory> _authorizationStateFactoryMock;
        private ValidatorMockContext _validatorMockContext;
        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<ISupportedScopesProvider> _supportedScopesProviderMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _authorizationStateFactoryMock = new Mock<IAuthorizationStateFactory>();
            _validatorMockContext = new ValidatorMockContext();
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
            _supportedScopesProviderMock = new Mock<ISupportedScopesProvider>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAuthorizationStateFactoryIsNull_ThrowsArgumentNullException()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authorizationStateFactory"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(_authorizationStateFactoryMock.Object, null, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenSecurityRepositoryIsNull_ThrowsArgumentNullException()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, null, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("securityRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenTrustedDomainResolverIsNull_ThrowsArgumentNullException()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, null, _supportedScopesProviderMock.Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("trustedDomainResolver"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenSupportedScopesProviderIsNull_ThrowsArgumentNullException()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("supportedScopesProvider"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertFromBase64StringWasCalledOnAuthorizationStateFactoryWithAuthorizationStateFromGenerateAuthorizationCodeCommand()
        {
            string authorizationState = _fixture.Create<string>();
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationState: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _authorizationStateFactoryMock.Verify(m => m.FromBase64String(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, authorizationState) == 0),
                    It.IsAny<Func<byte[], byte[]>>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertFromBase64StringWasCalledOnAuthorizationStateFactoryWithUnprotectFromGenerateAuthorizationCodeCommand()
        {
            Func<byte[], byte[]> unprotect = bytes => bytes;
            IGenerateAuthorizationCodeCommand sut = CreateSut(unprotect: unprotect);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _authorizationStateFactoryMock.Verify(m => m.FromBase64String(
                    It.IsAny<string>(),
                    It.Is<Func<byte[], byte[]>>(value => value != null && value == unprotect)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertResponseTypeWasCalledOnAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationStateMock.Object);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            authorizationStateMock.Verify(m => m.ResponseType, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertClientIdWasCalledOnAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationStateMock.Object);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            authorizationStateMock.Verify(m => m.ClientId, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertClientSecretWasNotCalledOnAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationStateMock.Object);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            authorizationStateMock.Verify(m => m.ClientSecret, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertRedirectUriWasCalledOnAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationStateMock.Object);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            authorizationStateMock.Verify(m => m.RedirectUri, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertScopesWasCalledOnAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationStateMock.Object);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            authorizationStateMock.Verify(m => m.Scopes, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertAuthorizationCodeWasNotCalledOnAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock();
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationStateMock.Object);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            authorizationStateMock.Verify(m => m.AuthorizationCode, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalled_AssertExternalStateWasCalledOnAuthorizationStateCreatedByAuthorizationStateFactory(bool hasExternalState)
        {
            Mock<IAuthorizationState> authorizationStateMock = _fixture.BuildAuthorizationStateMock(hasExternalState: hasExternalState);
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationStateMock.Object);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            authorizationStateMock.Verify(m => m.ExternalState, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertSupportedScopesWasCalledOnSupportedScopesProvider()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _supportedScopesProviderMock.Verify(m => m.SupportedScopes, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertStringWasCalledSixTimesOnValidator()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.String, Times.Exactly(6));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertObjectWasCalledSixTimesOnValidator()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Exactly(6));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertEnumerableWasCalledThreeTimesOnValidator()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.Enumerable, Times.Exactly(3));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithResponseTypeFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            string responseType = _fixture.Create<string>();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(responseType: responseType).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, responseType) == 0),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ResponseType") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithResponseTypeFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            string responseType = _fixture.Create<string>();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(responseType: responseType).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, responseType) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ResponseType") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithResponseTypeFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            string responseType = _fixture.Create<string>();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(responseType: responseType).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, responseType) == 0),
                    It.Is<Regex>(value => value != null && string.CompareOrdinal(value.ToString(), RegexTestHelper.ResponseTypeForAuthorizationCodeFlowPattern) == 0),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ResponseType") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithClientIdFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            string clientId = _fixture.Create<string>();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(clientId: clientId).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, clientId) == 0),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ClientId") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithClientIdFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            string clientId = _fixture.Create<string>();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(clientId: clientId).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, clientId) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ClientId") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithClientIdFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            string clientId = _fixture.Create<string>();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(clientId: clientId).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, clientId) == 0),
                    It.IsNotNull<Func<string, Task<bool>>>(),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ClientId") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithRedirectUriFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(redirectUri: redirectUri).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<Uri>(value => value != null && value == redirectUri),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "RedirectUri") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldBeKnownValueWasCalledTwoTimesOnObjectValidatorWithRedirectUriFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(redirectUri: redirectUri).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value != null && value == redirectUri),
                    It.IsNotNull<Func<Uri, Task<bool>>>(),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "RedirectUri") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithScopesFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            string[] scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(scopes: scopes).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<string[]>(value => value != null && scopes.All(value.Contains)),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Scopes") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldContainItemsWasCalledOnEnumerableValidatorWithScopesFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            string[] scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(scopes: scopes).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldContainItems(
                    It.Is<string[]>(value => value != null && scopes.All(value.Contains)),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Scopes") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorWithScopesFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            string[] scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(scopes: scopes).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMinItems(
                    It.Is<string[]>(value => value != null && scopes.All(value.Contains)),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Scopes") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldHaveMaxItemsWasCalledOnEnumerableValidatorWithScopesFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            IDictionary<string, IScope> supportedScopes = new Dictionary<string, IScope>
            {
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object}
            };

            string[] scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(scopes: scopes).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState, supportedScopes: supportedScopes);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMaxItems(
                    It.Is<string[]>(value => value != null && scopes.All(value.Contains)),
                    It.Is<int>(value => value == supportedScopes.Count),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Scopes") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithScopesFromAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            string[] scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(scopes: scopes).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<string[]>(value => value != null && scopes.All(value.Contains)),
                    It.IsNotNull<Func<string[], Task<bool>>>(),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Scopes") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasNotCalledOnStringValidatorWithExternalStateFromAuthorizationStateCreatedByAuthorizationStateFactory(bool hasExternalState)
        {
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(hasExternalState: hasExternalState).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.IsAny<string>(),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ExternalState") == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithExternalStateAuthorizationStateCreatedByAuthorizationStateFactory(bool hasExternalState)
        {
            string externalState = hasExternalState ? _fixture.Create<string>() : null;
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(hasExternalState: hasExternalState, externalState: externalState).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => hasExternalState ? string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, externalState) == 0 : string.IsNullOrWhiteSpace(value) == true),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ExternalState") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToDomain_WhenCalled_AssertShouldMatchPatternWasNotCalledOnStringValidatorWithExternalStateFromAuthorizationStateCreatedByAuthorizationStateFactory(bool hasExternalState)
        {
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock(hasExternalState: hasExternalState).Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.IsAny<string>(),
                    It.IsAny<Regex>(),
                    It.Is<Type>(value => value != null && value == authorizationState.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ExternalState") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNotNull()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            IAuthorizationState result = sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAuthorizationStateCreatedByAuthorizationStateFactory()
        {
            IAuthorizationState authorizationState = _fixture.BuildAuthorizationStateMock().Object;
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationStateFromFactory: authorizationState);

            IAuthorizationState result = sut.ToDomain(_authorizationStateFactoryMock.Object, _validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolverMock.Object, _supportedScopesProviderMock.Object);

            Assert.That(result, Is.EqualTo(authorizationState));
        }

        private IGenerateAuthorizationCodeCommand CreateSut(string authorizationState = null, Func<byte[], byte[]> unprotect = null, IAuthorizationState authorizationStateFromFactory = null, IDictionary<string, IScope> supportedScopes = null)
        {
            supportedScopes ??= new Dictionary<string, IScope>
            {
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object}
            };

            _authorizationStateFactoryMock.Setup(m => m.FromBase64String(It.IsAny<string>(), It.IsAny<Func<byte[], byte[]>>()))
                .Returns(authorizationStateFromFactory ?? _fixture.BuildAuthorizationStateMock().Object);

            _supportedScopesProviderMock.Setup(m => m.SupportedScopes)
                .Returns(new ConcurrentDictionary<string, IScope>(supportedScopes));

            return new BusinessLogic.Security.Commands.GenerateAuthorizationCodeCommand(authorizationState ?? _fixture.Create<string>(), Array.Empty<Claim>(), unprotect ?? (bytes => bytes));
        }
    }
}