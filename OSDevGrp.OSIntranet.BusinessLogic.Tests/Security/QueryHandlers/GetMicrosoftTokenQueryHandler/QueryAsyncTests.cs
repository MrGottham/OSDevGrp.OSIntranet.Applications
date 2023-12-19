using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.QueryHandlers.GetMicrosoftTokenQueryHandler
{
	[TestFixture]
	public class QueryAsyncTests
	{
		#region Private variables

		private Mock<IClaimResolver> _claimResolverMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_claimResolverMock = new Mock<IClaimResolver>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
		{
			IQueryHandler<IGetMicrosoftTokenQuery, IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result!.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertClaimsPrincipalWasCalledOnGetMicrosoftTokenQuery()
		{
			IQueryHandler<IGetMicrosoftTokenQuery, IRefreshableToken> sut = CreateSut();

			Mock<IGetMicrosoftTokenQuery> getMicrosoftTokenQueryMock = CreateGetMicrosoftTokenQueryMock();
			await sut.QueryAsync(getMicrosoftTokenQueryMock.Object);

			getMicrosoftTokenQueryMock.Verify(m => m.ClaimsPrincipal, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertUnprotectWasCalledOnGetMicrosoftTokenQuery()
		{
			IQueryHandler<IGetMicrosoftTokenQuery, IRefreshableToken> sut = CreateSut();

			Mock<IGetMicrosoftTokenQuery> getMicrosoftTokenQueryMock = CreateGetMicrosoftTokenQueryMock();
			await sut.QueryAsync(getMicrosoftTokenQueryMock.Object);

			getMicrosoftTokenQueryMock.Verify(m => m.Unprotect, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenClaimsPrincipalHasBeenSetOnGetMicrosoftTokenQuery_AssertGetMicrosoftTokenWasCalledOnClaimResolverWithClaimsPrincipal()
		{
			IQueryHandler<IGetMicrosoftTokenQuery, IRefreshableToken> sut = CreateSut();

			ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
			Func<string, string> unprotect = value => value;
			IGetMicrosoftTokenQuery getMicrosoftTokenQuery = CreateGetMicrosoftTokenQuery(hasClaimsPrincipal: true, claimsPrincipal: claimsPrincipal, unprotect: unprotect);
			await sut.QueryAsync(getMicrosoftTokenQuery);

			_claimResolverMock.Verify(m => m.GetMicrosoftToken(
					It.Is<IPrincipal>(value => value != null && value == claimsPrincipal),
					It.Is<Func<string, string>>(value => value != null && value == unprotect)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenClaimsPrincipalHasBeenSetOnGetMicrosoftTokenQuery_AssertGetMicrosoftTokenWasNotCalledOnClaimResolverWithoutClaimsPrincipal()
		{
			IQueryHandler<IGetMicrosoftTokenQuery, IRefreshableToken> sut = CreateSut();

			IGetMicrosoftTokenQuery getMicrosoftTokenQuery = CreateGetMicrosoftTokenQuery(hasClaimsPrincipal: true);
			await sut.QueryAsync(getMicrosoftTokenQuery);

			_claimResolverMock.Verify(m => m.GetMicrosoftToken(It.IsAny<Func<string, string>>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenClaimsPrincipalHasNotBeenSetOnGetMicrosoftTokenQuery_AssertGetMicrosoftTokenWasNotCalledOnClaimResolverWithClaimsPrincipal()
		{
			IQueryHandler<IGetMicrosoftTokenQuery, IRefreshableToken> sut = CreateSut();

			IGetMicrosoftTokenQuery getMicrosoftTokenQuery = CreateGetMicrosoftTokenQuery(hasClaimsPrincipal: false);
			await sut.QueryAsync(getMicrosoftTokenQuery);

			_claimResolverMock.Verify(m => m.GetMicrosoftToken(
					It.IsAny<IPrincipal>(),
					It.IsAny<Func<string, string>>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenClaimsPrincipalHasNotBeenSetOnGetMicrosoftTokenQuery_AssertGetMicrosoftTokenWasCalledOnClaimResolverWithoutClaimsPrincipal()
		{
			IQueryHandler<IGetMicrosoftTokenQuery, IRefreshableToken> sut = CreateSut();

			Func<string, string> unprotect = value => value;
			IGetMicrosoftTokenQuery getMicrosoftTokenQuery = CreateGetMicrosoftTokenQuery(hasClaimsPrincipal: false, unprotect: unprotect);
			await sut.QueryAsync(getMicrosoftTokenQuery);

			_claimResolverMock.Verify(m => m.GetMicrosoftToken(It.Is<Func<string, string>>(value => value != null && value == unprotect)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task QueryAsync_WhenNoMicrosoftTokenWasReturned_ReturnsNull(bool hasClaimsPrincipal)
		{
			IQueryHandler<IGetMicrosoftTokenQuery, IRefreshableToken> sut = CreateSut(hasMicrosoftToken: false);

			IGetMicrosoftTokenQuery getMicrosoftTokenQuery = CreateGetMicrosoftTokenQuery(hasClaimsPrincipal: hasClaimsPrincipal);
			IToken result = await sut.QueryAsync(getMicrosoftTokenQuery);

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task QueryAsync_WhenMicrosoftTokenWasReturned_ReturnsNotNull(bool hasClaimsPrincipal)
		{
			IQueryHandler<IGetMicrosoftTokenQuery, IRefreshableToken> sut = CreateSut(hasMicrosoftToken: true);

			IGetMicrosoftTokenQuery getMicrosoftTokenQuery = CreateGetMicrosoftTokenQuery(hasClaimsPrincipal: hasClaimsPrincipal);
			IToken result = await sut.QueryAsync(getMicrosoftTokenQuery);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task QueryAsync_WhenMicrosoftTokenWasReturned_ReturnsMicrosoftTokenFromClaimResolver(bool hasClaimsPrincipal)
		{
			IRefreshableToken microsoftToken = _fixture.BuildRefreshableTokenMock().Object;
			IQueryHandler<IGetMicrosoftTokenQuery, IRefreshableToken> sut = CreateSut(hasMicrosoftToken: true, microsoftToken: microsoftToken);

			IGetMicrosoftTokenQuery getMicrosoftTokenQuery = CreateGetMicrosoftTokenQuery(hasClaimsPrincipal: hasClaimsPrincipal);
			IToken result = await sut.QueryAsync(getMicrosoftTokenQuery);

			Assert.That(result, Is.EqualTo(microsoftToken));
		}

		private IQueryHandler<IGetMicrosoftTokenQuery, IRefreshableToken> CreateSut(bool hasMicrosoftToken = true, IRefreshableToken microsoftToken = null)
		{
			_claimResolverMock.Setup(m => m.GetMicrosoftToken(It.IsAny<Func<string, string>>()))
				.Returns(hasMicrosoftToken? microsoftToken ?? _fixture.BuildRefreshableTokenMock().Object : null);
			_claimResolverMock.Setup(m => m.GetMicrosoftToken(It.IsAny<IPrincipal>(), It.IsAny<Func<string, string>>()))
				.Returns(hasMicrosoftToken ? microsoftToken ?? _fixture.BuildRefreshableTokenMock().Object : null);

			return new BusinessLogic.Security.QueryHandlers.GetMicrosoftTokenQueryHandler(_claimResolverMock.Object);
		}

		private IGetMicrosoftTokenQuery CreateGetMicrosoftTokenQuery(bool hasClaimsPrincipal = true, ClaimsPrincipal claimsPrincipal = null, Func<string, string> unprotect = null)
		{
			return CreateGetMicrosoftTokenQueryMock(hasClaimsPrincipal, claimsPrincipal, unprotect).Object;
		}

		private Mock<IGetMicrosoftTokenQuery> CreateGetMicrosoftTokenQueryMock(bool hasClaimsPrincipal = true, ClaimsPrincipal claimsPrincipal = null, Func<string, string> unprotect = null)
		{
			unprotect ??= value => value;

			Mock<IGetMicrosoftTokenQuery> getMicrosoftTokenQueryMock = new Mock<IGetMicrosoftTokenQuery>();
			getMicrosoftTokenQueryMock.Setup(m => m.ClaimsPrincipal)
				.Returns(hasClaimsPrincipal ? claimsPrincipal ?? new ClaimsPrincipal() : null);
			getMicrosoftTokenQueryMock.Setup(m => m.Unprotect)
				.Returns(unprotect);
			return getMicrosoftTokenQueryMock;
		}
	}
}