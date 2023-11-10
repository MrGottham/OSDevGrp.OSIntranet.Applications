using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableTokenCreator
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
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromTokenBasedQuery(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("refreshableTokenBasedQuery"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_AssertTokenTypeWasCalledOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = CreateRefreshableTokenBasedQueryMock();
			sut.FromTokenBasedQuery(refreshableTokenBasedQueryMock.Object);

			refreshableTokenBasedQueryMock.Verify(m => m.TokenType, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_AssertAccessTokenWasCalledOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = CreateRefreshableTokenBasedQueryMock();
			sut.FromTokenBasedQuery(refreshableTokenBasedQueryMock.Object);

			refreshableTokenBasedQueryMock.Verify(m => m.AccessToken, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_AssertRefreshTokenWasCalledOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = CreateRefreshableTokenBasedQueryMock();
			sut.FromTokenBasedQuery(refreshableTokenBasedQueryMock.Object);

			refreshableTokenBasedQueryMock.Verify(m => m.RefreshToken, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_AssertExpiresWasCalledOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = CreateRefreshableTokenBasedQueryMock();
			sut.FromTokenBasedQuery(refreshableTokenBasedQueryMock.Object);

			refreshableTokenBasedQueryMock.Verify(m => m.Expires, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery();
			IRefreshableToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsRefreshableTokenWhereTokenTypeIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery();
			IRefreshableToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsRefreshableTokenWhereTokenTypeIsNotEmpty()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery();
			IRefreshableToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.TokenType, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsRefreshableTokenWhereTokenTypeIsEqualToTokenTypeOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			string tokenType = _fixture.Create<string>();
			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery(tokenType);
			IRefreshableToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery();
			IRefreshableToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsNotEmpty()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery();
			IRefreshableToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.AccessToken, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsEqualToAccessTokenOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			string accessToken = _fixture.Create<string>();
			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery(accessToken: accessToken);
			IRefreshableToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery();
			IRefreshableToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.RefreshToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsNotEmpty()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery();
			IRefreshableToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.RefreshToken, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsEqualToRefreshTokenOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			string refreshToken = _fixture.Create<string>();
			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery(refreshToken: refreshToken);
			IRefreshableToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedQuery_WhenCalled_ReturnsRefreshableTokenWhereExpiresIsEqualToExpiresOnRefreshableTokenBasedQuery()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			IRefreshableTokenBasedQuery refreshableTokenBasedQuery = CreateRefreshableTokenBasedQuery(expires: expires);
			IRefreshableToken result = sut.FromTokenBasedQuery(refreshableTokenBasedQuery);

			Assert.That(result.Expires, Is.EqualTo(expires));
		}

		private static ITokenCreator<IRefreshableToken> CreateSut()
		{
			return new Domain.Security.RefreshableTokenCreator();
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