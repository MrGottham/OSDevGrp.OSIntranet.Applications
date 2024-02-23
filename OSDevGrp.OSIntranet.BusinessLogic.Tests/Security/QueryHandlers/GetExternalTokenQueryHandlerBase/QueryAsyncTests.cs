using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.QueryHandlers.GetExternalTokenQueryHandlerBase
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
			IQueryHandler<IGetExternalTokenQuery, IToken> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result!.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertClaimsPrincipalWasCalledOnGetExternalTokenQuery()
		{
			IQueryHandler<IGetExternalTokenQuery, IToken> sut = CreateSut();

			Mock<IGetExternalTokenQuery> getExternalTokenQueryMock = CreateGetExternalTokenQueryMock();
			await sut.QueryAsync(getExternalTokenQueryMock.Object);

			getExternalTokenQueryMock.Verify(m => m.ClaimsPrincipal, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertUnprotectWasCalledOnGetExternalTokenQuery()
		{
			IQueryHandler<IGetExternalTokenQuery, IToken> sut = CreateSut();

			Mock<IGetExternalTokenQuery> getExternalTokenQueryMock = CreateGetExternalTokenQueryMock();
			await sut.QueryAsync(getExternalTokenQueryMock.Object);

			getExternalTokenQueryMock.Verify(m => m.Unprotect, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenClaimsPrincipalHasBeenSetOnGetExternalTokenQuery_AssertGetExternalTokenWasCalledOnGetExternalTokenQueryHandlerBase()
		{
			IQueryHandler<IGetExternalTokenQuery, IToken> sut = CreateSut();

			IGetExternalTokenQuery getExternalTokenQuery = CreateGetExternalTokenQuery(hasClaimsPrincipal: true);
			await sut.QueryAsync(getExternalTokenQuery);

			Assert.That(((MyGetExternalTokenQueryHandler) sut).GetExternalTokenWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenClaimsPrincipalHasBeenSetOnGetExternalTokenQuery_AssertGetExternalTokenWasCalledOnGetExternalTokenQueryHandlerBaseWithClaimsPrincipalFromGetExternalTokenQuery()
		{
			IQueryHandler<IGetExternalTokenQuery, IToken> sut = CreateSut();

			ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
			IGetExternalTokenQuery getExternalTokenQuery = CreateGetExternalTokenQuery(hasClaimsPrincipal: true, claimsPrincipal: claimsPrincipal);
			await sut.QueryAsync(getExternalTokenQuery);

			Assert.That(((MyGetExternalTokenQueryHandler) sut).GetExternalTokenWasCalledWithClaimsPrincipal, Is.EqualTo(claimsPrincipal));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenClaimsPrincipalHasBeenSetOnGetExternalTokenQuery_AssertGetExternalTokenWasCalledOnGetExternalTokenQueryHandlerBaseWithUnprotectFromGetExternalTokenQuery()
		{
			IQueryHandler<IGetExternalTokenQuery, IToken> sut = CreateSut();

			Func<string, string> unprotect = value => value;
			IGetExternalTokenQuery getExternalTokenQuery = CreateGetExternalTokenQuery(hasClaimsPrincipal: true, unprotect: unprotect);
			await sut.QueryAsync(getExternalTokenQuery);

			Assert.That(((MyGetExternalTokenQueryHandler) sut).GetExternalTokenWasCalledWithUnprotect, Is.EqualTo(unprotect));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenClaimsPrincipalHasNotBeenSetOnGetExternalTokenQuery_AssertGetExternalTokenWasCalledOnGetExternalTokenQueryHandlerBase()
		{
			IQueryHandler<IGetExternalTokenQuery, IToken> sut = CreateSut();

			IGetExternalTokenQuery getExternalTokenQuery = CreateGetExternalTokenQuery(hasClaimsPrincipal: false);
			await sut.QueryAsync(getExternalTokenQuery);

			Assert.That(((MyGetExternalTokenQueryHandler) sut).GetExternalTokenWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenClaimsPrincipalHasNotBeenSetOnGetExternalTokenQuery_AssertGetExternalTokenWasCalledOnGetExternalTokenQueryHandlerBaseWithoutAnyClaimsPrincipal()
		{
			IQueryHandler<IGetExternalTokenQuery, IToken> sut = CreateSut();

			IGetExternalTokenQuery getExternalTokenQuery = CreateGetExternalTokenQuery(hasClaimsPrincipal: false);
			await sut.QueryAsync(getExternalTokenQuery);

			Assert.That(((MyGetExternalTokenQueryHandler) sut).GetExternalTokenWasCalledWithClaimsPrincipal, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenClaimsPrincipalHasNotBeenSetOnGetExternalTokenQuery_AssertGetExternalTokenWasCalledOnGetExternalTokenQueryHandlerBaseWithUnprotectFromGetExternalTokenQuery()
		{
			IQueryHandler<IGetExternalTokenQuery, IToken> sut = CreateSut();

			Func<string, string> unprotect = value => value;
			IGetExternalTokenQuery getExternalTokenQuery = CreateGetExternalTokenQuery(hasClaimsPrincipal: false, unprotect: unprotect);
			await sut.QueryAsync(getExternalTokenQuery);

			Assert.That(((MyGetExternalTokenQueryHandler)sut).GetExternalTokenWasCalledWithUnprotect, Is.EqualTo(unprotect));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task QueryAsync_WhenNoTokenWasReturned_ReturnsNull(bool hasClaimsPrincipal)
		{
			IQueryHandler<IGetExternalTokenQuery, IToken> sut = CreateSut(hasToken: false);

			IGetExternalTokenQuery getExternalTokenQuery = CreateGetExternalTokenQuery(hasClaimsPrincipal: hasClaimsPrincipal);
			IToken result = await sut.QueryAsync(getExternalTokenQuery);

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task QueryAsync_WhenTokenWasReturned_ReturnsNotNull(bool hasClaimsPrincipal)
		{
			IQueryHandler<IGetExternalTokenQuery, IToken> sut = CreateSut(hasToken: true);

			IGetExternalTokenQuery getExternalTokenQuery = CreateGetExternalTokenQuery(hasClaimsPrincipal: hasClaimsPrincipal);
			IToken result = await sut.QueryAsync(getExternalTokenQuery);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task QueryAsync_WhenTokenWasReturned_ReturnsTokenFromGetExternalTokenOnGetExternalTokenQueryHandlerBase(bool hasClaimsPrincipal)
		{
			IToken token = _fixture.BuildTokenMock().Object;
			IQueryHandler<IGetExternalTokenQuery, IToken> sut = CreateSut(hasToken: true, token: token);

			IGetExternalTokenQuery getExternalTokenQuery = CreateGetExternalTokenQuery(hasClaimsPrincipal: hasClaimsPrincipal);
			IToken result = await sut.QueryAsync(getExternalTokenQuery);

			Assert.That(result, Is.EqualTo(token));
		}

		private IQueryHandler<IGetExternalTokenQuery, IToken> CreateSut(bool hasToken = true, IToken token = null)
		{
			return new MyGetExternalTokenQueryHandler(hasToken, token ?? _fixture.BuildTokenMock().Object, _claimResolverMock.Object);
		}

		private IGetExternalTokenQuery CreateGetExternalTokenQuery(bool hasClaimsPrincipal = true, ClaimsPrincipal claimsPrincipal = null, Func<string, string> unprotect = null)
		{
			return CreateGetExternalTokenQueryMock(hasClaimsPrincipal, claimsPrincipal, unprotect).Object;
		}

		private Mock<IGetExternalTokenQuery> CreateGetExternalTokenQueryMock(bool hasClaimsPrincipal = true, ClaimsPrincipal claimsPrincipal = null, Func<string, string> unprotect = null)
		{
			unprotect ??= value => value;

			Mock<IGetExternalTokenQuery> getExternalTokenQueryMock = new Mock<IGetExternalTokenQuery>();
			getExternalTokenQueryMock.Setup(m => m.ClaimsPrincipal)
				.Returns(hasClaimsPrincipal ? claimsPrincipal ?? new ClaimsPrincipal() : null);
			getExternalTokenQueryMock.Setup(m => m.Unprotect)
				.Returns(unprotect);
			return getExternalTokenQueryMock;
		}

		private class MyGetExternalTokenQueryHandler : BusinessLogic.Security.QueryHandlers.GetExternalTokenQueryHandlerBase<IGetExternalTokenQuery, IToken>
		{
			#region Private variables

			private readonly bool _hasToken;
			private readonly IToken _token;

			#endregion

			#region Constructor

			public MyGetExternalTokenQueryHandler(bool hasToken, IToken token, IClaimResolver claimResolver)
				: base(claimResolver)
			{
				NullGuard.NotNull(token, nameof(token));

				_hasToken = hasToken;
				_token = token;
			}

			#endregion

			#region Properties

			public bool GetExternalTokenWasCalled { get; private set; }

			public ClaimsPrincipal GetExternalTokenWasCalledWithClaimsPrincipal { get; private set; }

			public Func<string, string> GetExternalTokenWasCalledWithUnprotect { get; private set; }

			#endregion

			#region Methods

			protected override IToken GetExternalToken(Func<string, string> unprotect)
			{
				NullGuard.NotNull(unprotect, nameof(unprotect));

				GetExternalTokenWasCalled = true;
				GetExternalTokenWasCalledWithUnprotect = unprotect;

				return _hasToken ? _token : default;
			}

			protected override IToken GetExternalToken(ClaimsPrincipal claimsPrincipal, Func<string, string> unprotect)
			{
				NullGuard.NotNull(claimsPrincipal, nameof(claimsPrincipal))
					.NotNull(unprotect, nameof(unprotect));

				GetExternalTokenWasCalled = true;
				GetExternalTokenWasCalledWithClaimsPrincipal = claimsPrincipal;
				GetExternalTokenWasCalledWithUnprotect = unprotect;

				return _hasToken ? _token : default;
			}

			#endregion
		}
	}
}