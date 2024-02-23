using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthenticationSessionPreparer
{
	[TestFixture]
	public class PrepareAsyncTests
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
		public void PrepareAsync_WhenItemsIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.Security.Logic.AuthenticationSessionPreparer.PrepareAsync(null, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600))));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("items"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void PrepareAsync_WhenExternalTokenClaimTypeIsNull_ThrowsArgumentNullException()
		{
			IDictionary<string, string> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PrepareAsync(null, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600))));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("externalTokenClaimType"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void PrepareAsync_WhenExternalTokenClaimTypeIsEmpty_ThrowsArgumentNullException()
		{
			IDictionary<string, string> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PrepareAsync(string.Empty, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600))));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("externalTokenClaimType"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void PrepareAsync_WhenExternalTokenClaimTypeIsWhiteSpace_ThrowsArgumentNullException()
		{
			IDictionary<string, string> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PrepareAsync(" ", _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600))));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("externalTokenClaimType"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void PrepareAsync_WhenTokenTypeIsNull_ThrowsArgumentNullException()
		{
			IDictionary<string, string> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PrepareAsync(_fixture.Create<string>(), null, _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600))));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("tokenType"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void PrepareAsync_WhenTokenTypeIsEmpty_ThrowsArgumentNullException()
		{
			IDictionary<string, string> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PrepareAsync(_fixture.Create<string>(), string.Empty, _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600))));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("tokenType"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void PrepareAsync_WhenTokenTypeIsWhiteSpace_ThrowsArgumentNullException()
		{
			IDictionary<string, string> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PrepareAsync(_fixture.Create<string>(), " ", _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600))));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("tokenType"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void PrepareAsync_WhenAccessTokenIsNull_ThrowsArgumentNullException()
		{
			IDictionary<string, string> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), null, _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600))));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("accessToken"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void PrepareAsync_WhenAccessTokenIsEmpty_ThrowsArgumentNullException()
		{
			IDictionary<string, string> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), string.Empty, _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600))));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("accessToken"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void PrepareAsync_WhenAccessTokenIsWhiteSpace_ThrowsArgumentNullException()
		{
			IDictionary<string, string> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), " ", _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600))));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("accessToken"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenCalled_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut();

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenCalled_ReturnsSameDictionaryOfItems()
		{
			IDictionary<string, string> sut = CreateSut();

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result, Is.SameAs(sut));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenCalled_ReturnsNonEmptyDictionaryOfItems()
		{
			IDictionary<string, string> sut = CreateSut();

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenItemsAlreadyContainsKeyForExternalClaimType_ReturnsDictionaryContainingKeyForExternalClaimType()
		{
			IDictionary<string, string> sut = CreateSut(hasExternalTokenClaimType: true);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.ExternalTokenClaimTypeKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenItemsAlreadyContainsKeyForExternalClaimType_ReturnsDictionaryContainingExistingValueForExternalClaimType()
		{
			string externalTokenClaimType = _fixture.Create<string>();
			IDictionary<string, string> sut = CreateSut(hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result[AuthenticationSessionKeys.ExternalTokenClaimTypeKey], Is.EqualTo(externalTokenClaimType));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenItemsDoesNotKeyForExternalClaimType_ReturnsDictionaryContainingKeyForExternalClaimType()
		{
			IDictionary<string, string> sut = CreateSut(hasExternalTokenClaimType: false);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.ExternalTokenClaimTypeKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenItemsDoesNotKeyForExternalClaimType_ReturnsDictionaryContainingValueForExternalClaimType()
		{
			IDictionary<string, string> sut = CreateSut(hasExternalTokenClaimType: false);

			string externalTokenClaimType = _fixture.Create<string>();
			IDictionary<string, string> result = await sut.PrepareAsync(externalTokenClaimType, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result[AuthenticationSessionKeys.ExternalTokenClaimTypeKey], Is.EqualTo(externalTokenClaimType));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenItemsAlreadyContainsKeyForTokenType_ReturnsDictionaryContainingKeyForTokenType()
		{
			IDictionary<string, string> sut = CreateSut(hasTokenType: true);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.TokenTypeKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenItemsAlreadyContainsKeyForTokenType_ReturnsDictionaryContainingExistingValueForTokenType()
		{
			string tokenType = _fixture.Create<string>();
			IDictionary<string, string> sut = CreateSut(hasTokenType: true, tokenType: tokenType);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result[AuthenticationSessionKeys.TokenTypeKey], Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenItemsDoesNotKeyForTokenType_ReturnsDictionaryContainingKeyForTokenType()
		{
			IDictionary<string, string> sut = CreateSut(hasTokenType: false);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.TokenTypeKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenItemsDoesNotKeyForTokenType_ReturnsDictionaryContainingValueForTokenType()
		{
			IDictionary<string, string> sut = CreateSut(hasTokenType: false);

			string tokenType = _fixture.Create<string>();
			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), tokenType, _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result[AuthenticationSessionKeys.TokenTypeKey], Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenItemsAlreadyContainsKeyForAccessToken_ReturnsDictionaryContainingKeyForAccessToken()
		{
			IDictionary<string, string> sut = CreateSut(hasAccessToken: true);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.AccessTokenKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenItemsAlreadyContainsKeyForAccessToken_ReturnsDictionaryContainingExistingValueForAccessToken()
		{
			string accessToken = _fixture.Create<string>();
			IDictionary<string, string> sut = CreateSut(hasAccessToken: true, accessToken: accessToken);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result[AuthenticationSessionKeys.AccessTokenKey], Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenItemsDoesNotKeyForAccessToken_ReturnsDictionaryContainingKeyForAccessToken()
		{
			IDictionary<string, string> sut = CreateSut(hasAccessToken: false);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.AccessTokenKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenItemsDoesNotKeyForAccessToken_ReturnsDictionaryContainingValueForAccessToken()
		{
			IDictionary<string, string> sut = CreateSut(hasAccessToken: false);

			string accessToken = _fixture.Create<string>();
			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), accessToken, _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result[AuthenticationSessionKeys.AccessTokenKey], Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsNullAndItemsAlreadyContainsKeyForRefreshToken_ReturnsDictionaryContainingKeyForRefreshToken()
		{
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: true);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), null, TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.RefreshTokenKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsNullAndItemsAlreadyContainsKeyForRefreshToken_ReturnsDictionaryContainingExistingValueForRefreshToken()
		{
			string refreshToken = _fixture.Create<string>();
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: true, refreshToken: refreshToken);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), null, TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result[AuthenticationSessionKeys.RefreshTokenKey], Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsNullAndItemsDoesNotKeyForRefreshToken_ReturnsDictionaryNotContainingKeyForRefreshToken()
		{
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: false);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), null, TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.RefreshTokenKey), Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsEmptyAndItemsAlreadyContainsKeyForRefreshToken_ReturnsDictionaryContainingKeyForRefreshToken()
		{
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: true);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), string.Empty, TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.RefreshTokenKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsEmptyAndItemsAlreadyContainsKeyForRefreshToken_ReturnsDictionaryContainingExistingValueForRefreshToken()
		{
			string refreshToken = _fixture.Create<string>();
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: true, refreshToken: refreshToken);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), string.Empty, TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result[AuthenticationSessionKeys.RefreshTokenKey], Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsEmptyAndItemsDoesNotKeyForRefreshToken_ReturnsDictionaryNotContainingKeyForRefreshToken()
		{
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: false);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), string.Empty, TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.RefreshTokenKey), Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsWhiteSpaceAndItemsAlreadyContainsKeyForRefreshToken_ReturnsDictionaryContainingKeyForRefreshToken()
		{
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: true);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), " ", TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.RefreshTokenKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsWhiteSpaceAndItemsAlreadyContainsKeyForRefreshToken_ReturnsDictionaryContainingExistingValueForRefreshToken()
		{
			string refreshToken = _fixture.Create<string>();
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: true, refreshToken: refreshToken);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), " ", TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result[AuthenticationSessionKeys.RefreshTokenKey], Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsWhiteSpaceAndItemsDoesNotKeyForRefreshToken_ReturnsDictionaryNotContainingKeyForRefreshToken()
		{
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: false);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), " ", TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.RefreshTokenKey), Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsNotNullEmptyOrWhiteSpaceAndItemsAlreadyContainsKeyForRefreshToken_ReturnsDictionaryContainingKeyForRefreshToken()
		{
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: true);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.RefreshTokenKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsNotNullEmptyOrWhiteSpaceAndItemsAlreadyContainsKeyForRefreshToken_ReturnsDictionaryContainingExistingValueForRefreshToken()
		{
			string refreshToken = _fixture.Create<string>();
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: true, refreshToken: refreshToken);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result[AuthenticationSessionKeys.RefreshTokenKey], Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsNotNullEmptyOrWhiteSpaceAndItemsDoesNotKeyForRefreshToken_ReturnsDictionaryContainingKeyForRefreshToken()
		{
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: false);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.RefreshTokenKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenRefreshTokenIsNotNullEmptyOrWhiteSpaceAndItemsDoesNotKeyForRefreshToken_ReturnsDictionaryContainingValueForRefreshToken()
		{
			IDictionary<string, string> sut = CreateSut(hasRefreshToken: false);

			string refreshToken = _fixture.Create<string>();
			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), refreshToken, TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result[AuthenticationSessionKeys.RefreshTokenKey], Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenExpiresInIsNullAndItemsAlreadyContainsKeyForExpiresIn_ReturnsDictionaryContainingKeyForExpiresIn()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresIn: true);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), null);

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.ExpiresInKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenExpiresInIsNullAndItemsAlreadyContainsKeyForExpiresIn_ReturnsDictionaryContainingExistingValueForExpiresIn()
		{
			TimeSpan expiresIn = TimeSpan.FromSeconds(_random.Next(60, 3600));
			IDictionary<string, string> sut = CreateSut(hasExpiresIn: true, expiresIn: expiresIn);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), null);

			Assert.That(result[AuthenticationSessionKeys.ExpiresInKey], Is.EqualTo(expiresIn.TotalSeconds.ToString(CultureInfo.InvariantCulture)));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenExpiresInIsNullAndItemsDoesNotKeyForExpiresIn_ReturnsDictionaryNotContainingKeyForExpiresIn()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresIn: false);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), null);

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.ExpiresInKey), Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenExpiresInIsNotNullAndItemsAlreadyContainsKeyForExpiresIn_ReturnsDictionaryContainingKeyForExpiresIn()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresIn: true);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.ExpiresInKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenExpiresInIsNotNullAndItemsAlreadyContainsKeyForExpiresIn_ReturnsDictionaryContainingExistingValueForExpiresIn()
		{
			TimeSpan expiresIn = TimeSpan.FromSeconds(_random.Next(60, 3600));
			IDictionary<string, string> sut = CreateSut(hasExpiresIn: true, expiresIn: expiresIn);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result[AuthenticationSessionKeys.ExpiresInKey], Is.EqualTo(expiresIn.TotalSeconds.ToString(CultureInfo.InvariantCulture)));
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenExpiresInIsNotNullAndItemsDoesNotKeyForExpiresIn_ReturnsDictionaryContainingKeyForExpiresIn()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresIn: false);

			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), TimeSpan.FromSeconds(_random.Next(60, 3600)));

			Assert.That(result.ContainsKey(AuthenticationSessionKeys.ExpiresInKey), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task PrepareAsync_WhenExpiresInIsNotNullAndItemsDoesNotKeyForExpiresIn_ReturnsDictionaryContainingValueForExpiresIn()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresIn: false);

			TimeSpan expiresIn = TimeSpan.FromSeconds(_random.Next(60, 3600));
			IDictionary<string, string> result = await sut.PrepareAsync(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), expiresIn);

			Assert.That(result[AuthenticationSessionKeys.ExpiresInKey], Is.EqualTo(expiresIn.TotalSeconds.ToString(CultureInfo.InvariantCulture)));
		}

		private IDictionary<string, string> CreateSut(bool hasExternalTokenClaimType = true, string externalTokenClaimType = null, bool hasTokenType = true, string tokenType = null, bool hasAccessToken = true, string accessToken = null, bool hasRefreshToken = false, string refreshToken = null, bool hasExpiresIn = false, TimeSpan? expiresIn = null)
		{
			IDictionary<string, string> items = new ConcurrentDictionary<string, string>();

			if (hasExternalTokenClaimType)
			{
				items.Add(AuthenticationSessionKeys.ExternalTokenClaimTypeKey, externalTokenClaimType ?? _fixture.Create<string>());
			}

			if (hasTokenType)
			{
				items.Add(AuthenticationSessionKeys.TokenTypeKey, tokenType ?? _fixture.Create<string>());
			}

			if (hasAccessToken)
			{
				items.Add(AuthenticationSessionKeys.AccessTokenKey, accessToken ?? _fixture.Create<string>());
			}

			if (hasRefreshToken)
			{
				items.Add(AuthenticationSessionKeys.RefreshTokenKey, refreshToken ?? _fixture.Create<string>());
			}

			if (hasExpiresIn)
			{
				items.Add(AuthenticationSessionKeys.ExpiresInKey, (expiresIn ?? new TimeSpan(_random.Next(60, 3600))).TotalSeconds.ToString(CultureInfo.InvariantCulture));
			}

			return items;
		}
	}
}