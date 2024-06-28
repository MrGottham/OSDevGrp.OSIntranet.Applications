using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.PrepareAuthorizationCodeFlowCommand
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolver;
        private Mock<ISupportedScopesProvider> _supportedScopesProviderMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _trustedDomainResolver = new Mock<ITrustedDomainResolver>();
            _supportedScopesProviderMock = new Mock<ISupportedScopesProvider>();
            _fixture = new Fixture();
            _random = new Random();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenSecurityRepositoryIsNull_ThrowsArgumentNullException()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("securityRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenTrustedDomainResolverIsNull_ThrowsArgumentNullException()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, null, _supportedScopesProviderMock.Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("trustedDomainResolver"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenSupportedScopesProviderIsNull_ThrowsArgumentNullException()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("supportedScopesProvider"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertSupportedScopesWasCalledSupportedScopesProvider()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _supportedScopesProviderMock.Verify(m => m.SupportedScopes, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertStringWasCalledSixTimesOnValidator()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.String, Times.Exactly(6));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertObjectWasCalledSevenTimesOnValidator()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Exactly(7));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertEnumerableWasCalledThreeTimesOnValidator()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.Enumerable, Times.Exactly(3));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithResponseType()
        {
            string responseType = _fixture.Create<string>();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(responseType: responseType);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, responseType) == 0),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ResponseType") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithResponseType()
        {
            string responseType = _fixture.Create<string>();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(responseType: responseType);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, responseType) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ResponseType") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithResponseType()
        {
            string responseType = _fixture.Create<string>();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(responseType: responseType);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, responseType) == 0),
                    It.Is<Regex>(value => value != null && string.CompareOrdinal(value.ToString(), RegexTestHelper.ResponseTypePattern) == 0),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ResponseType") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithClientId()
        {
            string clientId = _fixture.Create<string>();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(clientId: clientId);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, clientId) == 0),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ClientId") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithClientId()
        {
            string clientId = _fixture.Create<string>();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(clientId: clientId);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, clientId) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ClientId") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithClientId()
        {
            string clientId = _fixture.Create<string>();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(clientId: clientId);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, clientId) == 0),
                    It.IsNotNull<Func<string, Task<bool>>>(),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ClientId") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithRedirectUri()
        {
            Uri redirectUri = new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty)}", UriKind.Absolute);
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(redirectUri: redirectUri);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<Uri>(value => value != null && value == redirectUri),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "RedirectUri") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledTwoTimesOnObjectValidatorWithRedirectUri()
        {
            Uri redirectUri = new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty)}", UriKind.Absolute);
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(redirectUri: redirectUri);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value != null && value == redirectUri),
                    It.IsNotNull<Func<Uri, Task<bool>>>(),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "RedirectUri") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithScopes()
        {
            string[] scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(scopes: scopes);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<string[]>(value => value != null && value.All(scope => scopes.Contains(scope))),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Scopes") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldContainItemsWasCalledOnEnumerableValidatorWithScopes()
        {
            string[] scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(scopes: scopes);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldContainItems(
                    It.Is<string[]>(value => value != null && value.All(scope => scopes.Contains(scope))),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Scopes") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorWithScopes()
        {
            string[] scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(scopes: scopes);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMinItems(
                    It.Is<string[]>(value => value != null && value.All(scope => scopes.Contains(scope))),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Scopes") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxItemsWasCalledOnEnumerableValidatorWithScopes()
        {
            IDictionary<string, IScope> supportedScopes = new Dictionary<string, IScope>
            {
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object}
            };

            string[] scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(scopes: scopes, supportedScopes: supportedScopes);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMaxItems(
                    It.Is<string[]>(value => value != null && value.All(scope => scopes.Contains(scope))),
                    It.Is<int>(value => value == supportedScopes.Count),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Scopes") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithScopes()
        {
            string[] scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(scopes: scopes);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<string[]>(value => value != null && value.All(scope => scopes.Contains(scope))),
                    It.IsNotNull<Func<string[], Task<bool>>>(),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Scopes") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldShouldNotBeNullOrWhiteSpaceWasNotCalledOnStringValidatorWithState()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.IsAny<string>(),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "State") == 0)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithState()
        {
            string state = _fixture.Create<string>();
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(state: state);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, state) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "State") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasNotCalledOnStringValidatorWithState()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.IsAny<string>(),
                    It.IsAny<Regex>(),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "State") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithProtector()
        {
            Func<byte[], byte[]> protector = bytes => bytes;
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut(protector: protector);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<Func<byte[], byte[]>>(value => value != null && value == protector),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Protector") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsNotNull()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidatorFromArguments()
        {
            IPrepareAuthorizationCodeFlowCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object, _trustedDomainResolver.Object, _supportedScopesProviderMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IPrepareAuthorizationCodeFlowCommand CreateSut(string responseType = null, string clientId = null, Uri redirectUri = null, string[] scopes = null, bool hasState = true, string state = null, IDictionary<string, IScope> supportedScopes = null, Func<byte[], byte[]> protector = null)
        {
            supportedScopes ??= new Dictionary<string, IScope>
            {
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object},
                {_fixture.Create<string>(), _fixture.BuildScopeMock().Object}
            };

            _supportedScopesProviderMock.Setup(m => m.SupportedScopes)
                .Returns(new ConcurrentDictionary<string, IScope>(supportedScopes));

            return new BusinessLogic.Security.Commands.PrepareAuthorizationCodeFlowCommand(
                responseType ?? _fixture.Create<string>(),
                clientId ?? _fixture.Create<string>(),
                redirectUri ?? new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty)}", UriKind.Absolute),
                scopes ?? _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray(),
                hasState ? state ?? _fixture.Create<string>() : state,
                protector ?? (bytes => bytes));
        }
    }
}