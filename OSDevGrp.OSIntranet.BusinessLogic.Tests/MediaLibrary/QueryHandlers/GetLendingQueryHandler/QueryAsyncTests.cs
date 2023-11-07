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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetLendingQueryHandler
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
			IQueryHandler<IGetLendingQuery, ILending> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetLendingQuery()
		{
			IQueryHandler<IGetLendingQuery, ILending> sut = CreateSut();

			Mock<IGetLendingQuery> getLendingQueryMock = CreateGetLendingQueryMock();
			await sut.QueryAsync(getLendingQueryMock.Object);

			getLendingQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertLendingIdentifierWasCalledOnGetLendingQuery()
		{
			IQueryHandler<IGetLendingQuery, ILending> sut = CreateSut();

			Mock<IGetLendingQuery> getLendingQueryMock = CreateGetLendingQueryMock();
			await sut.QueryAsync(getLendingQueryMock.Object);

			getLendingQueryMock.Verify(m => m.LendingIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertGetLendingAsyncWasCalledOnMediaLibraryRepository()
		{
			IQueryHandler<IGetLendingQuery, ILending> sut = CreateSut();

			Guid mediaIdentifier = Guid.NewGuid();
			IGetLendingQuery getLendingQuery = CreateGetLendingQuery(mediaIdentifier);
			await sut.QueryAsync(getLendingQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetLendingAsync(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenLendingHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetLendingQuery, ILending> sut = CreateSut();

			ILending result = await sut.QueryAsync(CreateGetLendingQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenLendingHasBeenReturnedFromMediaLibraryRepository_ReturnsLendingFromMediaLibraryRepository()
		{
			ILending lending = _fixture.BuildLendingMock().Object;
			IQueryHandler<IGetLendingQuery, ILending> sut = CreateSut(lending: lending);

			ILending result = await sut.QueryAsync(CreateGetLendingQuery());

			Assert.That(result, Is.EqualTo(lending));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenLendingHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNull()
		{
			IQueryHandler<IGetLendingQuery, ILending> sut = CreateSut(false);

			ILending result = await sut.QueryAsync(CreateGetLendingQuery());

			Assert.That(result, Is.Null);
		}

		private IQueryHandler<IGetLendingQuery, ILending> CreateSut(bool hasLending = true, ILending lending = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetLendingAsync(It.IsAny<Guid>()))
				.Returns(Task.FromResult(hasLending ? lending ?? _fixture.BuildLendingMock().Object : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetLendingQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private static IGetLendingQuery CreateGetLendingQuery(Guid? lendingIdentifier = null)
		{
			return CreateGetLendingQueryMock(lendingIdentifier).Object;
		}

		private static Mock<IGetLendingQuery> CreateGetLendingQueryMock(Guid? lendingIdentifier = null)
		{
			Mock<IGetLendingQuery> getLendingQueryMock = new Mock<IGetLendingQuery>();
			getLendingQueryMock.Setup(m => m.LendingIdentifier)
				.Returns(lendingIdentifier ?? Guid.NewGuid());
			return getLendingQueryMock;
		}
	}
}