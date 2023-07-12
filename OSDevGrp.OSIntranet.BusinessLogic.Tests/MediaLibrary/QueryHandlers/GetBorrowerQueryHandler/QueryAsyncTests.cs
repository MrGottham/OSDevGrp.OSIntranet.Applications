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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetBorrowerQueryHandler
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
			IQueryHandler<IGetBorrowerQuery, IBorrower> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetBorrowerQuery()
		{
			IQueryHandler<IGetBorrowerQuery, IBorrower> sut = CreateSut();

			Mock<IGetBorrowerQuery> getBorrowerQueryMock = CreateGetBorrowerQueryMock();
			await sut.QueryAsync(getBorrowerQueryMock.Object);

			getBorrowerQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertBorrowerIdentifierWasCalledOnGetBorrowerQuery()
		{
			IQueryHandler<IGetBorrowerQuery, IBorrower> sut = CreateSut();

			Mock<IGetBorrowerQuery> getBorrowerQueryMock = CreateGetBorrowerQueryMock();
			await sut.QueryAsync(getBorrowerQueryMock.Object);

			getBorrowerQueryMock.Verify(m => m.BorrowerIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertGetBorrowerAsyncWasCalledOnMediaLibraryRepository()
		{
			IQueryHandler<IGetBorrowerQuery, IBorrower> sut = CreateSut();

			Guid mediaIdentifier = Guid.NewGuid();
			IGetBorrowerQuery getBorrowerQuery = CreateGetBorrowerQuery(mediaIdentifier);
			await sut.QueryAsync(getBorrowerQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetBorrowerAsync(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBorrowerHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetBorrowerQuery, IBorrower> sut = CreateSut();

			IBorrower result = await sut.QueryAsync(CreateGetBorrowerQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBorrowerHasBeenReturnedFromMediaLibraryRepository_ReturnsBorrowerFromMediaLibraryRepository()
		{
			IBorrower borrower = _fixture.BuildBorrowerMock().Object;
			IQueryHandler<IGetBorrowerQuery, IBorrower> sut = CreateSut(borrower: borrower);

			IBorrower result = await sut.QueryAsync(CreateGetBorrowerQuery());

			Assert.That(result, Is.EqualTo(borrower));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenBorrowerHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNull()
		{
			IQueryHandler<IGetBorrowerQuery, IBorrower> sut = CreateSut(false);

			IBorrower result = await sut.QueryAsync(CreateGetBorrowerQuery());

			Assert.That(result, Is.Null);
		}

		private IQueryHandler<IGetBorrowerQuery, IBorrower> CreateSut(bool hasBorrower = true, IBorrower borrower = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetBorrowerAsync(It.IsAny<Guid>()))
				.Returns(Task.FromResult(hasBorrower ? borrower ?? _fixture.BuildBorrowerMock().Object : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetBorrowerQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private static IGetBorrowerQuery CreateGetBorrowerQuery(Guid? borrowerIdentifier = null)
		{
			return CreateGetBorrowerQueryMock(borrowerIdentifier).Object;
		}

		private static Mock<IGetBorrowerQuery> CreateGetBorrowerQueryMock(Guid? borrowerIdentifier = null)
		{
			Mock<IGetBorrowerQuery> getBorrowerQueryMock = new Mock<IGetBorrowerQuery>();
			getBorrowerQueryMock.Setup(m => m.BorrowerIdentifier)
				.Returns(borrowerIdentifier ?? Guid.NewGuid());
			return getBorrowerQueryMock;
		}
	}
}