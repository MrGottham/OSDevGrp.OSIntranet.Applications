using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.TokenCreator
{
	[TestFixture]
	public class FromByteArrayTests
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
		public void FromByteArray_WhenByteArrayIsNull_ThrowsArgumentNullException()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromByteArray(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("byteArray"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsTokenWhereTokenTypeIsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			IToken result = sut.FromByteArray(CreateByteArray());

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsTokenWhereTokenTypeIsDeserialized()
		{
			ITokenCreator<IToken> sut = CreateSut();

			string tokenType = _fixture.Create<string>();
			byte[] byteArray = CreateByteArray(tokenType);
			IToken result = sut.FromByteArray(byteArray);

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsTokenWhereAccessTokenIsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			IToken result = sut.FromByteArray(CreateByteArray());

			Assert.That(result.AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsTokenWhereAccessTokenIsDeserialized()
		{
			ITokenCreator<IToken> sut = CreateSut();

			string accessToken = _fixture.Create<string>();
			byte[] byteArray = CreateByteArray(accessToken: accessToken);
			IToken result = sut.FromByteArray(byteArray);

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsTokenWhereExpiresIsDeserialized()
		{
			ITokenCreator<IToken> sut = CreateSut();

			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			byte[] byteArray = CreateByteArray(expires: expires);
			IToken result = sut.FromByteArray(byteArray);

			Assert.That(result.Expires, Is.EqualTo(expires));
		}

		private static ITokenCreator<IToken> CreateSut()
		{
			return new Domain.Security.TokenCreator();
		}

		private byte[] CreateByteArray(string tokenType = null, string accessToken = null, DateTime? expires = null)
		{
			return new Domain.Security.Token(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), expires ?? DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime()).ToByteArray();
		}
	}
}