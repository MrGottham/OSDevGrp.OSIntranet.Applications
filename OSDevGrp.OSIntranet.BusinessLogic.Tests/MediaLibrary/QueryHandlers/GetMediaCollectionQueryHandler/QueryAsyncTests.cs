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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMediaCollectionQueryHandler
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
			IQueryHandler<IGetMediaCollectionQuery, IEnumerable<IMedia>> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetMediaCollectionQuery()
		{
			IQueryHandler<IGetMediaCollectionQuery, IEnumerable<IMedia>> sut = CreateSut();

			Mock<IGetMediaCollectionQuery> getMediaCollectionQueryMock = CreateGetMediaCollectionQueryMock();
			await sut.QueryAsync(getMediaCollectionQueryMock.Object);

			getMediaCollectionQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertFilterWasCalledOnGetMediaCollectionQuery()
		{
			IQueryHandler<IGetMediaCollectionQuery, IEnumerable<IMedia>> sut = CreateSut();

			Mock<IGetMediaCollectionQuery> getMediaCollectionQueryMock = CreateGetMediaCollectionQueryMock();
			await sut.QueryAsync(getMediaCollectionQueryMock.Object);

			getMediaCollectionQueryMock.Verify(m => m.Filter, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		public async Task QueryAsync_WhenFilterHasNotBeenSetOnGetMediaCollectionQuery_AssertGetMediasAsyncWasCalledOnMediaLibraryRepository(string filter)
		{
			IQueryHandler<IGetMediaCollectionQuery, IEnumerable<IMedia>> sut = CreateSut();

			IGetMediaCollectionQuery getMediaCollectionQuery = CreateGetMediaCollectionQuery(filter);
			await sut.QueryAsync(getMediaCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediasAsync(It.Is<string>(value => string.IsNullOrWhiteSpace(value))), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenFilterHasBeenSetOnGetMediaCollectionQuery_AssertGetMediasAsyncWasCalledOnMediaLibraryRepository()
		{
			IQueryHandler<IGetMediaCollectionQuery, IEnumerable<IMedia>> sut = CreateSut();

			string filter = _fixture.Create<string>();
			IGetMediaCollectionQuery getMediaCollectionQuery = CreateGetMediaCollectionQuery(filter);
			await sut.QueryAsync(getMediaCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediasAsync(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, filter) == 0)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetMediaCollectionQuery, IEnumerable<IMedia>> sut = CreateSut();

			IEnumerable<IMedia> result = await sut.QueryAsync(CreateGetMediaCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotEmpty()
		{
			IQueryHandler<IGetMediaCollectionQuery, IEnumerable<IMedia>> sut = CreateSut();

			IEnumerable<IMedia> result = await sut.QueryAsync(CreateGetMediaCollectionQuery());

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsMediaCollectionFromMediaLibraryRepository()
		{
			IEnumerable<IMedia> mediaCollection = new[]
			{
				_fixture.BuildMediaMock().Object,
				_fixture.BuildMediaMock().Object,
				_fixture.BuildMediaMock().Object,
				_fixture.BuildMediaMock().Object,
				_fixture.BuildMediaMock().Object
			};
			IQueryHandler<IGetMediaCollectionQuery, IEnumerable<IMedia>> sut = CreateSut(mediaCollection: mediaCollection);

			IEnumerable<IMedia> result = await sut.QueryAsync(CreateGetMediaCollectionQuery());

			Assert.That(result, Is.EqualTo(mediaCollection));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetMediaCollectionQuery, IEnumerable<IMedia>> sut = CreateSut(false);

			IEnumerable<IMedia> result = await sut.QueryAsync(CreateGetMediaCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsEmpty()
		{
			IQueryHandler<IGetMediaCollectionQuery, IEnumerable<IMedia>> sut = CreateSut(false);

			IEnumerable<IMedia> result = await sut.QueryAsync(CreateGetMediaCollectionQuery());

			Assert.That(result, Is.Empty);
		}

		private IQueryHandler<IGetMediaCollectionQuery, IEnumerable<IMedia>> CreateSut(bool hasMediaCollection = true, IEnumerable<IMedia> mediaCollection = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediasAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(hasMediaCollection ? mediaCollection ?? new[] {_fixture.BuildMediaMock().Object, _fixture.BuildMediaMock().Object, _fixture.BuildMediaMock().Object} : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetMediaCollectionQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private static IGetMediaCollectionQuery CreateGetMediaCollectionQuery(string filter = null)
		{
			return CreateGetMediaCollectionQueryMock(filter).Object;
		}

		private static Mock<IGetMediaCollectionQuery> CreateGetMediaCollectionQueryMock(string filter = null)
		{
			Mock<IGetMediaCollectionQuery> getMediaCollectionQueryMock = new Mock<IGetMediaCollectionQuery>();
			getMediaCollectionQueryMock.Setup(m => m.Filter)
				.Returns(string.IsNullOrWhiteSpace(filter) == false ? filter : null);
			return getMediaCollectionQueryMock;
		}
	}
}