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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMusicGenreQueryHandler
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
            IQueryHandler<IGetMusicGenreQuery, IMusicGenre> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetMusicGenreQuery()
        {
            IQueryHandler<IGetMusicGenreQuery, IMusicGenre> sut = CreateSut();

            Mock<IGetMusicGenreQuery> getMusicGenreQueryMock = CreateGetMusicGenreQueryMock();
            await sut.QueryAsync(getMusicGenreQueryMock.Object);

            getMusicGenreQueryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberWasCalledOnGetMusicGenreQuery()
        {
            IQueryHandler<IGetMusicGenreQuery, IMusicGenre> sut = CreateSut();

            Mock<IGetMusicGenreQuery> getMusicGenreQueryMock = CreateGetMusicGenreQueryMock();
            await sut.QueryAsync(getMusicGenreQueryMock.Object);

            getMusicGenreQueryMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetMusicGenreAsyncWasCalledOnMediaLibraryRepository()
        {
            IQueryHandler<IGetMusicGenreQuery, IMusicGenre> sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.QueryAsync(CreateGetMusicGenreQuery(number));

            _mediaLibraryRepositoryMock.Verify(m => m.GetMusicGenreAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMusicGenreWasReturnedFromMediaLibraryRepository_ReturnsNotNull()
        {
            IQueryHandler<IGetMusicGenreQuery, IMusicGenre> sut = CreateSut();

            IMusicGenre result = await sut.QueryAsync(CreateGetMusicGenreQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMusicGenreWasReturnedFromMediaLibraryRepository_ReturnsMusicGenreFromMediaLibraryRepository()
        {
            IMusicGenre musicGenre = _fixture.BuildMusicGenreMock().Object;
            IQueryHandler<IGetMusicGenreQuery, IMusicGenre> sut = CreateSut(musicGenre: musicGenre);

            IMusicGenre result = await sut.QueryAsync(CreateGetMusicGenreQuery());

            Assert.That(result, Is.EqualTo(musicGenre));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMusicGenreWasReturnedFromMediaLibraryRepository_ReturnsNull()
        {
            IQueryHandler<IGetMusicGenreQuery, IMusicGenre> sut = CreateSut(false);

            IMusicGenre result = await sut.QueryAsync(CreateGetMusicGenreQuery());

            Assert.That(result, Is.Null);
        }

        private IQueryHandler<IGetMusicGenreQuery, IMusicGenre> CreateSut(bool hasMusicGenre = true, IMusicGenre musicGenre = null)
        {
            _mediaLibraryRepositoryMock.Setup(m => m.GetMusicGenreAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(hasMusicGenre ? musicGenre?? _fixture.BuildMusicGenreMock().Object : null));

            return new BusinessLogic.MediaLibrary.QueryHandlers.GetMusicGenreQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
        }

        private IGetMusicGenreQuery CreateGetMusicGenreQuery(int? number = null)
        {
            return CreateGetMusicGenreQueryMock(number).Object;
        }

        private Mock<IGetMusicGenreQuery> CreateGetMusicGenreQueryMock(int? number = null)
        {
            Mock<IGetMusicGenreQuery> getMusicGenreQueryMock = new Mock<IGetMusicGenreQuery>();
            getMusicGenreQueryMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return getMusicGenreQueryMock;
        }
    }
}