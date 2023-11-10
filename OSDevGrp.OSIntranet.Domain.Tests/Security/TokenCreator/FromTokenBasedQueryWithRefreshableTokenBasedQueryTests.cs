using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.TokenCreator
{
	[TestFixture]
	public class FromTokenBasedQueryWithRefreshableTokenBasedQueryTests
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
		public void FromTokenBasedQuery_WhenRefreshableTokenBasedQueryIsNull_ThrowsArgumentNullException()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromTokenBasedQuery(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("refreshableTokenBasedQuery"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_AssertTokenTypeWasCalledOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = CreateRefreshableTokenBasedQueryMock();
			sut.FromTokenBasedQuery(refreshableTokenBasedQueryMock.Object);

			refreshableTokenBasedQueryMock.Verify(m => m.TokenType, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_AssertAccessTokenWasCalledOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = CreateRefreshableTokenBasedQueryMock();
			sut.FromTokenBasedQuery(refreshableTokenBasedQueryMock.Object);

			refreshableTokenBasedQueryMock.Verify(m => m.AccessToken, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_AssertRefreshTokenWasNotCalledOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = CreateRefreshableTokenBasedQueryMock();
			sut.FromTokenBasedQuery(refreshableTokenBasedQueryMock.Object);

			refreshableTokenBasedQueryMock.Verify(m => m.RefreshToken, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_AssertExpiresWasCalledOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = CreateRefreshableTokenBasedQueryMock();
			sut.FromTokenBasedQuery(refreshableTokenBasedQueryMock.Object);

			refreshableTokenBasedQueryMock.Verify(m => m.Expires, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery();
			IToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereTokenTypeIsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery();
			IToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereTokenTypeIsNotEmpty()
		{
			ITokenCreator<IToken> sut = CreateSut();

			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery();
			IToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.TokenType, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereTokenTypeIsEqualToTokenTypeOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			string tokenType = _fixture.Create<string>();
			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery(tokenType);
			IToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereAccessTokenIsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery();
			IToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereAccessTokenIsNotEmpty()
		{
			ITokenCreator<IToken> sut = CreateSut();

			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery();
			IToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.AccessToken, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereAccessTokenIsEqualToAccessTokenOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			string accessToken = _fixture.Create<string>();
			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery(accessToken: accessToken);
			IToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsTokenWhereExpiresIsEqualToExpiresOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IToken> sut = CreateSut();

			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery(expires: expires);
			IToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.Expires, Is.EqualTo(expires));
		}

		private static ITokenCreator<IToken> CreateSut()
		{
			return new Domain.Security.TokenCreator();
		}

		private IRefreshableTokenBasedQuery CreateRefreshableTokenBasedQuery(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
		{
			return CreateRefreshableTokenBasedQueryMock(tokenType, accessToken, refreshToken, expires).Object;
		}

		private Mock<IRefreshableTokenBasedQuery> CreateRefreshableTokenBasedQueryMock(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
		{
			Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = new Mock<IRefreshableTokenBasedQuery>();
			refreshableTokenBasedQueryMock.Setup(m => m.TokenType)
				.Returns(tokenType ?? _fixture.Create<string>());
			refreshableTokenBasedQueryMock.Setup(m => m.AccessToken)
				.Returns(accessToken ?? _fixture.Create<string>());
			refreshableTokenBasedQueryMock.Setup(m => m.RefreshToken)
				.Returns(refreshToken ?? _fixture.Create<string>());
			refreshableTokenBasedQueryMock.Setup(m => m.Expires)
				.Returns(expires ?? DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());
			return refreshableTokenBasedQueryMock;
		}
	}
}