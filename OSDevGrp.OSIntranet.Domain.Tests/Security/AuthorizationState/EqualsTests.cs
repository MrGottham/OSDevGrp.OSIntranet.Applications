using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationState
{
    [TestFixture]
    public class EqualsTests : AuthorizationStateTestBase
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
        public void Equals_WhenObjectIsNull_ReturnsFalse()
        {
            IAuthorizationState sut = CreateSut();

            bool result = sut.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsNonAuthorizationState_ReturnsFalse()
        {
            IAuthorizationState sut = CreateSut();

            bool result = sut.Equals(_fixture.Create<object>());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void Equals_WhenObjectIsAuthorizationStateWithNonMatchingValues_ReturnsFalse(bool hasClientSecret, bool hasExternalState, bool hasAuthorizationCode)
        {
            IAuthorizationState sut = CreateSut(hasClientSecret: hasClientSecret, hasExternalState: hasExternalState, hasAuthorizationCode: hasAuthorizationCode);

            IAuthorizationState authorizationState = CreateSut(hasClientSecret: hasClientSecret, hasExternalState: hasExternalState, hasAuthorizationCode: hasAuthorizationCode);
            bool result = sut.Equals(authorizationState);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void Equals_WhenObjectIsAuthorizationStateWithMatchingValues_ReturnsTrue(bool hasClientSecret, bool hasExternalState, bool hasAuthorizationCode)
        {
            string responseType = _fixture.Create<string>();
            string clientId = _fixture.Create<string>();
            string clientSecret = _fixture.Create<string>();
            Uri redirectUri = CreateRedirectUri(_fixture);
            string[] scopes = CreateScopes(_fixture, _random);
            string externalState = _fixture.Create<string>();
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock().Object;
            IAuthorizationState sut = CreateSut(responseType, clientId, hasClientSecret, clientSecret, redirectUri, scopes, hasExternalState, externalState, hasAuthorizationCode, authorizationCode);

            IAuthorizationState authorizationState = CreateSut(responseType, clientId, hasClientSecret, clientSecret, redirectUri, scopes, hasExternalState, externalState, hasAuthorizationCode, authorizationCode);
            bool result = sut.Equals(authorizationState);

            Assert.That(result, Is.True);
        }

        private IAuthorizationState CreateSut(string responseType = null, string clientId = null, bool hasClientSecret = false, string clientSecret = null, Uri redirectUri = null, string[] scopes = null, bool hasExternalState = true, string externalState = null, bool hasAuthorizationCode = false, IAuthorizationCode authorizationCode = null)
        {
            return CreateSut(_fixture, _random, responseType, clientId, hasClientSecret, clientSecret, redirectUri, scopes, hasExternalState, externalState, hasAuthorizationCode, authorizationCode);
        }
    }
}