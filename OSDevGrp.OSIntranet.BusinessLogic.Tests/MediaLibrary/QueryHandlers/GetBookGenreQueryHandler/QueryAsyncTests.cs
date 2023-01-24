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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetBookGenreQueryHandler
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
            IQueryHandler<IGetBookGenreQuery, IBookGenre> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetBookGenreQuery()
        {
            IQueryHandler<IGetBookGenreQuery, IBookGenre> sut = CreateSut();

            Mock<IGetBookGenreQuery> getBookGenreQueryMock = CreateGetBookGenreQueryMock();
            await sut.QueryAsync(getBookGenreQueryMock.Object);

            getBookGenreQueryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberWasCalledOnGetBookGenreQuery()
        {
            IQueryHandler<IGetBookGenreQuery, IBookGenre> sut = CreateSut();

            Mock<IGetBookGenreQuery> getBookGenreQueryMock = CreateGetBookGenreQueryMock();
            await sut.QueryAsync(getBookGenreQueryMock.Object);

            getBookGenreQueryMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetBookGenreAsyncWasCalledOnMediaLibraryRepository()
        {
            IQueryHandler<IGetBookGenreQuery, IBookGenre> sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.QueryAsync(CreateGetBookGenreQuery(number));

            _mediaLibraryRepositoryMock.Verify(m => m.GetBookGenreAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBookGenreWasReturnedFromMediaLibraryRepository_ReturnsNotNull()
        {
            IQueryHandler<IGetBookGenreQuery, IBookGenre> sut = CreateSut();

            IBookGenre result = await sut.QueryAsync(CreateGetBookGenreQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBookGenreWasReturnedFromMediaLibraryRepository_ReturnsBookGenreFromMediaLibraryRepository()
        {
            IBookGenre bookGenre = _fixture.BuildBookGenreMock().Object;
            IQueryHandler<IGetBookGenreQuery, IBookGenre> sut = CreateSut(bookGenre: bookGenre);

            IBookGenre result = await sut.QueryAsync(CreateGetBookGenreQuery());

            Assert.That(result, Is.EqualTo(bookGenre));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBookGenreWasReturnedFromMediaLibraryRepository_ReturnsNull()
        {
            IQueryHandler<IGetBookGenreQuery, IBookGenre> sut = CreateSut(false);

            IBookGenre result = await sut.QueryAsync(CreateGetBookGenreQuery());

            Assert.That(result, Is.Null);
        }

        private IQueryHandler<IGetBookGenreQuery, IBookGenre> CreateSut(bool hasBookGenre = true, IBookGenre bookGenre = null)
        {
            _mediaLibraryRepositoryMock.Setup(m => m.GetBookGenreAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(hasBookGenre ? bookGenre ?? _fixture.BuildBookGenreMock().Object : null));

            return new BusinessLogic.MediaLibrary.QueryHandlers.GetBookGenreQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
        }

        private IGetBookGenreQuery CreateGetBookGenreQuery(int? number = null)
        {
            return CreateGetBookGenreQueryMock(number).Object;
        }

        private Mock<IGetBookGenreQuery> CreateGetBookGenreQueryMock(int? number = null)
        {
            Mock<IGetBookGenreQuery> getBookGenreQueryMock = new Mock<IGetBookGenreQuery>();
            getBookGenreQueryMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return getBookGenreQueryMock;
        }
    }
}