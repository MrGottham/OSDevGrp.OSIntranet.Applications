using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationState
{
    [TestFixture]
    public class ToStringWithoutProtectorTests : AuthorizationStateTestBase
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
        public void ToString_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationState sut = CreateSut();

            string result = sut.ToString();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsNoneEmptyString()
        {
            IAuthorizationState sut = CreateSut();

            string result = sut.ToString();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsBase64String()
        {
            IAuthorizationState sut = CreateSut();

            string result = sut.ToString();

            Assert.That(IsBase64String(result), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsBase64StringContainingMatchingJsonPropertyForResponseType()
        {
            string responseType = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(responseType: responseType);

            string result = sut.ToString();

            Assert.That(HasMatchingResponseType(result, responseType), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsBase64StringContainingMatchingJsonPropertyForClientId()
        {
            string clientId = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(clientId: clientId);

            string result = sut.ToString();

            Assert.That(HasMatchingClientId(result, clientId), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenClientSecretIsSet_ReturnsBase64StringContainingMatchingJsonPropertyForClientSecret()
        {
            string clientSecret = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(hasClientSecret: true, clientSecret: clientSecret);

            string result = sut.ToString();

            Assert.That(HasMatchingClientSecret(result, clientSecret), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenClientSecretIsNotSet_ReturnsBase64StringContainingMatchingJsonPropertyForClientSecret()
        {
            IAuthorizationState sut = CreateSut(hasClientSecret: false);

            string result = sut.ToString();

            Assert.That(HasClientSecretWithoutValue(result), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsBase64StringContainingMatchingJsonPropertyForRedirectUri()
        {
            Uri redirectUri = _fixture.CreateEndpoint();
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            string result = sut.ToString();

            Assert.That(HasMatchingRedirectUri(result, redirectUri), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenCalled_ReturnsBase64StringContainingMatchingJsonPropertyForScopes()
        {
            string[] scopes = CreateScopes(_fixture, _random);
            IAuthorizationState sut = CreateSut(scopes: scopes);

            string result = sut.ToString();

            Assert.That(HasMatchingScopes(result, scopes), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenExternalStateIsSet_ReturnsBase64StringContainingMatchingJsonPropertyForExternalState()
        {
            string externalState = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(hasExternalState: true, externalState: externalState);

            string result = sut.ToString();

            Assert.That(HasMatchingExternalState(result, externalState), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenExternalStateIsNotSet_ReturnsBase64StringContainingMatchingJsonPropertyForExternalState()
        {
            IAuthorizationState sut = CreateSut(hasExternalState: false);

            string result = sut.ToString();

            Assert.That(HasExternalStateWithoutValue(result), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenNonceIsSet_ReturnsBase64StringContainingMatchingJsonPropertyForNonce()
        {
            string nonce = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(hasNonce: true, nonce: nonce);

            string result = sut.ToString();

            Assert.That(HasMatchingNonce(result, nonce), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenNonceIsNotSet_ReturnsBase64StringContainingMatchingJsonPropertyForNonce()
        {
            IAuthorizationState sut = CreateSut(hasNonce: false);

            string result = sut.ToString();

            Assert.That(HasNonceWithoutValue(result), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenAuthorizationCodeIsSet_ReturnsBase64StringContainingMatchingJsonPropertyForValueOnAuthorizationCode()
        {
            string value = _fixture.Create<string>();
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(value: value).Object;
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: true, authorizationCode: authorizationCode);

            string result = sut.ToString();

            Assert.That(HasMatchingValueForAuthorizationCode(result, value), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenAuthorizationCodeIsSet_ReturnsBase64StringContainingMatchingJsonPropertyForExpiresOnAuthorizationCode()
        {
            DateTime expires = DateTime.Now.AddSeconds(_random.Next(5, 10));
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expires: expires).Object;
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: true, authorizationCode: authorizationCode);

            string result = sut.ToString();

            Assert.That(HasMatchingExpiresForAuthorizationCode(result, expires), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToString_WhenAuthorizationCodeIsNotSet_ReturnsBase64StringContainingMatchingJsonPropertyForAuthorizationCode()
        {
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: false);

            string result = sut.ToString();

            Assert.That(HasAuthorizationCodeWithoutValue(result), Is.True);
        }

        private IAuthorizationState CreateSut(string responseType = null, string clientId = null, bool hasClientSecret = false, string clientSecret = null, Uri redirectUri = null, string[] scopes = null, bool hasExternalState = true, string externalState = null, bool hasNonce = true, string nonce = null, bool hasAuthorizationCode = false, IAuthorizationCode authorizationCode = null)
        {
            return CreateSut(_fixture, _random, responseType, clientId, hasClientSecret, clientSecret, redirectUri, scopes, hasExternalState, externalState, hasNonce, nonce, hasAuthorizationCode, authorizationCode);
        }
    }
}