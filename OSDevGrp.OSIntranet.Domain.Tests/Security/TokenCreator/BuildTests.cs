using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.TokenCreator
{
	[TestFixture]
	public class BuildTests
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
		public void Build_WhenTokenTypeIsNotSetWithTokenType_ThrowsIntranetSystemException()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenTokenTypeIsNotSetWithTokenType_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToValueNotSetByNamedMethod()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotSetByNamedMethod));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenTokenTypeIsNotSetWithTokenType_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenTokenTypeIsNotSetWithTokenType_ThrowsIntranetSystemExceptionWhereMessageContainsWithTokenType()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message.Contains("'WithTokenType'"), Is.True);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenTokenTypeIsNotSetWithTokenType_ThrowsIntranetSystemExceptionWhereMessageContainsTypeNameForTokenBuilder()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message.Contains($"'{sut.GetType().Name}'"), Is.True);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAccessTokenIsNotSetWithAccessToken_ThrowsIntranetSystemException()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAccessTokenIsNotSetWithAccessToken_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToValueNotSetByNamedMethod()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotSetByNamedMethod));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAccessTokenIsNotSetWithAccessToken_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAccessTokenIsNotSetWithAccessToken_ThrowsIntranetSystemExceptionWhereMessageContainsWithAccessToken()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message.Contains("'WithAccessToken'"), Is.True);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAccessTokenIsNotSetWithAccessToken_ThrowsIntranetSystemExceptionWhereMessageContainsTypeNameForTokenBuilder()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message.Contains($"'{sut.GetType().Name}'"), Is.True);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAccessTokenIsNotSetWithExpires_ThrowsIntranetSystemException()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAccessTokenIsNotSetWithExpires_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToValueNotSetByNamedMethod()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueNotSetByNamedMethod));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAccessTokenIsNotSetWithExpires_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAccessTokenIsNotSetWithExpires_ThrowsIntranetSystemExceptionWhereMessageContainsWithExpires()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message.Contains("'WithExpires'"), Is.True);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAccessTokenIsNotSetWithExpires_ThrowsIntranetSystemExceptionWhereMessageContainsTypeNameForTokenBuilder()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>());

			IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Build());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message.Contains($"'{sut.GetType().Name}'"), Is.True);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAllTokenValuesItSet_ReturnsNotNull()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IToken result = sut.Build();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAllTokenValuesItSet_ReturnsTokenWhereTokenTypeIsNotNull()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IToken result = sut.Build();

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAllTokenValuesItSet_ReturnsTokenWhereTokenTypeIsNotEmpty()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IToken result = sut.Build();

			Assert.That(result.TokenType, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAllTokenValuesItSet_ReturnsTokenWhereTokenTypeIsEqualToValueFromWithTokenType()
		{
			string tokenType = _fixture.Create<string>();
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(tokenType)
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IToken result = sut.Build();

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAllTokenValuesItSet_ReturnsTokenWhereAccessTokenIsNotNull()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IToken result = sut.Build();

			Assert.That(result.AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAllTokenValuesItSet_ReturnsTokenWhereAccessTokenIsNotEmpty()
		{
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IToken result = sut.Build();

			Assert.That(result.AccessToken, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAllTokenValuesItSet_ReturnsTokenWhereAccessTokenIsEqualToValueFromWithAccessToken()
		{
			string accessToken = _fixture.Create<string>();
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(accessToken)
				.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			IToken result = sut.Build();

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAllTokenValuesItSet_ReturnsTokenWhereExpiresIsEqualToValueFromWithExpires()
		{
			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			ITokenBuilder<IToken> sut = CreateSut()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(expires);

			IToken result = sut.Build();

			Assert.That(result.Expires, Is.EqualTo(expires));
		}

		private static ITokenBuilder<IToken> CreateSut()
		{
			return new Domain.Security.TokenCreator();
		}
	}
}