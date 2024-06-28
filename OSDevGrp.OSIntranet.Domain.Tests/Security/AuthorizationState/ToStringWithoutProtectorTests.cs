using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
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
        public void ToString_WhenCalled_ReturnsBase64StringContainingMatchingJsonPropertyForRedirectUri()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
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

        private IAuthorizationState CreateSut(string responseType = null, string clientId = null, Uri redirectUri = null, string[] scopes = null, bool hasExternalState = true, string externalState = null)
        {
            return CreateSut(_fixture, _random, responseType, clientId, redirectUri, scopes, hasExternalState, externalState);
        }
    }
}