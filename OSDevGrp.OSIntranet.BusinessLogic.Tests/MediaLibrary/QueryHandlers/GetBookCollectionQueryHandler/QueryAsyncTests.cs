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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetBookCollectionQueryHandler
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
			IQueryHandler<IGetBookCollectionQuery, IEnumerable<IBook>> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetBookCollectionQuery()
		{
			IQueryHandler<IGetBookCollectionQuery, IEnumerable<IBook>> sut = CreateSut();

			Mock<IGetBookCollectionQuery> getBookCollectionQueryMock = CreateGetBookCollectionQueryMock();
			await sut.QueryAsync(getBookCollectionQueryMock.Object);

			getBookCollectionQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertFilterWasCalledOnGetBookCollectionQuery()
		{
			IQueryHandler<IGetBookCollectionQuery, IEnumerable<IBook>> sut = CreateSut();

			Mock<IGetBookCollectionQuery> getBookCollectionQueryMock = CreateGetBookCollectionQueryMock();
			await sut.QueryAsync(getBookCollectionQueryMock.Object);

			getBookCollectionQueryMock.Verify(m => m.Filter, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		public async Task QueryAsync_WhenFilterHasNotBeenSetOnGetBookCollectionQuery_AssertGetMediasAsyncWasCalledOnMediaLibraryRepository(string filter)
		{
			IQueryHandler<IGetBookCollectionQuery, IEnumerable<IBook>> sut = CreateSut();

			IGetBookCollectionQuery getBookCollectionQuery = CreateGetBookCollectionQuery(filter);
			await sut.QueryAsync(getBookCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediasAsync<IBook>(It.Is<string>(value => string.IsNullOrWhiteSpace(value))), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenFilterHasBeenSetOnGetBookCollectionQuery_AssertGetMediasAsyncWasCalledOnMediaLibraryRepository()
		{
			IQueryHandler<IGetBookCollectionQuery, IEnumerable<IBook>> sut = CreateSut();

			string filter = _fixture.Create<string>();
			IGetBookCollectionQuery getBookCollectionQuery = CreateGetBookCollectionQuery(filter);
			await sut.QueryAsync(getBookCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediasAsync<IBook>(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, filter) == 0)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBookCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetBookCollectionQuery, IEnumerable<IBook>> sut = CreateSut();

			IEnumerable<IBook> result = await sut.QueryAsync(CreateGetBookCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBookCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotEmpty()
		{
			IQueryHandler<IGetBookCollectionQuery, IEnumerable<IBook>> sut = CreateSut();

			IEnumerable<IBook> result = await sut.QueryAsync(CreateGetBookCollectionQuery());

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBookCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsBookCollectionFromMediaLibraryRepository()
		{
			IEnumerable<IBook> bookCollection = new[]
			{
				_fixture.BuildBookMock().Object,
				_fixture.BuildBookMock().Object,
				_fixture.BuildBookMock().Object,
				_fixture.BuildBookMock().Object,
				_fixture.BuildBookMock().Object
			};
			IQueryHandler<IGetBookCollectionQuery, IEnumerable<IBook>> sut = CreateSut(bookCollection: bookCollection);

			IEnumerable<IBook> result = await sut.QueryAsync(CreateGetBookCollectionQuery());

			Assert.That(result, Is.EqualTo(bookCollection));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBookCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetBookCollectionQuery, IEnumerable<IBook>> sut = CreateSut(false);

			IEnumerable<IBook> result = await sut.QueryAsync(CreateGetBookCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBookCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsEmpty()
		{
			IQueryHandler<IGetBookCollectionQuery, IEnumerable<IBook>> sut = CreateSut(false);

			IEnumerable<IBook> result = await sut.QueryAsync(CreateGetBookCollectionQuery());

			Assert.That(result, Is.Empty);
		}

		private IQueryHandler<IGetBookCollectionQuery, IEnumerable<IBook>> CreateSut(bool hasBookCollection = true, IEnumerable<IBook> bookCollection = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediasAsync<IBook>(It.IsAny<string>()))
				.Returns(Task.FromResult(hasBookCollection ? bookCollection ?? new[] {_fixture.BuildBookMock().Object, _fixture.BuildBookMock().Object, _fixture.BuildBookMock().Object} : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetBookCollectionQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private static IGetBookCollectionQuery CreateGetBookCollectionQuery(string filter = null)
		{
			return CreateGetBookCollectionQueryMock(filter).Object;
		}

		private static Mock<IGetBookCollectionQuery> CreateGetBookCollectionQueryMock(string filter = null)
		{
			Mock<IGetBookCollectionQuery> getBookCollectionQueryMock = new Mock<IGetBookCollectionQuery>();
			getBookCollectionQueryMock.Setup(m => m.Filter)
				.Returns(string.IsNullOrWhiteSpace(filter) == false ? filter : null);
			return getBookCollectionQueryMock;
		}
	}
}