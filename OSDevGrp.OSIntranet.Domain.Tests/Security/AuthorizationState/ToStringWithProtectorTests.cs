using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationState
{
    [TestFixture]
    public class ToStringWithProtectorTests : AuthorizationStateTestBase
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenProtectorIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationState sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToString(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("protector"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_AssertProtectorWasCalledWithByteArrayContainingMatchingJsonPropertyForResponseType()
        {
            string responseType = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(responseType: responseType);

            byte[] protectorCalledWithBytes = [];
            sut.ToString(bytes =>
            {
                protectorCalledWithBytes = bytes;
                return bytes;
            });

            Assert.That(HasMatchingResponseType(Convert.ToBase64String(protectorCalledWithBytes), responseType), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_AssertProtectorWasCalledWithByteArrayContainingMatchingJsonPropertyForClientId()
        {
            string clientId = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(clientId: clientId);

            byte[] protectorCalledWithBytes = [];
            sut.ToString(bytes =>
            {
                protectorCalledWithBytes = bytes;
                return bytes;
            });

            Assert.That(HasMatchingClientId(Convert.ToBase64String(protectorCalledWithBytes), clientId), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenClientSecretIsSet_AssertProtectorWasCalledWithByteArrayContainingMatchingJsonPropertyForClientSecret()
        {
            string clientSecret = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(hasClientSecret: true, clientSecret: clientSecret);

            byte[] protectorCalledWithBytes = [];
            sut.ToString(bytes =>
            {
                protectorCalledWithBytes = bytes;
                return bytes;
            });

            Assert.That(HasMatchingClientSecret(Convert.ToBase64String(protectorCalledWithBytes), clientSecret), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenClientSecretIsNotSet_AssertProtectorWasCalledWithByteArrayContainingMatchingJsonPropertyForClientSecret()
        {
            IAuthorizationState sut = CreateSut(hasClientSecret: false);

            byte[] protectorCalledWithBytes = [];
            sut.ToString(bytes =>
            {
                protectorCalledWithBytes = bytes;
                return bytes;
            });

            Assert.That(HasClientSecretWithoutValue(Convert.ToBase64String(protectorCalledWithBytes)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_AssertProtectorWasCalledWithByteArrayContainingMatchingJsonPropertyForRedirectUri()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            byte[] protectorCalledWithBytes = [];
            sut.ToString(bytes =>
            {
                protectorCalledWithBytes = bytes;
                return bytes;
            });

            Assert.That(HasMatchingRedirectUri(Convert.ToBase64String(protectorCalledWithBytes), redirectUri), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_AssertProtectorWasCalledWithByteArrayContainingMatchingJsonPropertyForScopes()
        {
            string[] scopes = CreateScopes(_fixture, _random);
            IAuthorizationState sut = CreateSut(scopes: scopes);

            byte[] protectorCalledWithBytes = [];
            sut.ToString(bytes =>
            {
                protectorCalledWithBytes = bytes;
                return bytes;
            });

            Assert.That(HasMatchingScopes(Convert.ToBase64String(protectorCalledWithBytes), scopes), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenExternalStateIsSet_AssertProtectorWasCalledWithByteArrayContainingMatchingJsonPropertyForExternalState()
        {
            string externalState = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(hasExternalState: true, externalState: externalState);

            byte[] protectorCalledWithBytes = [];
            sut.ToString(bytes =>
            {
                protectorCalledWithBytes = bytes;
                return bytes;
            });

            Assert.That(HasMatchingExternalState(Convert.ToBase64String(protectorCalledWithBytes), externalState), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenExternalStateIsNotSet_AssertProtectorWasCalledWithByteArrayContainingMatchingJsonPropertyForExternalState()
        {
            IAuthorizationState sut = CreateSut(hasExternalState: false);

            byte[] protectorCalledWithBytes = [];
            sut.ToString(bytes =>
            {
                protectorCalledWithBytes = bytes;
                return bytes;
            });

            Assert.That(HasExternalStateWithoutValue(Convert.ToBase64String(protectorCalledWithBytes)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenAuthorizationCodeIsSet_AssertProtectorWasCalledWithByteArrayContainingMatchingJsonPropertyForValueOnAuthorizationCode()
        {
            string value = _fixture.Create<string>();
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(value: value).Object;
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: true, authorizationCode: authorizationCode);

            byte[] protectorCalledWithBytes = [];
            sut.ToString(bytes =>
            {
                protectorCalledWithBytes = bytes;
                return bytes;
            });

            Assert.That(HasMatchingValueForAuthorizationCode(Convert.ToBase64String(protectorCalledWithBytes), value), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenAuthorizationCodeIsSet_AssertProtectorWasCalledWithByteArrayContainingMatchingJsonPropertyForExpiresOnAuthorizationCode()
        {
            DateTime expires = DateTime.Now.AddSeconds(_random.Next(5, 10));
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expires: expires).Object;
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: true, authorizationCode: authorizationCode);

            byte[] protectorCalledWithBytes = [];
            sut.ToString(bytes =>
            {
                protectorCalledWithBytes = bytes;
                return bytes;
            });

            Assert.That(HasMatchingExpiresForAuthorizationCode(Convert.ToBase64String(protectorCalledWithBytes), expires), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenAuthorizationCodeIsNotSet_AssertProtectorWasCalledWithByteArrayContainingMatchingJsonPropertyForAuthorizationCode()
        {
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: false);

            byte[] protectorCalledWithBytes = [];
            sut.ToString(bytes =>
            {
                protectorCalledWithBytes = bytes;
                return bytes;
            });

            Assert.That(HasAuthorizationCodeWithoutValue(Convert.ToBase64String(protectorCalledWithBytes)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationState sut = CreateSut();

            string result = sut.ToString(Protect);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsNoneEmptyString()
        {
            IAuthorizationState sut = CreateSut();

            string result = sut.ToString(Protect);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsBase64String()
        {
            IAuthorizationState sut = CreateSut();

            string result = sut.ToString(Protect);

            Assert.That(IsBase64String(result), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsBase64StringContainingMatchingJsonPropertyForResponseType()
        {
            string responseType = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(responseType: responseType);

            string result = sut.ToString(Protect);

            Assert.That(HasMatchingResponseType(result, responseType), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsBase64StringContainingMatchingJsonPropertyForClientId()
        {
            string clientId = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(clientId: clientId);

            string result = sut.ToString(Protect);

            Assert.That(HasMatchingClientId(result, clientId), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenClientSecretIsSet_ReturnsBase64StringContainingMatchingJsonPropertyForClientSecret()
        {
            string clientSecret = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(hasClientSecret: true, clientSecret: clientSecret);

            string result = sut.ToString(Protect);

            Assert.That(HasMatchingClientSecret(result, clientSecret), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenClientSecretIsNotSet_ReturnsBase64StringContainingMatchingJsonPropertyForClientSecret()
        {
            IAuthorizationState sut = CreateSut(hasClientSecret: false);

            string result = sut.ToString(Protect);

            Assert.That(HasClientSecretWithoutValue(result), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsBase64StringContainingMatchingJsonPropertyForRedirectUri()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            string result = sut.ToString(Protect);

            Assert.That(HasMatchingRedirectUri(result, redirectUri), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsBase64StringContainingMatchingJsonPropertyForScopes()
        {
            string[] scopes = CreateScopes(_fixture, _random);
            IAuthorizationState sut = CreateSut(scopes: scopes);

            string result = sut.ToString(Protect);

            Assert.That(HasMatchingScopes(result, scopes), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenExternalStateIsSet_ReturnsBase64StringContainingMatchingJsonPropertyForExternalState()
        {
            string externalState = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(hasExternalState: true, externalState: externalState);

            string result = sut.ToString(Protect);

            Assert.That(HasMatchingExternalState(result, externalState), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenExternalStateIsNotSet_ReturnsBase64StringContainingMatchingJsonPropertyForExternalState()
        {
            IAuthorizationState sut = CreateSut(hasExternalState: false);

            string result = sut.ToString(Protect);

            Assert.That(HasExternalStateWithoutValue(result), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenAuthorizationCodeIsSet_ReturnsBase64StringContainingMatchingJsonPropertyForValueOnAuthorizationCode()
        {
            string value = _fixture.Create<string>();
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(value: value).Object;
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: true, authorizationCode: authorizationCode);

            string result = sut.ToString(Protect);

            Assert.That(HasMatchingValueForAuthorizationCode(result, value), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenAuthorizationCodeIsSet_ReturnsBase64StringContainingMatchingJsonPropertyForExpiresOnAuthorizationCode()
        {
            DateTime expires = DateTime.Now.AddSeconds(_random.Next(5, 10));
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expires: expires).Object;
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: true, authorizationCode: authorizationCode);

            string result = sut.ToString(Protect);

            Assert.That(HasMatchingExpiresForAuthorizationCode(result, expires), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenAuthorizationCodeIsNotSet_ReturnsBase64StringContainingMatchingJsonPropertyForAuthorizationCode()
        {
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: false);

            string result = sut.ToString(Protect);

            Assert.That(HasAuthorizationCodeWithoutValue(result), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_Called_ReturnsBase64StringMatchingByteArrayFromProtect()
        {
            IAuthorizationState sut = CreateSut(hasExternalState: false);

            byte[] bytes = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
            string result = sut.ToString(_ => bytes);

            Assert.That(result, Is.EqualTo(Convert.ToBase64String(bytes)));
        }

        private IAuthorizationState CreateSut(string responseType = null, string clientId = null, bool hasClientSecret = false, string clientSecret = null, Uri redirectUri = null, string[] scopes = null, bool hasExternalState = true, string externalState = null, bool hasAuthorizationCode = false, IAuthorizationCode authorizationCode = null)
        {
            return CreateSut(_fixture, _random, responseType, clientId, hasClientSecret, clientSecret, redirectUri, scopes, hasExternalState, externalState, hasAuthorizationCode, authorizationCode);
        }

        private static byte[] Protect(byte[] bytes)
        {
            NullGuard.NotNull(bytes, nameof(bytes));

            return bytes;
        }
    }
}