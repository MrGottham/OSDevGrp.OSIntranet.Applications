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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetBookGenreCollectionQueryHandler
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
            IQueryHandler<EmptyQuery, IEnumerable<IBookGenre>> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetBookGenresAsyncWasCalledOnMediaLibraryRepository()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IBookGenre>> sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _mediaLibraryRepositoryMock.Verify(m => m.GetBookGenresAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBookGenresWasReturnedFromMediaLibraryRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IBookGenre>> sut = CreateSut();

            IEnumerable<IBookGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBookGenresWasReturnedFromMediaLibraryRepository_ReturnsNonEmptyCollectionOfBookGenres()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IBookGenre>> sut = CreateSut();

            IEnumerable<IBookGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBookGenresWasReturnedFromMediaLibraryRepository_ReturnsBookGenreCollectionFromMediaLibraryRepository()
        {
            IBookGenre[] bookGenreCollection = BuildBookGenreCollection();
            IQueryHandler<EmptyQuery, IEnumerable<IBookGenre>> sut = CreateSut(bookGenreCollection: bookGenreCollection);

            IEnumerable<IBookGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(bookGenreCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBookGenresWasReturnedFromMediaLibraryRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IBookGenre>> sut = CreateSut(false);

            IEnumerable<IBookGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBookGenresWasReturnedFromMediaLibraryRepository_ReturnsEmptyCollectionOfBookGenres()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IBookGenre>> sut = CreateSut(false);

            IEnumerable<IBookGenre> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Empty);
        }

        public IQueryHandler<EmptyQuery, IEnumerable<IBookGenre>> CreateSut(bool hasBookGenreCollection = true, IEnumerable<IBookGenre> bookGenreCollection = null)
        {
            _mediaLibraryRepositoryMock.Setup(m => m.GetBookGenresAsync())
                .Returns(Task.FromResult(hasBookGenreCollection ? bookGenreCollection ?? BuildBookGenreCollection() : null));

            return new BusinessLogic.MediaLibrary.QueryHandlers.GetBookGenreCollectionQueryHandler(_mediaLibraryRepositoryMock.Object);
        }

        private IBookGenre[] BuildBookGenreCollection()
        {
            return new[]
            {
                _fixture.BuildBookGenreMock().Object,
                _fixture.BuildBookGenreMock().Object,
                _fixture.BuildBookGenreMock().Object,
                _fixture.BuildBookGenreMock().Object,
                _fixture.BuildBookGenreMock().Object
            };
        }
    }
}