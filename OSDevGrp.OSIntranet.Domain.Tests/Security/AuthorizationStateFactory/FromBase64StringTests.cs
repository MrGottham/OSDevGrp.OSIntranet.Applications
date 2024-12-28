using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationStateFactory
{
    [TestFixture]
    public class FromBase64StringTests
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
            IAuthorizationStateFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromBase64String(null, bytes => bytes));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("base64String"));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenBase64StringIsEmpty_ThrowsArgumentNullException()
        {
            IAuthorizationStateFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromBase64String(string.Empty, bytes => bytes));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("base64String"));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenBase64StringIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAuthorizationStateFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromBase64String(" ", bytes => bytes));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("base64String"));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenUnprotectIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationStateFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromBase64String(CreateBase64String(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("unprotect"));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_AssertUnprotectWasCalled()
        {
            IAuthorizationStateFactory sut = CreateSut();

            bool unprotectWasCalled = false;
            sut.FromBase64String(CreateBase64String(), _ =>
            {
                unprotectWasCalled = true;
                return CreateByteArrayForAuthorizationState();
            });

            Assert.That(unprotectWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_AssertUnprotectWasCalledWithBytesForBase64String()
        {
            IAuthorizationStateFactory sut = CreateSut();

            string base64String = CreateBase64String();

            byte[] unprotectCalledWithBytes = [];
            sut .FromBase64String(base64String, bytes =>
            {
                unprotectCalledWithBytes = bytes;
                return CreateByteArrayForAuthorizationState();
            });

            Assert.That(Convert.ToBase64String(unprotectCalledWithBytes), Is.EqualTo(base64String));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            IAuthorizationState result =  sut.FromBase64String(CreateBase64String(), _ => CreateByteArrayForAuthorizationState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationState()
        {
            IAuthorizationStateFactory sut = CreateSut();

            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => CreateByteArrayForAuthorizationState());

            Assert.That(result, Is.TypeOf<Domain.Security.AuthorizationState>());
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereResponseTypeIsNotNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => CreateByteArrayForAuthorizationState());

            Assert.That(result.ResponseType, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereResponseTypeIsNotEmpty()
        {
            IAuthorizationStateFactory sut = CreateSut();

            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => CreateByteArrayForAuthorizationState());

            Assert.That(result.ResponseType, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereResponseTypeIsEqualToResponseTypeFromBase64String()
        {
            IAuthorizationStateFactory sut = CreateSut();

            string responseType = _fixture.Create<string>();
            string base64String = CreateBase64StringForAuthorizationState(responseType);
            IAuthorizationState result = sut.FromBase64String(base64String, bytes => bytes);

            Assert.That(result.ResponseType, Is.EqualTo(responseType));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereClientIdIsNotNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => CreateByteArrayForAuthorizationState());

            Assert.That(result.ClientId, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereClientIdIsNotEmpty()
        {
            IAuthorizationStateFactory sut = CreateSut();

            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => CreateByteArrayForAuthorizationState());

            Assert.That(result.ClientId, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereClientIdIsEqualToClientIdFromBase64String()
        {
            IAuthorizationStateFactory sut = CreateSut();

            string clientId = _fixture.Create<string>();
            string base64String = CreateBase64StringForAuthorizationState(clientId: clientId);
            IAuthorizationState result = sut.FromBase64String(base64String, bytes => bytes);

            Assert.That(result.ClientId, Is.EqualTo(clientId));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenClientSecretIsSetInBase64String_ReturnsAuthorizationStateWhereClientSecretIsNotNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasClientSecret: true);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.ClientSecret, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenClientSecretIsSetInBase64String_ReturnsAuthorizationStateWhereClientSecretIsNotEmpty()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasClientSecret: true);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.ClientSecret, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenClientSecretIsSetInBase64String_ReturnsAuthorizationStateWhereClientSecretIsEqualToClientSecretFromBase64String()
        {
            IAuthorizationStateFactory sut = CreateSut();

            string clientSecret = _fixture.Create<string>();
            string base64String = CreateBase64StringForAuthorizationState(hasClientSecret: true, clientSecret: clientSecret);
            IAuthorizationState result = sut.FromBase64String(base64String, bytes => bytes);

            Assert.That(result.ClientSecret, Is.EqualTo(clientSecret));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenClientSecretIsNotSetInBase64String_ReturnsAuthorizationStateWhereClientSecretIsNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasClientSecret: false);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.ClientSecret, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereRedirectUriIsNotNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => CreateByteArrayForAuthorizationState());

            Assert.That(result.RedirectUri, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereRedirectUriIsEqualToRedirectUriFromBase64String()
        {
            IAuthorizationStateFactory sut = CreateSut();

            Uri redirectUri = _fixture.CreateEndpoint();
            string base64String = CreateBase64StringForAuthorizationState(redirectUri: redirectUri);
            IAuthorizationState result = sut.FromBase64String(base64String, bytes => bytes);

            Assert.That(result.RedirectUri, Is.EqualTo(redirectUri));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereScopesIsNotNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => CreateByteArrayForAuthorizationState());

            Assert.That(result.Scopes, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereScopesIsNotEmpty()
        {
            IAuthorizationStateFactory sut = CreateSut();

            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => CreateByteArrayForAuthorizationState());

            Assert.That(result.Scopes, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenCalled_ReturnsAuthorizationStateWhereScopesIsEqualToScopesFromBase64String()
        {
            IAuthorizationStateFactory sut = CreateSut();

            string[] scopes = CreateScopes();
            string base64String = CreateBase64StringForAuthorizationState(scopes: scopes);
            IAuthorizationState result = sut.FromBase64String(base64String, bytes => bytes);

            Assert.That(result.Scopes, Is.EqualTo(scopes));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenExternalStateIsSetInBase64String_ReturnsAuthorizationStateWhereExternalStateIsNotNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasExternalState: true);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.ExternalState, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenExternalStateIsSetInBase64String_ReturnsAuthorizationStateWhereExternalStateIsNotEmpty()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasExternalState: true);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.ExternalState, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenExternalStateIsSetInBase64String_ReturnsAuthorizationStateWhereExternalStateIsEqualToExternalStateFromBase64String()
        {
            IAuthorizationStateFactory sut = CreateSut();

            string externalState = _fixture.Create<string>();
            string base64String = CreateBase64StringForAuthorizationState(hasExternalState: true, externalState: externalState);
            IAuthorizationState result = sut.FromBase64String(base64String, bytes => bytes);

            Assert.That(result.ExternalState, Is.EqualTo(externalState));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenExternalStateIsNotSetInBase64String_ReturnsAuthorizationStateWhereExternalStateIsNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasExternalState: false);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.ExternalState, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenNonceIsSetInBase64String_ReturnsAuthorizationStateWhereNonceIsNotNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasNonce: true);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.Nonce, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenNonceIsSetInBase64String_ReturnsAuthorizationStateWhereNonceIsNotEmpty()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasNonce: true);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.Nonce, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenNonceIsSetInBase64String_ReturnsAuthorizationStateWhereNonceIsEqualToNonceFromBase64String()
        {
            IAuthorizationStateFactory sut = CreateSut();

            string nonce = _fixture.Create<string>();
            string base64String = CreateBase64StringForAuthorizationState(hasNonce: true, nonce: nonce);
            IAuthorizationState result = sut.FromBase64String(base64String, bytes => bytes);

            Assert.That(result.Nonce, Is.EqualTo(nonce));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenNonceIsNotSetInBase64String_ReturnsAuthorizationStateWhereNonceIsNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasNonce: false);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.Nonce, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenAuthorizationCodeIsSetInBase64String_ReturnsAuthorizationStateWhereAuthorizationCodeIsNotNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasAuthorizationCode: true);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.AuthorizationCode, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenAuthorizationCodeIsSetInBase64String_ReturnsAuthorizationStateWhereValueOnAuthorizationCodeIsNotNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasAuthorizationCode: true);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.AuthorizationCode.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenAuthorizationCodeIsSetInBase64String_ReturnsAuthorizationStateWhereValueOnAuthorizationCodeIsNotEmpty()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasAuthorizationCode: true);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.AuthorizationCode.Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenAuthorizationCodeIsSetInBase64String_ReturnsAuthorizationStateWhereValueOnAuthorizationCodeIsEqualToValueOnAuthorizationCodeFromBase64String()
        {
            IAuthorizationStateFactory sut = CreateSut();

            string value = _fixture.Create<string>();
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(value: value).Object;
            string base64String = CreateBase64StringForAuthorizationState(hasAuthorizationCode: true, authorizationCode: authorizationCode);
            IAuthorizationState result = sut.FromBase64String(base64String, bytes => bytes);

            Assert.That(result.AuthorizationCode.Value, Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenAuthorizationCodeIsSetInBase64String_ReturnsAuthorizationStateWhereExpiresOnAuthorizationCodeIsEqualToExpiresOnAuthorizationCodeFromBase64String()
        {
            IAuthorizationStateFactory sut = CreateSut();

            DateTimeOffset expires = DateTimeOffset.Now.AddSeconds(_random.Next(5, 10));
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(expires: expires).Object;
            string base64String = CreateBase64StringForAuthorizationState(hasAuthorizationCode: true, authorizationCode: authorizationCode);
            IAuthorizationState result = sut.FromBase64String(base64String, bytes => bytes);

            Assert.That(result.AuthorizationCode.Expires, Is.EqualTo(expires.UtcDateTime));
        }

        [Test]
        [Category("UnitTest")]
        public void FromBase64String_WhenAuthorizationCodeIsNotSetInBase64String_ReturnsAuthorizationStateWhereAuthorizationCodeIsNull()
        {
            IAuthorizationStateFactory sut = CreateSut();

            byte[] byteArrayForAuthorizationState = CreateByteArrayForAuthorizationState(hasAuthorizationCode: false);
            IAuthorizationState result = sut.FromBase64String(CreateBase64String(), _ => byteArrayForAuthorizationState);

            Assert.That(result.AuthorizationCode, Is.Null);
        }

        private IAuthorizationStateFactory CreateSut()
        {
            return new Domain.Security.AuthorizationStateFactory();
        }

        private string CreateBase64String()
        {
            return Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray());
        }

        private string CreateBase64StringForAuthorizationState(string responseType = null, string clientId = null, bool hasClientSecret = false, string clientSecret = null, Uri redirectUri = null, string[] scopes = null, bool hasExternalState = true, string externalState = null, bool hasNonce = true, string nonce = null, bool hasAuthorizationCode = false, IAuthorizationCode authorizationCode = null)
        {
            return Convert.ToBase64String(CreateByteArrayForAuthorizationState(responseType, clientId, hasClientSecret, clientSecret, redirectUri, scopes, hasExternalState, externalState, hasNonce, nonce, hasAuthorizationCode, authorizationCode));
        }

        private byte[] CreateByteArrayForAuthorizationState(string responseType = null, string clientId = null, bool hasClientSecret = false, string clientSecret = null, Uri redirectUri = null, string[] scopes = null, bool hasExternalState = true, string externalState = null, bool hasNonce = true, string nonce = null, bool hasAuthorizationCode = false, IAuthorizationCode authorizationCode = null)
        {
            IAuthorizationState authorizationState = new Domain.Security.AuthorizationState(
                responseType ?? _fixture.Create<string>(),
                clientId ?? _fixture.Create<string>(),
                hasClientSecret ? clientSecret ?? _fixture.Create<string>() : null,
                redirectUri ?? _fixture.CreateEndpoint(),
                scopes ?? CreateScopes(),
                hasExternalState ? externalState ?? _fixture.Create<string>() : null,
                hasNonce ? nonce ?? _fixture.Create<string>() : null,
                hasAuthorizationCode ? authorizationCode ?? _fixture.BuildAuthorizationCodeMock().Object : null);
            return Convert.FromBase64String(authorizationState.ToString() ?? string.Empty);
        }

        private string[] CreateScopes()
        {
            return _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
        }
    }
}