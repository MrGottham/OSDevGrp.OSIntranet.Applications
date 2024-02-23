using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.Token
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
			IToken sut = CreateSut();

			string result = sut.ToBase64String();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsNonEmptyString()
		{
			IToken sut = CreateSut();

			string result = sut.ToBase64String();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsBase64String()
		{
			IToken sut = CreateSut();

			string result = sut.ToBase64String();

			Assert.That(result.IsBase64String(), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsByteArrayWhereTokenTypeIsNotNull()
		{
			IToken sut = CreateSut();

			string result = sut.ToBase64String();

			Assert.That(Deserialize(result).TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsByteArrayWhereTokenTypeIsSerialized()
		{
			string tokenType = _fixture.Create<string>();
			IToken sut = CreateSut(tokenType);

			string result = sut.ToBase64String();

			Assert.That(Deserialize(result).TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsByteArrayWhereAccessTokenIsNotNull()
		{
			IToken sut = CreateSut();

			string result = sut.ToBase64String();

			Assert.That(Deserialize(result).AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsByteArrayWhereAccessTokenIsSerialized()
		{
			string accessToken = _fixture.Create<string>();
			IToken sut = CreateSut(accessToken: accessToken);

			string result = sut.ToBase64String();

			Assert.That(Deserialize(result).AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void ToBase64String_WhenCalled_ReturnsByteArrayWhereExpiresIsSerialized()
		{
			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			IToken sut = CreateSut(expires: expires);

			string result = sut.ToBase64String();

			Assert.That(Deserialize(result).Expires, Is.EqualTo(expires));
		}

		private IToken CreateSut(string tokenType = null, string accessToken = null, DateTime? expires = null)
		{
			return new Domain.Security.Token(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), expires ?? DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());
		}

		private static IToken Deserialize(string base64String)
		{
			NullGuard.NotNullOrWhiteSpace(base64String, nameof(base64String));

			return TokenFactory.Create().FromBase64String(base64String);
		}
	}
}