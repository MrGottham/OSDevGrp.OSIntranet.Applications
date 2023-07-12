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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetBorrowerCollectionQueryHandler
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
			IQueryHandler<IGetBorrowerCollectionQuery, IEnumerable<IBorrower>> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetBorrowerCollectionQuery()
		{
			IQueryHandler<IGetBorrowerCollectionQuery, IEnumerable<IBorrower>> sut = CreateSut();

			Mock<IGetBorrowerCollectionQuery> getBorrowerCollectionQueryMock = CreateGetBorrowerCollectionQueryMock();
			await sut.QueryAsync(getBorrowerCollectionQueryMock.Object);

			getBorrowerCollectionQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertFilterWasCalledOnGetBorrowerCollectionQuery()
		{
			IQueryHandler<IGetBorrowerCollectionQuery, IEnumerable<IBorrower>> sut = CreateSut();

			Mock<IGetBorrowerCollectionQuery> getBorrowerCollectionQueryMock = CreateGetBorrowerCollectionQueryMock();
			await sut.QueryAsync(getBorrowerCollectionQueryMock.Object);

			getBorrowerCollectionQueryMock.Verify(m => m.Filter, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		public async Task QueryAsync_WhenFilterHasNotBeenSetOnGetBorrowerCollectionQuery_AssertGetBorrowersAsyncWasCalledOnMediaLibraryRepository(string filter)
		{
			IQueryHandler<IGetBorrowerCollectionQuery, IEnumerable<IBorrower>> sut = CreateSut();

			IGetBorrowerCollectionQuery getBorrowerCollectionQuery = CreateGetBorrowerCollectionQuery(filter);
			await sut.QueryAsync(getBorrowerCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetBorrowersAsync(It.Is<string>(value => string.IsNullOrWhiteSpace(value))), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenFilterHasBeenSetOnGetBorrowerCollectionQuery_AssertGetBorrowersAsyncWasCalledOnMediaLibraryRepository()
		{
			IQueryHandler<IGetBorrowerCollectionQuery, IEnumerable<IBorrower>> sut = CreateSut();

			string filter = _fixture.Create<string>();
			IGetBorrowerCollectionQuery getBorrowerCollectionQuery = CreateGetBorrowerCollectionQuery(filter);
			await sut.QueryAsync(getBorrowerCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetBorrowersAsync(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, filter) == 0)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBorrowerCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetBorrowerCollectionQuery, IEnumerable<IBorrower>> sut = CreateSut();

			IEnumerable<IBorrower> result = await sut.QueryAsync(CreateGetBorrowerCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBorrowerCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotEmpty()
		{
			IQueryHandler<IGetBorrowerCollectionQuery, IEnumerable<IBorrower>> sut = CreateSut();

			IEnumerable<IBorrower> result = await sut.QueryAsync(CreateGetBorrowerCollectionQuery());

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBorrowerCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsBorrowerCollectionFromMediaLibraryRepository()
		{
			IEnumerable<IBorrower> borrowerCollection = new[]
			{
				_fixture.BuildBorrowerMock().Object,
				_fixture.BuildBorrowerMock().Object,
				_fixture.BuildBorrowerMock().Object,
				_fixture.BuildBorrowerMock().Object,
				_fixture.BuildBorrowerMock().Object
			};
			IQueryHandler<IGetBorrowerCollectionQuery, IEnumerable<IBorrower>> sut = CreateSut(borrowerCollection: borrowerCollection);

			IEnumerable<IBorrower> result = await sut.QueryAsync(CreateGetBorrowerCollectionQuery());

			Assert.That(result, Is.EqualTo(borrowerCollection));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBorrowerCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetBorrowerCollectionQuery, IEnumerable<IBorrower>> sut = CreateSut(false);

			IEnumerable<IBorrower> result = await sut.QueryAsync(CreateGetBorrowerCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBorrowerCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsEmpty()
		{
			IQueryHandler<IGetBorrowerCollectionQuery, IEnumerable<IBorrower>> sut = CreateSut(false);

			IEnumerable<IBorrower> result = await sut.QueryAsync(CreateGetBorrowerCollectionQuery());

			Assert.That(result, Is.Empty);
		}

		private IQueryHandler<IGetBorrowerCollectionQuery, IEnumerable<IBorrower>> CreateSut(bool hasBorrowerCollection = true, IEnumerable<IBorrower> borrowerCollection = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetBorrowersAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(hasBorrowerCollection ? borrowerCollection ?? new[] {_fixture.BuildBorrowerMock().Object, _fixture.BuildBorrowerMock().Object, _fixture.BuildBorrowerMock().Object} : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetBorrowerCollectionQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private static IGetBorrowerCollectionQuery CreateGetBorrowerCollectionQuery(string filter = null)
		{
			return CreateGetBorrowerCollectionQueryMock(filter).Object;
		}

		private static Mock<IGetBorrowerCollectionQuery> CreateGetBorrowerCollectionQueryMock(string filter = null)
		{
			Mock<IGetBorrowerCollectionQuery> getBorrowerCollectionQueryMock = new Mock<IGetBorrowerCollectionQuery>();
			getBorrowerCollectionQueryMock.Setup(m => m.Filter)
				.Returns(string.IsNullOrWhiteSpace(filter) == false ? filter : null);
			return getBorrowerCollectionQueryMock;
		}
	}
}