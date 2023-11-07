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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.MediaLibraryFilterQueryHandlerBase
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
			IQueryHandler<IMediaLibraryFilterQuery, IEnumerable<object>> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnMediaLibraryFilterQuery()
		{
			IQueryHandler<IMediaLibraryFilterQuery, IEnumerable<object>> sut = CreateSut();

			Mock<IMediaLibraryFilterQuery> mediaLibraryFilterQueryMock = CreateMediaLibraryFilterQueryMock();
			await sut.QueryAsync(mediaLibraryFilterQueryMock.Object);

			mediaLibraryFilterQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenResultHasBeenReturned_ReturnsNotNull()
		{
			IQueryHandler<IMediaLibraryFilterQuery, IEnumerable<object>> sut = CreateSut(true);

			IEnumerable<object> result = await sut.QueryAsync(CreateMediaLibraryFilterQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenResultHasBeenReturned_ReturnsNotEmpty()
		{
			IQueryHandler<IMediaLibraryFilterQuery, IEnumerable<object>> sut = CreateSut(true);

			IEnumerable<object> result = await sut.QueryAsync(CreateMediaLibraryFilterQuery());

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenResultHasBeenReturned_ReturnsResult()
		{
			IEnumerable<object> objectCollection = _fixture.CreateMany<object>(_random.Next(1, 7)).ToArray();
			IQueryHandler<IMediaLibraryFilterQuery, IEnumerable<object>> sut = CreateSut(true, objectCollection);

			IEnumerable<object> result = await sut.QueryAsync(CreateMediaLibraryFilterQuery());

			Assert.That(result, Is.EqualTo(objectCollection));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenResultHasNotBeenReturned_ReturnsNotNull()
		{
			IQueryHandler<IMediaLibraryFilterQuery, IEnumerable<object>> sut = CreateSut(false);

			IEnumerable<object> result = await sut.QueryAsync(CreateMediaLibraryFilterQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenResultHasNotBeenReturned_ReturnsEmpty()
		{
			IQueryHandler<IMediaLibraryFilterQuery, IEnumerable<object>> sut = CreateSut(false);

			IEnumerable<object> result = await sut.QueryAsync(CreateMediaLibraryFilterQuery());

			Assert.That(result, Is.Empty);
		}

		private IQueryHandler<IMediaLibraryFilterQuery, IEnumerable<object>> CreateSut(bool? hasResult = null, IEnumerable<object> result = null)
		{
			return new MyMediaLibraryFilterQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object, hasResult ?? _random.Next(100) > 50, result ?? _fixture.CreateMany<object>(_random.Next(1, 7)).ToArray());
		}

		private static IMediaLibraryFilterQuery CreateMediaLibraryFilterQuery()
		{
			return CreateMediaLibraryFilterQueryMock().Object;
		}

		private static Mock<IMediaLibraryFilterQuery> CreateMediaLibraryFilterQueryMock()
		{
			return new Mock<IMediaLibraryFilterQuery>();
		}

		private class MyMediaLibraryFilterQueryHandler : MediaLibraryFilterQueryHandlerBase<IMediaLibraryFilterQuery, object>
		{
			#region Private variables

			private readonly bool _hasResult;
			private readonly IEnumerable<object> _result;

			#endregion

			#region Constructor

			public MyMediaLibraryFilterQueryHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository, bool hasResult, IEnumerable<object> result) 
				: base(validator, mediaLibraryRepository)
			{
				NullGuard.NotNull(result, nameof(result));

				_hasResult = hasResult;
				_result = result;
			}

			#endregion

			#region Methods

			protected override Task<IEnumerable<object>> GetResultAsync(IMediaLibraryFilterQuery query)
			{
				NullGuard.NotNull(query, nameof(query));

				return Task.FromResult(_hasResult ? _result : null);
			}

			#endregion
		}
	}
}