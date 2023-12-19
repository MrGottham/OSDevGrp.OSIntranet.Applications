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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.QueryHandlers.GetGoogleTokenQueryHandler
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
			IQueryHandler<IGetGoogleTokenQuery, IToken> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result!.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertClaimsPrincipalWasCalledOnGetGoogleTokenQuery()
		{
			IQueryHandler<IGetGoogleTokenQuery, IToken> sut = CreateSut();

			Mock<IGetGoogleTokenQuery> getGoogleTokenQueryMock = CreateGetGoogleTokenQueryMock();
			await sut.QueryAsync(getGoogleTokenQueryMock.Object);

			getGoogleTokenQueryMock.Verify(m => m.ClaimsPrincipal, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertUnprotectWasCalledOnGetGoogleTokenQuery()
		{
			IQueryHandler<IGetGoogleTokenQuery, IToken> sut = CreateSut();

			Mock<IGetGoogleTokenQuery> getGoogleTokenQueryMock = CreateGetGoogleTokenQueryMock();
			await sut.QueryAsync(getGoogleTokenQueryMock.Object);

			getGoogleTokenQueryMock.Verify(m => m.Unprotect, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalledClaimsPrincipalHasBeenSetOnGetGoogleTokenQuery_AssertGetGoogleTokenWasCalledOnClaimResolverWithClaimsPrincipal()
		{
			IQueryHandler<IGetGoogleTokenQuery, IToken> sut = CreateSut();

			ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
			Func<string, string> unprotect = value => value;
			IGetGoogleTokenQuery getGoogleTokenQuery = CreateGetGoogleTokenQuery(hasClaimsPrincipal: true, claimsPrincipal: claimsPrincipal, unprotect: unprotect);
			await sut.QueryAsync(getGoogleTokenQuery);

			_claimResolverMock.Verify(m => m.GetGoogleToken(
					It.Is<IPrincipal>(value => value != null && value == claimsPrincipal),
					It.Is<Func<string, string>>(value => value != null && value == unprotect)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalledClaimsPrincipalHasBeenSetOnGetGoogleTokenQuery_AssertGetGoogleTokenWasNotCalledOnClaimResolverWithoutClaimsPrincipal()
		{
			IQueryHandler<IGetGoogleTokenQuery, IToken> sut = CreateSut();

			IGetGoogleTokenQuery getGoogleTokenQuery = CreateGetGoogleTokenQuery(hasClaimsPrincipal: true);
			await sut.QueryAsync(getGoogleTokenQuery);

			_claimResolverMock.Verify(m => m.GetGoogleToken(It.IsAny<Func<string, string>>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalledClaimsPrincipalHasNotBeenSetOnGetGoogleTokenQuery_AssertGetGoogleTokenWasNotCalledOnClaimResolverWithClaimsPrincipal()
		{
			IQueryHandler<IGetGoogleTokenQuery, IToken> sut = CreateSut();

			IGetGoogleTokenQuery getGoogleTokenQuery = CreateGetGoogleTokenQuery(hasClaimsPrincipal: false);
			await sut.QueryAsync(getGoogleTokenQuery);

			_claimResolverMock.Verify(m => m.GetGoogleToken(
					It.IsAny<IPrincipal>(),
					It.IsAny<Func<string, string>>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalledClaimsPrincipalHasBeenSetOnGetGoogleTokenQuery_AssertGetGoogleTokenWasCalledOnClaimResolverWithoutClaimsPrincipal()
		{
			IQueryHandler<IGetGoogleTokenQuery, IToken> sut = CreateSut();

			Func<string, string> unprotect = value => value;
			IGetGoogleTokenQuery getGoogleTokenQuery = CreateGetGoogleTokenQuery(hasClaimsPrincipal: false, unprotect: unprotect);
			await sut.QueryAsync(getGoogleTokenQuery);

			_claimResolverMock.Verify(m => m.GetGoogleToken(It.Is<Func<string, string>>(value => value != null && value == unprotect)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task QueryAsync_WhenNoGoogleTokenWasReturned_ReturnsNull(bool hasClaimsPrincipal)
		{
			IQueryHandler<IGetGoogleTokenQuery, IToken> sut = CreateSut(hasGoogleToken: false);

			IGetGoogleTokenQuery getGoogleTokenQuery = CreateGetGoogleTokenQuery(hasClaimsPrincipal: hasClaimsPrincipal);
			IToken result = await sut.QueryAsync(getGoogleTokenQuery);

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task QueryAsync_WhenGoogleTokenWasReturned_ReturnsNotNull(bool hasClaimsPrincipal)
		{
			IQueryHandler<IGetGoogleTokenQuery, IToken> sut = CreateSut(hasGoogleToken: true);

			IGetGoogleTokenQuery getGoogleTokenQuery = CreateGetGoogleTokenQuery(hasClaimsPrincipal: hasClaimsPrincipal);
			IToken result = await sut.QueryAsync(getGoogleTokenQuery);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task QueryAsync_WhenGoogleTokenWasReturned_ReturnsGoogleTokenFromClaimResolver(bool hasClaimsPrincipal)
		{
			IToken googleToken = _fixture.BuildTokenMock().Object;
			IQueryHandler<IGetGoogleTokenQuery, IToken> sut = CreateSut(hasGoogleToken: true, googleToken: googleToken);

			IGetGoogleTokenQuery getGoogleTokenQuery = CreateGetGoogleTokenQuery(hasClaimsPrincipal: hasClaimsPrincipal);
			IToken result = await sut.QueryAsync(getGoogleTokenQuery);

			Assert.That(result, Is.EqualTo(googleToken));
		}

		private IQueryHandler<IGetGoogleTokenQuery, IToken> CreateSut(bool hasGoogleToken = true, IToken googleToken = null)
		{
			_claimResolverMock.Setup(m => m.GetGoogleToken(It.IsAny<Func<string, string>>()))
				.Returns(hasGoogleToken ? googleToken ?? _fixture.BuildTokenMock().Object : null);
			_claimResolverMock.Setup(m => m.GetGoogleToken(It.IsAny<IPrincipal>(), It.IsAny<Func<string, string>>()))
				.Returns(hasGoogleToken ? googleToken ?? _fixture.BuildTokenMock().Object : null);

			return new BusinessLogic.Security.QueryHandlers.GetGoogleTokenQueryHandler(_claimResolverMock.Object);
		}

		private IGetGoogleTokenQuery CreateGetGoogleTokenQuery(bool hasClaimsPrincipal = true, ClaimsPrincipal claimsPrincipal = null, Func<string, string> unprotect = null)
		{
			return CreateGetGoogleTokenQueryMock(hasClaimsPrincipal, claimsPrincipal, unprotect).Object;
		}

		private Mock<IGetGoogleTokenQuery> CreateGetGoogleTokenQueryMock(bool hasClaimsPrincipal = true, ClaimsPrincipal claimsPrincipal = null, Func<string, string> unprotect = null)
		{
			unprotect ??= value => value;

			Mock<IGetGoogleTokenQuery> getGoogleTokenQueryMock = new Mock<IGetGoogleTokenQuery>();
			getGoogleTokenQueryMock.Setup(m => m.ClaimsPrincipal)
				.Returns(hasClaimsPrincipal ? claimsPrincipal ?? new ClaimsPrincipal() : null);
			getGoogleTokenQueryMock.Setup(m => m.Unprotect)
				.Returns(unprotect);
			return getGoogleTokenQueryMock;
		}
	}
}