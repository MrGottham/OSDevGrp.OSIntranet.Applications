using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableTokenCreator
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
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromBase64String(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("base64String"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromBase64String_WhenBase64StringIsEmpty_ThrowsArgumentNullException()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromBase64String(string.Empty));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("base64String"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromBase64String_WhenBase64StringIsWhiteSpace_ThrowsArgumentNullException()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromBase64String(" "));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("base64String"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromBase64String_WhenCalled_ReturnsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableToken result = sut.FromBase64String(CreateBase64String());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromBase64String_WhenCalled_ReturnsRefreshableTokenWhereTokenTypeIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableToken result = sut.FromBase64String(CreateBase64String());

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromBase64String_WhenCalled_ReturnsRefreshableTokenWhereTokenTypeIsDeserialized()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			string tokenType = _fixture.Create<string>();
			string base64String = CreateBase64String(tokenType);
			IRefreshableToken result = sut.FromBase64String(base64String);

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void FromBase64String_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableToken result = sut.FromBase64String(CreateBase64String());

			Assert.That(result.AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromBase64String_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsDeserialized()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			string accessToken = _fixture.Create<string>();
			string base64String = CreateBase64String(accessToken: accessToken);
			IRefreshableToken result = sut.FromBase64String(base64String);

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromBase64String_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableToken result = sut.FromBase64String(CreateBase64String());

			Assert.That(result.RefreshToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromBase64String_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsDeserialized()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			string refreshToken = _fixture.Create<string>();
			string base64String = CreateBase64String(refreshToken: refreshToken);
			IRefreshableToken result = sut.FromBase64String(base64String);

			Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromBase64String_WhenCalled_ReturnsRefreshableTokenWhereExpiresIsDeserialized()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			string base64String = CreateBase64String(expires: expires);
			IRefreshableToken result = sut.FromBase64String(base64String);

			Assert.That(result.Expires, Is.EqualTo(expires));
		}

		private static ITokenCreator<IRefreshableToken> CreateSut()
		{
			return new Domain.Security.RefreshableTokenCreator();
		}

		private string CreateBase64String(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
		{
			return new Domain.Security.RefreshableToken(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), refreshToken ?? _fixture.Create<string>(), expires ?? DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime()).ToBase64String();
		}
	}
}