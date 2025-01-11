using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.QueryHandlers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.MediaLibraryQueryHandlerBase
{
	[TestFixture]
	public class QueryAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
		{
			IQueryHandler<IMediaLibraryQuery, object> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnMediaLibraryQuery()
		{
			IQueryHandler<IMediaLibraryQuery, object> sut = CreateSut();

			Mock<IMediaLibraryQuery> mediaLibraryQueryMock = CreateMediaLibraryQueryMock();
			await sut.QueryAsync(mediaLibraryQueryMock.Object);

			mediaLibraryQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertGetResultAsyncWasCalledOnMediaLibraryQueryHandlerBase()
		{
			IQueryHandler<IMediaLibraryQuery, object> sut = CreateSut();

			await sut.QueryAsync(CreateMediaLibraryQuery());

			Assert.That(((MyMediaLibraryQueryHandler) sut).GetResultAsyncWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertGetResultAsyncWasCalledOnMediaLibraryQueryHandlerBaseWithSameMediaLibraryQuery()
		{
			IQueryHandler<IMediaLibraryQuery, object> sut = CreateSut();

			IMediaLibraryQuery mediaLibraryQuery = CreateMediaLibraryQuery();
			await sut.QueryAsync(mediaLibraryQuery);

			Assert.That(((MyMediaLibraryQueryHandler)sut).GetResultAsyncCalledWithMediaLibraryQuery, Is.SameAs(mediaLibraryQuery));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenGetResultAsyncReturnsResult_AssertGetDefaultWasCalledWasNotCalledOnMediaLibraryQueryHandlerBase()
		{
			IQueryHandler<IMediaLibraryQuery, object> sut = CreateSut(true);

			await sut.QueryAsync(CreateMediaLibraryQuery());

			Assert.That(((MyMediaLibraryQueryHandler)sut).GetDefaultWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenGetResultAsyncDoesNotReturnResult_AssertGetDefaultWasCalledWasCalledOnMediaLibraryQueryHandlerBase()
		{
			IQueryHandler<IMediaLibraryQuery, object> sut = CreateSut(false);

			await sut.QueryAsync(CreateMediaLibraryQuery());

			Assert.That(((MyMediaLibraryQueryHandler)sut).GetDefaultWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenGetResultAsyncReturnsResult_ReturnsNotNull()
		{
			IQueryHandler<IMediaLibraryQuery, object> sut = CreateSut(true);

			object result = await sut.QueryAsync(CreateMediaLibraryQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenGetResultAsyncReturnsResult_ReturnsResultFromGetResultAsync()
		{
			object obj = _fixture.Create<object>();
			IQueryHandler<IMediaLibraryQuery, object> sut = CreateSut(true, obj);

			object result = await sut.QueryAsync(CreateMediaLibraryQuery());

			Assert.That(result, Is.EqualTo(obj));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenGetResultAsyncDoesNotReturnResult_ReturnsDefault()
		{
			IQueryHandler<IMediaLibraryQuery, object> sut = CreateSut(false);

			object result = await sut.QueryAsync(CreateMediaLibraryQuery());

			Assert.That(result, Is.EqualTo(default(object)));
		}

		private IQueryHandler<IMediaLibraryQuery, object> CreateSut(bool? hasResult = null, object result = null)
		{
			return new MyMediaLibraryQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object, hasResult ?? _random.Next(100) > 50, result ?? _fixture.Create<object>());
		}

		private static IMediaLibraryQuery CreateMediaLibraryQuery()
		{
			return CreateMediaLibraryQueryMock().Object;
		}

		private static Mock<IMediaLibraryQuery> CreateMediaLibraryQueryMock()
		{
			return new Mock<IMediaLibraryQuery>();
		}

		private class MyMediaLibraryQueryHandler : MediaLibraryQueryHandlerBase<IMediaLibraryQuery, object>
		{
			#region Private variables

			private readonly bool _hasResult;
			private readonly object _result;

			#endregion

			#region Constructor

			public MyMediaLibraryQueryHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository, bool hasResult, object result) 
				: base(validator, mediaLibraryRepository)
			{
				NullGuard.NotNull(result, nameof(result));

				_hasResult = hasResult;
				_result = result;
			}

			#endregion

			#region Properties

			public bool GetResultAsyncWasCalled { get; private set; }

			public IMediaLibraryQuery GetResultAsyncCalledWithMediaLibraryQuery { get; private set; }

			public bool GetDefaultWasCalled { get; private set; }

			#endregion

			#region Methods

			protected override Task<object> GetResultAsync(IMediaLibraryQuery query)
			{
				NullGuard.NotNull(query, nameof(query));

				GetResultAsyncWasCalled = true;
				GetResultAsyncCalledWithMediaLibraryQuery = query;

				return Task.FromResult(_hasResult ? _result : null);
			}

			protected override object GetDefault()
			{
				GetDefaultWasCalled = true;

				return base.GetDefault();
			}

			#endregion
		}
	}
}