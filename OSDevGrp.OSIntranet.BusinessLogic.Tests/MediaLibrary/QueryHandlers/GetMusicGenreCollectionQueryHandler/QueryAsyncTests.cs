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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMusicGenreCollectionQueryHandler
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
            IQueryHandler<EmptyQuery, IEnumerable<IMusicGenre>> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetMusicGenresAsyncWasCalledOnMediaLibraryRepository()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMusicGenre>> sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _mediaLibraryRepositoryMock.Verify(m => m.GetMusicGenresAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMusicGenresWasReturnedFromMediaLibraryRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMusicGenre>> sut = CreateSut();

            IEnumerable<IMusicGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMusicGenresWasReturnedFromMediaLibraryRepository_ReturnsNonEmptyCollectionOfMusicGenres()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMusicGenre>> sut = CreateSut();

            IEnumerable<IMusicGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMusicGenresWasReturnedFromMediaLibraryRepository_ReturnsMusicGenreCollectionFromMediaLibraryRepository()
        {
            IMusicGenre[] musicGenreCollection = BuildMusicGenreCollection();
            IQueryHandler<EmptyQuery, IEnumerable<IMusicGenre>> sut = CreateSut(musicGenreCollection: musicGenreCollection);

            IEnumerable<IMusicGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(musicGenreCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMusicGenresWasReturnedFromMediaLibraryRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMusicGenre>> sut = CreateSut(false);

            IEnumerable<IMusicGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMusicGenresWasReturnedFromMediaLibraryRepository_ReturnsEmptyCollectionOfMusicGenres()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMusicGenre>> sut = CreateSut(false);

            IEnumerable<IMusicGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Empty);
        }

        public IQueryHandler<EmptyQuery, IEnumerable<IMusicGenre>> CreateSut(bool hasMusicGenreCollection = true, IEnumerable<IMusicGenre> musicGenreCollection = null)
        {
            _mediaLibraryRepositoryMock.Setup(m => m.GetMusicGenresAsync())
                .Returns(Task.FromResult(hasMusicGenreCollection ? musicGenreCollection ?? BuildMusicGenreCollection() : null));

            return new BusinessLogic.MediaLibrary.QueryHandlers.GetMusicGenreCollectionQueryHandler(_mediaLibraryRepositoryMock.Object);
        }

        private IMusicGenre[] BuildMusicGenreCollection()
        {
            return new[]
            {
                _fixture.BuildMusicGenreMock().Object,
                _fixture.BuildMusicGenreMock().Object,
                _fixture.BuildMusicGenreMock().Object,
                _fixture.BuildMusicGenreMock().Object,
                _fixture.BuildMusicGenreMock().Object
            };
        }
    }
}