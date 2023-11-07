using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMovieGenreCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMovieGenre>> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetMovieGenresAsyncWasCalledOnMediaLibraryRepository()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMovieGenre>> sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _mediaLibraryRepositoryMock.Verify(m => m.GetMovieGenresAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMovieGenresWasReturnedFromMediaLibraryRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMovieGenre>> sut = CreateSut();

            IEnumerable<IMovieGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMovieGenresWasReturnedFromMediaLibraryRepository_ReturnsNonEmptyCollectionOfMovieGenres()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMovieGenre>> sut = CreateSut();

            IEnumerable<IMovieGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMovieGenresWasReturnedFromMediaLibraryRepository_ReturnsMovieGenreCollectionFromMediaLibraryRepository()
        {
            IMovieGenre[] movieGenreCollection = BuildMovieGenreCollection();
            IQueryHandler<EmptyQuery, IEnumerable<IMovieGenre>> sut = CreateSut(movieGenreCollection: movieGenreCollection);

            IEnumerable<IMovieGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(movieGenreCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMovieGenresWasReturnedFromMediaLibraryRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMovieGenre>> sut = CreateSut(false);

            IEnumerable<IMovieGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMovieGenresWasReturnedFromMediaLibraryRepository_ReturnsEmptyCollectionOfMovieGenres()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMovieGenre>> sut = CreateSut(false);

            IEnumerable<IMovieGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Empty);
        }

        public IQueryHandler<EmptyQuery, IEnumerable<IMovieGenre>> CreateSut(bool hasMovieGenreCollection = true, IEnumerable<IMovieGenre> movieGenreCollection = null)
        {
            _mediaLibraryRepositoryMock.Setup(m => m.GetMovieGenresAsync())
                .Returns(Task.FromResult(hasMovieGenreCollection? movieGenreCollection ?? BuildMovieGenreCollection() : null));

            return new BusinessLogic.MediaLibrary.QueryHandlers.GetMovieGenreCollectionQueryHandler(_mediaLibraryRepositoryMock.Object);
        }

        private IMovieGenre[] BuildMovieGenreCollection()
        {
            return new[]
            {
                _fixture.BuildMovieGenreMock().Object,
                _fixture.BuildMovieGenreMock().Object,
                _fixture.BuildMovieGenreMock().Object,
                _fixture.BuildMovieGenreMock().Object,
                _fixture.BuildMovieGenreMock().Object
            };
        }
    }
}