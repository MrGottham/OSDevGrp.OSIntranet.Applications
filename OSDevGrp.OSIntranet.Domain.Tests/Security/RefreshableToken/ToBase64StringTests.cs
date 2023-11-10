using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableToken
{
	[TestFixture]
    public class ToBase64StringTests
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
		public void ToBase64String_WhenCalled_ReturnsNotNull()
		{
			IRefreshableToken sut = CreateSut();

			string result = sut.ToBase64String();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsNonEmptyString()
		{
			IRefreshableToken sut = CreateSut();

			string result = sut.ToBase64String();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsBase64String()
		{
			IRefreshableToken sut = CreateSut();

			string result = sut.ToBase64String();

			Assert.That(result.IsBase64String(), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsByteArrayWhereTokenTypeIsNotNull()
		{
			IRefreshableToken sut = CreateSut();

			string result = sut.ToBase64String();

			Assert.That(Deserialize(result).TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsByteArrayWhereTokenTypeIsSerialized()
		{
			string tokenType = _fixture.Create<string>();
			IRefreshableToken sut = CreateSut(tokenType);

			string result = sut.ToBase64String();

			Assert.That(Deserialize(result).TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsByteArrayWhereAccessTokenIsNotNull()
		{
			IRefreshableToken sut = CreateSut();

			string result = sut.ToBase64String();

			Assert.That(Deserialize(result).AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsByteArrayWhereAccessTokenIsSerialized()
		{
			string accessToken = _fixture.Create<string>();
			IRefreshableToken sut = CreateSut(accessToken: accessToken);

			string result = sut.ToBase64String();

			Assert.That(Deserialize(result).AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsByteArrayWhereRefreshTokenIsNotNull()
		{
			IRefreshableToken sut = CreateSut();

			string result = sut.ToBase64String();

			Assert.That(Deserialize(result).RefreshToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsByteArrayWhereRefreshTokenIsSerialized()
		{
			string refreshToken = _fixture.Create<string>();
			IRefreshableToken sut = CreateSut(refreshToken: refreshToken);

			string result = sut.ToBase64String();

			Assert.That(Deserialize(result).RefreshToken, Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsByteArrayWhereExpiresIsSerialized()
		{
			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			IRefreshableToken sut = CreateSut(expires: expires);

			string result = sut.ToBase64String();

			Assert.That(Deserialize(result).Expires, Is.EqualTo(expires));
		}

		private IRefreshableToken CreateSut(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
		{
			return new Domain.Security.RefreshableToken(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), refreshToken ?? _fixture.Create<string>(), expires ?? DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());
		}

		private static IRefreshableToken Deserialize(string base64String)
		{
			NullGuard.NotNullOrWhiteSpace(base64String, nameof(base64String));

			return RefreshableTokenFactory.Create().FromBase64String(base64String);
		}
	}
}