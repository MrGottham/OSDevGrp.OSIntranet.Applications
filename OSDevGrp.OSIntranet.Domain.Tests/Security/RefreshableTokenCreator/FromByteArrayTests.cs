using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableTokenCreator
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
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromByteArray(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("byteArray"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableToken result = sut.FromByteArray(CreateByteArray());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsRefreshableTokenWhereTokenTypeIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableToken result = sut.FromByteArray(CreateByteArray());

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsRefreshableTokenWhereTokenTypeIsDeserialized()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			string tokenType = _fixture.Create<string>();
			byte[] byteArray = CreateByteArray(tokenType);
			IRefreshableToken result = sut.FromByteArray(byteArray);

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableToken result = sut.FromByteArray(CreateByteArray());

			Assert.That(result.AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsDeserialized()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			string accessToken = _fixture.Create<string>();
			byte[] byteArray = CreateByteArray(accessToken: accessToken);
			IRefreshableToken result = sut.FromByteArray(byteArray);

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableToken result = sut.FromByteArray(CreateByteArray());

			Assert.That(result.RefreshToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsDeserialized()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			string refreshToken = _fixture.Create<string>();
			byte[] byteArray = CreateByteArray(refreshToken: refreshToken);
			IRefreshableToken result = sut.FromByteArray(byteArray);

			Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromByteArray_WhenCalled_ReturnsRefreshableTokenWhereExpiresIsDeserialized()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			byte[] byteArray = CreateByteArray(expires: expires);
			IRefreshableToken result = sut.FromByteArray(byteArray);

			Assert.That(result.Expires, Is.EqualTo(expires));
		}

		private static ITokenCreator<IRefreshableToken> CreateSut()
		{
			return new Domain.Security.RefreshableTokenCreator();
		}

		private byte[] CreateByteArray(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
		{
			return new Domain.Security.RefreshableToken(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), refreshToken ?? _fixture.Create<string>(), expires ?? DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime()).ToByteArray();
		}
	}
}