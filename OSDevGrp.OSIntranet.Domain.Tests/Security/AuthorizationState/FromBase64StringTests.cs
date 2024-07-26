using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using Sut = OSDevGrp.OSIntranet.Domain.Security.AuthorizationState;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationState
{
    [TestFixture]
    public class FromBase64StringTests : AuthorizationStateTestBase
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
        public void FromBase64String_WhenBase64StringIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.FromBase64String(null, Unprotect));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("base64String"));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenBase64StringIsEmpty_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.FromBase64String(string.Empty, Unprotect));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("base64String"));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenBase64StringIsWhiteSpace_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.FromBase64String(" ", Unprotect));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("base64String"));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenUnprotectIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Sut.FromBase64String(CreateBase64String(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("unprotect"));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_AssertUnprotectWasCalled()
        {
            bool unprotectWasCalled = false;
            Sut.FromBase64String(CreateBase64String(), bytes =>
            {
                unprotectWasCalled = true;
                return bytes;
            });

            Assert.That(unprotectWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_AssertUnprotectWasCalledWithBytesForBase64String()
        {
            string base64String = CreateBase64String();

            byte[] unprotectCalledWithBytes = [];
            Sut.FromBase64String(base64String, bytes =>
            {
                unprotectCalledWithBytes = bytes;
                return bytes;
            });

            Assert.That(Convert.ToBase64String(unprotectCalledWithBytes), Is.EqualTo(base64String));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(), Unprotect);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationState()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(), Unprotect);

            Assert.That(result, Is.TypeOf<Sut>());
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereResponseTypeIsNotNull()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(), Unprotect);

            Assert.That(result.ResponseType, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereResponseTypeIsNotEmpty()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(), Unprotect);

            Assert.That(result.ResponseType, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereResponseTypeIsEqualToResponseTypeFromBase64String()
        {
            string responseType = _fixture.Create<string>();
            string base64String = CreateBase64String(responseType: responseType);
            IAuthorizationState result = Sut.FromBase64String(base64String, Unprotect);

            Assert.That(result.ResponseType, Is.EqualTo(responseType));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereClientIdIsNotNull()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(), Unprotect);

            Assert.That(result.ClientId, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereClientIdIsNotEmpty()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(), Unprotect);

            Assert.That(result.ClientId, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereClientIdIsEqualToClientIdFromBase64String()
        {
            string clientId = _fixture.Create<string>();
            string base64String = CreateBase64String(clientId: clientId);
            IAuthorizationState result = Sut.FromBase64String(base64String, Unprotect);

            Assert.That(result.ClientId, Is.EqualTo(clientId));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenClientSecretIsSetInBase64String_ReturnsAuthorizationStateWhereClientSecretIsNotNull()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(hasClientSecret: true), Unprotect);

            Assert.That(result.ClientSecret, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenClientSecretIsSetInBase64String_ReturnsAuthorizationStateWhereClientSecretIsNotEmpty()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(hasClientSecret: true), Unprotect);

            Assert.That(result.ClientSecret, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenClientSecretIsSetInBase64String_ReturnsAuthorizationStateWhereClientSecretIsEqualToClientSecretFromBase64String()
        {
            string clientSecret = _fixture.Create<string>();
            string base64String = CreateBase64String(hasClientSecret: true, clientSecret: clientSecret);
            IAuthorizationState result = Sut.FromBase64String(base64String, Unprotect);

            Assert.That(result.ClientSecret, Is.EqualTo(clientSecret));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenClientSecretIsNotSetInBase64String_ReturnsAuthorizationStateWhereClientSecretIsNull()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(hasClientSecret: false), Unprotect);

            Assert.That(result.ClientSecret, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereRedirectUriIsNotNull()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(), Unprotect);

            Assert.That(result.RedirectUri, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereRedirectUriIsEqualToRedirectUriFromBase64String()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            string base64String = CreateBase64String(redirectUri: redirectUri);
            IAuthorizationState result = Sut.FromBase64String(base64String, Unprotect);

            Assert.That(result.RedirectUri, Is.EqualTo(redirectUri));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereScopesIsNotNull()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(), Unprotect);

            Assert.That(result.Scopes, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereScopesIsNotEmpty()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(), Unprotect);

            Assert.That(result.Scopes, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereScopesIsEqualToScopesFromBase64String()
        {
            string[] scopes = CreateScopes(_fixture, _random);
            string base64String = CreateBase64String(scopes: scopes);
            IAuthorizationState result = Sut.FromBase64String(base64String, Unprotect);

            Assert.That(result.Scopes, Is.EqualTo(scopes));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenExternalStateIsSetInBase64String_ReturnsAuthorizationStateWhereExternalStateIsNotNull()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(hasExternalState: true), Unprotect);

            Assert.That(result.ExternalState, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenExternalStateIsSetInBase64String_ReturnsAuthorizationStateWhereExternalStateIsNotEmpty()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(hasExternalState: true), Unprotect);

            Assert.That(result.ExternalState, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenExternalStateIsSetInBase64String_ReturnsAuthorizationStateWhereExternalStateIsEqualToExternalStateFromBase64String()
        {
            string externalState = _fixture.Create<string>();
            string base64String = CreateBase64String(hasExternalState: true, externalState: externalState);
            IAuthorizationState result = Sut.FromBase64String(base64String, Unprotect);

            Assert.That(result.ExternalState, Is.EqualTo(externalState));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenExternalStateIsNotSetInBase64String_ReturnsAuthorizationStateWhereExternalStateIsNull()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(hasExternalState: false), Unprotect);

            Assert.That(result.ExternalState, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenAuthorizationCodeIsSetInBase64String_ReturnsAuthorizationStateWhereAuthorizationCodeIsNotNull()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(hasAuthorizationCode: true), Unprotect);

            Assert.That(result.AuthorizationCode, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenAuthorizationCodeIsSetInBase64String_ReturnsAuthorizationStateWhereValueOnAuthorizationCodeIsNotNull()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(hasAuthorizationCode: true), Unprotect);

            Assert.That(result.AuthorizationCode.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenAuthorizationCodeIsSetInBase64String_ReturnsAuthorizationStateWhereValueOnAuthorizationCodeIsNotEmpty()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(hasAuthorizationCode: true), Unprotect);

            Assert.That(result.AuthorizationCode.Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenAuthorizationCodeIsSetInBase64String_ReturnsAuthorizationStateWhereValueOnAuthorizationCodeIsEqualToValueOnAuthorizationCodeFromBase64String()
        {
            string value = _fixture.Create<string>();
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(value: value).Object;
            string base64String = CreateBase64String(hasAuthorizationCode: true, authorizationCode: authorizationCode);
            IAuthorizationState result = Sut.FromBase64String(base64String, Unprotect);

            Assert.That(result.AuthorizationCode.Value, Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenAuthorizationCodeIsSetInBase64String_ReturnsAuthorizationStateWhereExpiresOnAuthorizationCodeIsEqualToExpiresOnAuthorizationCodeFromBase64String()
        {
            DateTimeOffset expires = DateTimeOffset.Now.AddSeconds(_random.Next(5, 10));
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expires: expires).Object;
            string base64String = CreateBase64String(hasAuthorizationCode: true, authorizationCode: authorizationCode);
            IAuthorizationState result = Sut.FromBase64String(base64String, Unprotect);

            Assert.That(result.AuthorizationCode.Expires, Is.EqualTo(expires.UtcDateTime));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenAuthorizationCodeIsNotSetInBase64String_ReturnsAuthorizationStateWhereAuthorizationCodeIsNull()
        {
            IAuthorizationState result = Sut.FromBase64String(CreateBase64String(hasAuthorizationCode: false), Unprotect);

            Assert.That(result.AuthorizationCode, Is.Null);
        }

        private string CreateBase64String(string responseType = null, string clientId = null, bool hasClientSecret = false, string clientSecret = null, Uri redirectUri = null, string[] scopes = null, bool hasExternalState = true, string externalState = null, bool hasAuthorizationCode = false, IAuthorizationCode authorizationCode = null)
        {
            return CreateSut(_fixture, _random, responseType, clientId, hasClientSecret, clientSecret, redirectUri, scopes, hasExternalState, externalState, hasAuthorizationCode, authorizationCode).ToString();
        }

        private static byte[] Unprotect(byte[] bytes)
        {
            NullGuard.NotNull(bytes, nameof(bytes));

            return bytes;
        }
    }
}