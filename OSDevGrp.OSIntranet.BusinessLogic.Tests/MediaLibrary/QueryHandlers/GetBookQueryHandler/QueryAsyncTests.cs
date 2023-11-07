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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetBookQueryHandler
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
			IQueryHandler<IGetBookQuery, IBook> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetBookQuery()
		{
			IQueryHandler<IGetBookQuery, IBook> sut = CreateSut();

			Mock<IGetBookQuery> getBookQueryMock = CreateGetBookQueryMock();
			await sut.QueryAsync(getBookQueryMock.Object);

			getBookQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertMediaIdentifierWasCalledOnGetBookQuery()
		{
			IQueryHandler<IGetBookQuery, IBook> sut = CreateSut();

			Mock<IGetBookQuery> getBookQueryMock = CreateGetBookQueryMock();
			await sut.QueryAsync(getBookQueryMock.Object);

			getBookQueryMock.Verify(m => m.MediaIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertGetMediaAsyncWasCalledOnMediaLibraryRepository()
		{
			IQueryHandler<IGetBookQuery, IBook> sut = CreateSut();

			Guid mediaIdentifier = Guid.NewGuid();
			IGetBookQuery getBookQuery = CreateGetBookQuery(mediaIdentifier);
			await sut.QueryAsync(getBookQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IBook>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBookHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetBookQuery, IBook> sut = CreateSut();

			IBook result = await sut.QueryAsync(CreateGetBookQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBookHasBeenReturnedFromMediaLibraryRepository_ReturnsBookFromMediaLibraryRepository()
		{
			IBook book = _fixture.BuildBookMock().Object;
			IQueryHandler<IGetBookQuery, IBook> sut = CreateSut(book: book);

			IBook result = await sut.QueryAsync(CreateGetBookQuery());

			Assert.That(result, Is.EqualTo(book));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBookHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNull()
		{
			IQueryHandler<IGetBookQuery, IBook> sut = CreateSut(false);

			IBook result = await sut.QueryAsync(CreateGetBookQuery());

			Assert.That(result, Is.Null);
		}

		private IQueryHandler<IGetBookQuery, IBook> CreateSut(bool hasBook = true, IBook book = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IBook>(It.IsAny<Guid>()))
				.Returns(Task.FromResult(hasBook ? book ?? _fixture.BuildBookMock().Object : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetBookQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private static IGetBookQuery CreateGetBookQuery(Guid? mediaIdentifier = null)
		{
			return CreateGetBookQueryMock(mediaIdentifier).Object;
		}

		private static Mock<IGetBookQuery> CreateGetBookQueryMock(Guid? mediaIdentifier = null)
		{
			Mock<IGetBookQuery> getBookQueryMock = new Mock<IGetBookQuery>();
			getBookQueryMock.Setup(m => m.MediaIdentifier)
				.Returns(mediaIdentifier ?? Guid.NewGuid());
			return getBookQueryMock;
		}
	}
}