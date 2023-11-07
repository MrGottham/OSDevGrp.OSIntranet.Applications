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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMediaTypeQueryHandler
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
            IQueryHandler<IGetMediaTypeQuery, IMediaType> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetMediaTypeQuery()
        {
            IQueryHandler<IGetMediaTypeQuery, IMediaType> sut = CreateSut();

            Mock<IGetMediaTypeQuery> getMediaTypeQueryMock = CreateGetMediaTypeQueryMock();
            await sut.QueryAsync(getMediaTypeQueryMock.Object);

            getMediaTypeQueryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberWasCalledOnGetMediaTypeQuery()
        {
            IQueryHandler<IGetMediaTypeQuery, IMediaType> sut = CreateSut();

            Mock<IGetMediaTypeQuery> getMediaTypeQueryMock = CreateGetMediaTypeQueryMock();
            await sut.QueryAsync(getMediaTypeQueryMock.Object);

            getMediaTypeQueryMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetMediaTypeAsyncWasCalledOnMediaLibraryRepository()
        {
            IQueryHandler<IGetMediaTypeQuery, IMediaType> sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.QueryAsync(CreateGetMediaTypeQuery(number));

            _mediaLibraryRepositoryMock.Verify(m => m.GetMediaTypeAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMediaTypeWasReturnedFromMediaLibraryRepository_ReturnsNotNull()
        {
            IQueryHandler<IGetMediaTypeQuery, IMediaType> sut = CreateSut();

            IMediaType result = await sut.QueryAsync(CreateGetMediaTypeQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMediaTypeWasReturnedFromMediaLibraryRepository_ReturnsMediaTypeFromMediaLibraryRepository()
        {
            IMediaType mediaType = _fixture.BuildMediaTypeMock().Object;
            IQueryHandler<IGetMediaTypeQuery, IMediaType> sut = CreateSut(mediaType: mediaType);

            IMediaType result = await sut.QueryAsync(CreateGetMediaTypeQuery());

            Assert.That(result, Is.EqualTo(mediaType));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMediaTypeWasReturnedFromMediaLibraryRepository_ReturnsNull()
        {
            IQueryHandler<IGetMediaTypeQuery, IMediaType> sut = CreateSut(false);

            IMediaType result = await sut.QueryAsync(CreateGetMediaTypeQuery());

            Assert.That(result, Is.Null);
        }

        private IQueryHandler<IGetMediaTypeQuery, IMediaType> CreateSut(bool hasMediaType = true, IMediaType mediaType = null)
        {
            _mediaLibraryRepositoryMock.Setup(m => m.GetMediaTypeAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(hasMediaType ? mediaType ?? _fixture.BuildMediaTypeMock().Object : null));

            return new BusinessLogic.MediaLibrary.QueryHandlers.GetMediaTypeQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
        }

        private IGetMediaTypeQuery CreateGetMediaTypeQuery(int? number = null)
        {
            return CreateGetMediaTypeQueryMock(number).Object;
        }

        private Mock<IGetMediaTypeQuery> CreateGetMediaTypeQueryMock(int? number = null)
        {
            Mock<IGetMediaTypeQuery> getMediaTypeQueryMock = new Mock<IGetMediaTypeQuery>();
            getMediaTypeQueryMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return getMediaTypeQueryMock;
        }
    }
}