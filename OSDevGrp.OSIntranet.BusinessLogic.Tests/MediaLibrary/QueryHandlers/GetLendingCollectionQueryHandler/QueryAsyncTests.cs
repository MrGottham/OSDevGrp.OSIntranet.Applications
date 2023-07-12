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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetLendingCollectionQueryHandler
{
	[TestFixture]
	public class QueryAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
		{
			IQueryHandler<IGetLendingCollectionQuery, IEnumerable<ILending>> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetLendingCollectionQuery()
		{
			IQueryHandler<IGetLendingCollectionQuery, IEnumerable<ILending>> sut = CreateSut();

			Mock<IGetLendingCollectionQuery> getLendingCollectionQueryMock = CreateGetLendingCollectionQueryMock();
			await sut.QueryAsync(getLendingCollectionQueryMock.Object);

			getLendingCollectionQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertIncludeReturnedWasCalledOnGetLendingCollectionQuery()
		{
			IQueryHandler<IGetLendingCollectionQuery, IEnumerable<ILending>> sut = CreateSut();

			Mock<IGetLendingCollectionQuery> getLendingCollectionQueryMock = CreateGetLendingCollectionQueryMock();
			await sut.QueryAsync(getLendingCollectionQueryMock.Object);

			getLendingCollectionQueryMock.Verify(m => m.IncludeReturned, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task QueryAsync_WhenCalled_AssertGetLendingsAsyncWasCalledOnMediaLibraryRepository(bool includeReturned)
		{
			IQueryHandler<IGetLendingCollectionQuery, IEnumerable<ILending>> sut = CreateSut();

			IGetLendingCollectionQuery getLendingCollectionQuery = CreateGetLendingCollectionQuery(includeReturned);
			await sut.QueryAsync(getLendingCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetLendingsAsync(It.Is<bool>(value => value == includeReturned)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenLendingCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetLendingCollectionQuery, IEnumerable<ILending>> sut = CreateSut();

			IEnumerable<ILending> result = await sut.QueryAsync(CreateGetLendingCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenLendingCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotEmpty()
		{
			IQueryHandler<IGetLendingCollectionQuery, IEnumerable<ILending>> sut = CreateSut();

			IEnumerable<ILending> result = await sut.QueryAsync(CreateGetLendingCollectionQuery());

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenLendingCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsLendingCollectionFromMediaLibraryRepository()
		{
			IEnumerable<ILending> lendingCollection = new[]
			{
				_fixture.BuildLendingMock().Object,
				_fixture.BuildLendingMock().Object,
				_fixture.BuildLendingMock().Object,
				_fixture.BuildLendingMock().Object,
				_fixture.BuildLendingMock().Object
			};
			IQueryHandler<IGetLendingCollectionQuery, IEnumerable<ILending>> sut = CreateSut(lendingCollection: lendingCollection);

			IEnumerable<ILending> result = await sut.QueryAsync(CreateGetLendingCollectionQuery());

			Assert.That(result, Is.EqualTo(lendingCollection));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenLendingCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetLendingCollectionQuery, IEnumerable<ILending>> sut = CreateSut(false);

			IEnumerable<ILending> result = await sut.QueryAsync(CreateGetLendingCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenLendingCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsEmpty()
		{
			IQueryHandler<IGetLendingCollectionQuery, IEnumerable<ILending>> sut = CreateSut(false);

			IEnumerable<ILending> result = await sut.QueryAsync(CreateGetLendingCollectionQuery());

			Assert.That(result, Is.Empty);
		}

		private IQueryHandler<IGetLendingCollectionQuery, IEnumerable<ILending>> CreateSut(bool hasLendingCollection = true, IEnumerable<ILending> lendingCollection = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetLendingsAsync(It.IsAny<bool>()))
				.Returns(Task.FromResult(hasLendingCollection ? lendingCollection ?? new[] {_fixture.BuildLendingMock().Object, _fixture.BuildLendingMock().Object, _fixture.BuildLendingMock().Object} : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetLendingCollectionQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private IGetLendingCollectionQuery CreateGetLendingCollectionQuery(bool? includeReturned = null)
		{
			return CreateGetLendingCollectionQueryMock(includeReturned).Object;
		}

		private Mock<IGetLendingCollectionQuery> CreateGetLendingCollectionQueryMock(bool? includeReturned = null)
		{
			Mock<IGetLendingCollectionQuery> getLendingCollectionQueryMock = new Mock<IGetLendingCollectionQuery>();
			getLendingCollectionQueryMock.Setup(m => m.IncludeReturned)
				.Returns(includeReturned ?? _random.Next(100) > 50);
			return getLendingCollectionQueryMock;
		}
	}
}