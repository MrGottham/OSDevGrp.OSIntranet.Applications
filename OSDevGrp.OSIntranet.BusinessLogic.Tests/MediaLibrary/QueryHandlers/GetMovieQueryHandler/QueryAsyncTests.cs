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
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMovieQueryHandler
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
			IQueryHandler<IGetMovieQuery, IMovie> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetMovieQuery()
		{
			IQueryHandler<IGetMovieQuery, IMovie> sut = CreateSut();

			Mock<IGetMovieQuery> getMovieQueryMock = CreateGetMovieQueryMock();
			await sut.QueryAsync(getMovieQueryMock.Object);

			getMovieQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertMediaIdentifierWasCalledOnGetMovieQuery()
		{
			IQueryHandler<IGetMovieQuery, IMovie> sut = CreateSut();

			Mock<IGetMovieQuery> getMovieQueryMock = CreateGetMovieQueryMock();
			await sut.QueryAsync(getMovieQueryMock.Object);

			getMovieQueryMock.Verify(m => m.MediaIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertGetMediaAsyncWasCalledOnMediaLibraryRepository()
		{
			IQueryHandler<IGetMovieQuery, IMovie> sut = CreateSut();

			Guid mediaIdentifier = Guid.NewGuid();
			IGetMovieQuery getMovieQuery = CreateGetMovieQuery(mediaIdentifier);
			await sut.QueryAsync(getMovieQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMovie>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMovieHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetMovieQuery, IMovie> sut = CreateSut();

			IMovie result = await sut.QueryAsync(CreateGetMovieQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMovieHasBeenReturnedFromMediaLibraryRepository_ReturnsMovieFromMediaLibraryRepository()
		{
			IMovie movie = _fixture.BuildMovieMock().Object;
			IQueryHandler<IGetMovieQuery, IMovie> sut = CreateSut(movie: movie);

			IMovie result = await sut.QueryAsync(CreateGetMovieQuery());

			Assert.That(result, Is.EqualTo(movie));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMovieHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNull()
		{
			IQueryHandler<IGetMovieQuery, IMovie> sut = CreateSut(false);

			IMovie result = await sut.QueryAsync(CreateGetMovieQuery());

			Assert.That(result, Is.Null);
		}

		private IQueryHandler<IGetMovieQuery, IMovie> CreateSut(bool hasMovie = true, IMovie movie = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IMovie>(It.IsAny<Guid>()))
				.Returns(Task.FromResult(hasMovie ? movie ?? _fixture.BuildMovieMock().Object : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetMovieQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private static IGetMovieQuery CreateGetMovieQuery(Guid? mediaIdentifier = null)
		{
			return CreateGetMovieQueryMock(mediaIdentifier).Object;
		}

		private static Mock<IGetMovieQuery> CreateGetMovieQueryMock(Guid? mediaIdentifier = null)
		{
			Mock<IGetMovieQuery> getMovieQueryMock = new Mock<IGetMovieQuery>();
			getMovieQueryMock.Setup(m => m.MediaIdentifier)
				.Returns(mediaIdentifier ?? Guid.NewGuid());
			return getMovieQueryMock;
		}
	}
}