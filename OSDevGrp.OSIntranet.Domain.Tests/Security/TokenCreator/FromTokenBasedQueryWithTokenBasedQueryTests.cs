using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.TokenCreator
{
	[TestFixture]
	public class FromTokenBasedQueryWithTokenBasedQueryTests
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
		public void FromTokenBasedQuery_WhenTokenBasedQueryIsNull_ThrowsArgumentNullException()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromTokenBasedQuery((ITokenBasedQuery) null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("tokenBasedQuery"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_AssertTokenTypeWasCalledOnTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<ITokenBasedQuery> tokenBasedQueryMock = CreateTokenBasedQueryMock();
			sut.FromTokenBasedQuery(tokenBasedQueryMock.Object);

			tokenBasedQueryMock.Verify(m => m.TokenType, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_AssertAccessTokenWasCalledOnTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<ITokenBasedQuery> tokenBasedQueryMock = CreateTokenBasedQueryMock();
			sut.FromTokenBasedQuery(tokenBasedQueryMock.Object);

			tokenBasedQueryMock.Verify(m => m.AccessToken, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_AssertExpiresWasCalledOnTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<ITokenBasedQuery> tokenBasedQueryMock = CreateTokenBasedQueryMock();
			sut.FromTokenBasedQuery(tokenBasedQueryMock.Object);

			tokenBasedQueryMock.Verify(m => m.Expires, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ITokenBasedQuery tokenBasedQuery = CreateTokenBasedQuery();
			IToken result = sut.FromTokenBasedQuery(tokenBasedQuery);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereTokenTypeIsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ITokenBasedQuery tokenBasedQuery = CreateTokenBasedQuery();
			IToken result = sut.FromTokenBasedQuery(tokenBasedQuery);

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereTokenTypeIsNotEmpty()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ITokenBasedQuery tokenBasedQuery = CreateTokenBasedQuery();
			IToken result = sut.FromTokenBasedQuery(tokenBasedQuery);

			Assert.That(result.TokenType, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereTokenTypeIsEqualToTokenTypeOnTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			string tokenType = _fixture.Create<string>();
			ITokenBasedQuery tokenBasedQuery = CreateTokenBasedQuery(tokenType);
			IToken result = sut.FromTokenBasedQuery(tokenBasedQuery);

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereAccessTokenIsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ITokenBasedQuery tokenBasedQuery = CreateTokenBasedQuery();
			IToken result = sut.FromTokenBasedQuery(tokenBasedQuery);

			Assert.That(result.AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereAccessTokenIsNotEmpty()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ITokenBasedQuery tokenBasedQuery = CreateTokenBasedQuery();
			IToken result = sut.FromTokenBasedQuery(tokenBasedQuery);

			Assert.That(result.AccessToken, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereAccessTokenIsEqualToAccessTokenOnTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			string accessToken = _fixture.Create<string>();
			ITokenBasedQuery tokenBasedQuery = CreateTokenBasedQuery(accessToken: accessToken);
			IToken result = sut.FromTokenBasedQuery(tokenBasedQuery);

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereExpiresIsEqualToExpiresOnTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			ITokenBasedQuery tokenBasedQuery = CreateTokenBasedQuery(expires: expires);
			IToken result = sut.FromTokenBasedQuery(tokenBasedQuery);

			Assert.That(result.Expires, Is.EqualTo(expires));
		}

		private static ITokenCreator<IToken> CreateSut()
		{
			return new Domain.Security.TokenCreator();
		}

		private ITokenBasedQuery CreateTokenBasedQuery(string tokenType = null, string accessToken = null, DateTime? expires = null)
		{
			return CreateTokenBasedQueryMock(tokenType, accessToken, expires).Object;
		}

		private Mock<ITokenBasedQuery> CreateTokenBasedQueryMock(string tokenType = null, string accessToken = null, DateTime? expires = null)
		{
			Mock<ITokenBasedQuery> tokenBasedQueryMock = new Mock<ITokenBasedQuery>();
			tokenBasedQueryMock.Setup(m => m.TokenType)
				.Returns(tokenType ?? _fixture.Create<string>());
			tokenBasedQueryMock.Setup(m => m.AccessToken)
				.Returns(accessToken ?? _fixture.Create<string>());
			tokenBasedQueryMock.Setup(m => m.Expires)
				.Returns(expires ?? DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());
			return tokenBasedQueryMock;
		}
	}
}