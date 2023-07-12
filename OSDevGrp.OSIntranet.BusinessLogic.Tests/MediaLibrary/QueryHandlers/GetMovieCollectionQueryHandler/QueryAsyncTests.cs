using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMovieCollectionQueryHandler
{
	[TestFixture]
	public class QueryAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
		{
			IQueryHandler<IGetMovieCollectionQuery, IEnumerable<IMovie>> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetMovieCollectionQuery()
		{
			IQueryHandler<IGetMovieCollectionQuery, IEnumerable<IMovie>> sut = CreateSut();

			Mock<IGetMovieCollectionQuery> getMovieCollectionQueryMock = CreateGetMovieCollectionQueryMock();
			await sut.QueryAsync(getMovieCollectionQueryMock.Object);

			getMovieCollectionQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertFilterWasCalledOnGetMovieCollectionQuery()
		{
			IQueryHandler<IGetMovieCollectionQuery, IEnumerable<IMovie>> sut = CreateSut();

			Mock<IGetMovieCollectionQuery> getMovieCollectionQueryMock = CreateGetMovieCollectionQueryMock();
			await sut.QueryAsync(getMovieCollectionQueryMock.Object);

			getMovieCollectionQueryMock.Verify(m => m.Filter, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		public async Task QueryAsync_WhenFilterHasNotBeenSetOnGetMovieCollectionQuery_AssertGetMediasAsyncWasCalledOnMediaLibraryRepository(string filter)
		{
			IQueryHandler<IGetMovieCollectionQuery, IEnumerable<IMovie>> sut = CreateSut();

			IGetMovieCollectionQuery getMovieCollectionQuery = CreateGetMovieCollectionQuery(filter);
			await sut.QueryAsync(getMovieCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediasAsync<IMovie>(It.Is<string>(value => string.IsNullOrWhiteSpace(value))), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenFilterHasBeenSetOnGetMovieCollectionQuery_AssertGetMediasAsyncWasCalledOnMediaLibraryRepository()
		{
			IQueryHandler<IGetMovieCollectionQuery, IEnumerable<IMovie>> sut = CreateSut();

			string filter = _fixture.Create<string>();
			IGetMovieCollectionQuery getMovieCollectionQuery = CreateGetMovieCollectionQuery(filter);
			await sut.QueryAsync(getMovieCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediasAsync<IMovie>(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, filter) == 0)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMovieCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetMovieCollectionQuery, IEnumerable<IMovie>> sut = CreateSut();

			IEnumerable<IMovie> result = await sut.QueryAsync(CreateGetMovieCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMovieCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotEmpty()
		{
			IQueryHandler<IGetMovieCollectionQuery, IEnumerable<IMovie>> sut = CreateSut();

			IEnumerable<IMovie> result = await sut.QueryAsync(CreateGetMovieCollectionQuery());

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMovieCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsMovieCollectionFromMediaLibraryRepository()
		{
			IEnumerable<IMovie> movieCollection = new[]
			{
				_fixture.BuildMovieMock().Object,
				_fixture.BuildMovieMock().Object,
				_fixture.BuildMovieMock().Object,
				_fixture.BuildMovieMock().Object,
				_fixture.BuildMovieMock().Object
			};
			IQueryHandler<IGetMovieCollectionQuery, IEnumerable<IMovie>> sut = CreateSut(movieCollection: movieCollection);

			IEnumerable<IMovie> result = await sut.QueryAsync(CreateGetMovieCollectionQuery());

			Assert.That(result, Is.EqualTo(movieCollection));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMovieCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetMovieCollectionQuery, IEnumerable<IMovie>> sut = CreateSut(false);

			IEnumerable<IMovie> result = await sut.QueryAsync(CreateGetMovieCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMovieCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsEmpty()
		{
			IQueryHandler<IGetMovieCollectionQuery, IEnumerable<IMovie>> sut = CreateSut(false);

			IEnumerable<IMovie> result = await sut.QueryAsync(CreateGetMovieCollectionQuery());

			Assert.That(result, Is.Empty);
		}

		private IQueryHandler<IGetMovieCollectionQuery, IEnumerable<IMovie>> CreateSut(bool hasMovieCollection = true, IEnumerable<IMovie> movieCollection = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediasAsync<IMovie>(It.IsAny<string>()))
				.Returns(Task.FromResult(hasMovieCollection ? movieCollection ?? new[] {_fixture.BuildMovieMock().Object, _fixture.BuildMovieMock().Object, _fixture.BuildMovieMock().Object} : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetMovieCollectionQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private static IGetMovieCollectionQuery CreateGetMovieCollectionQuery(string filter = null)
		{
			return CreateGetMovieCollectionQueryMock(filter).Object;
		}

		private static Mock<IGetMovieCollectionQuery> CreateGetMovieCollectionQueryMock(string filter = null)
		{
			Mock<IGetMovieCollectionQuery> getMovieCollectionQueryMock = new Mock<IGetMovieCollectionQuery>();
			getMovieCollectionQueryMock.Setup(m => m.Filter)
				.Returns(string.IsNullOrWhiteSpace(filter) == false ? filter : null);
			return getMovieCollectionQueryMock;
		}
	}
}