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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMediaTypeCollectionQueryHandler
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
            IQueryHandler<EmptyQuery, IEnumerable<IMediaType>> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetMediaTypesAsyncWasCalledOnMediaLibraryRepository()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMediaType>> sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _mediaLibraryRepositoryMock.Verify(m => m.GetMediaTypesAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMediaTypesWasReturnedFromMediaLibraryRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMediaType>> sut = CreateSut();

            IEnumerable<IMediaType> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMediaTypesWasReturnedFromMediaLibraryRepository_ReturnsNonEmptyCollectionOfMediaTypes()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMediaType>> sut = CreateSut();

            IEnumerable<IMediaType> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMediaTypesWasReturnedFromMediaLibraryRepository_ReturnsMediaTypeCollectionFromMediaLibraryRepository()
        {
            IMediaType[] mediaTypeCollection = BuildMediaTypeCollection();
            IQueryHandler<EmptyQuery, IEnumerable<IMediaType>> sut = CreateSut(mediaTypeCollection: mediaTypeCollection);

            IEnumerable<IMediaType> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(mediaTypeCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMediaTypesWasReturnedFromMediaLibraryRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMediaType>> sut = CreateSut(false);

            IEnumerable<IMediaType> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMediaTypesWasReturnedFromMediaLibraryRepository_ReturnsEmptyCollectionOfMediaTypes()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IMediaType>> sut = CreateSut(false);

            IEnumerable<IMediaType> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Empty);
        }

        public IQueryHandler<EmptyQuery, IEnumerable<IMediaType>> CreateSut(bool hasMediaTypeCollection = true, IEnumerable<IMediaType> mediaTypeCollection = null)
        {
            _mediaLibraryRepositoryMock.Setup(m => m.GetMediaTypesAsync())
                .Returns(Task.FromResult(hasMediaTypeCollection ? mediaTypeCollection ?? BuildMediaTypeCollection() : null));

            return new BusinessLogic.MediaLibrary.QueryHandlers.GetMediaTypeCollectionQueryHandler(_mediaLibraryRepositoryMock.Object);
        }

        private IMediaType[] BuildMediaTypeCollection()
        {
            return new[]
            {
                _fixture.BuildMediaTypeMock().Object,
                _fixture.BuildMediaTypeMock().Object,
                _fixture.BuildMediaTypeMock().Object,
                _fixture.BuildMediaTypeMock().Object,
                _fixture.BuildMediaTypeMock().Object
            };
        }
    }
}