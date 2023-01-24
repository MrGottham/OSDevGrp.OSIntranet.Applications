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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMovieGenreQueryHandler
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
            IQueryHandler<IGetMovieGenreQuery, IMovieGenre> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetMovieGenreQuery()
        {
            IQueryHandler<IGetMovieGenreQuery, IMovieGenre> sut = CreateSut();

            Mock<IGetMovieGenreQuery> getMovieGenreQueryMock = CreateGetMovieGenreQueryMock();
            await sut.QueryAsync(getMovieGenreQueryMock.Object);

            getMovieGenreQueryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberWasCalledOnGetMovieGenreQuery()
        {
            IQueryHandler<IGetMovieGenreQuery, IMovieGenre> sut = CreateSut();

            Mock<IGetMovieGenreQuery> getMovieGenreQueryMock = CreateGetMovieGenreQueryMock();
            await sut.QueryAsync(getMovieGenreQueryMock.Object);

            getMovieGenreQueryMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetMovieGenreAsyncWasCalledOnMediaLibraryRepository()
        {
            IQueryHandler<IGetMovieGenreQuery, IMovieGenre> sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.QueryAsync(CreateGetMovieGenreQuery(number));

            _mediaLibraryRepositoryMock.Verify(m => m.GetMovieGenreAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMovieGenreWasReturnedFromMediaLibraryRepository_ReturnsNotNull()
        {
            IQueryHandler<IGetMovieGenreQuery, IMovieGenre> sut = CreateSut();

            IMovieGenre result = await sut.QueryAsync(CreateGetMovieGenreQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMovieGenreWasReturnedFromMediaLibraryRepository_ReturnsMovieGenreFromMediaLibraryRepository()
        {
            IMovieGenre movieGenre = _fixture.BuildMovieGenreMock().Object;
            IQueryHandler<IGetMovieGenreQuery, IMovieGenre> sut = CreateSut(movieGenre: movieGenre);

            IMovieGenre result = await sut.QueryAsync(CreateGetMovieGenreQuery());

            Assert.That(result, Is.EqualTo(movieGenre));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMovieGenreWasReturnedFromMediaLibraryRepository_ReturnsNull()
        {
            IQueryHandler<IGetMovieGenreQuery, IMovieGenre> sut = CreateSut(false);

            IMovieGenre result = await sut.QueryAsync(CreateGetMovieGenreQuery());

            Assert.That(result, Is.Null);
        }

        private IQueryHandler<IGetMovieGenreQuery, IMovieGenre> CreateSut(bool hasMovieGenre = true, IMovieGenre movieGenre = null)
        {
            _mediaLibraryRepositoryMock.Setup(m => m.GetMovieGenreAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(hasMovieGenre ? movieGenre ?? _fixture.BuildMovieGenreMock().Object : null));

            return new BusinessLogic.MediaLibrary.QueryHandlers.GetMovieGenreQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
        }

        private IGetMovieGenreQuery CreateGetMovieGenreQuery(int? number = null)
        {
            return CreateGetMovieGenreQueryMock(number).Object;
        }

        private Mock<IGetMovieGenreQuery> CreateGetMovieGenreQueryMock(int? number = null)
        {
            Mock<IGetMovieGenreQuery> getMovieGenreQueryMock = new Mock<IGetMovieGenreQuery>();
            getMovieGenreQueryMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return getMovieGenreQueryMock;
        }
    }
}