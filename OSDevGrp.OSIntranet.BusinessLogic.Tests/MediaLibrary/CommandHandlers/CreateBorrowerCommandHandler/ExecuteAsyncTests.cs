using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.CreateBorrowerCommandHandler
{
	[TestFixture]
	public class ExecuteAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<IClaimResolver> _claimResolverMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_claimResolverMock = new Mock<IClaimResolver>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<ICreateBorrowerCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCreateBorrowerCommand()
		{
			ICommandHandler<ICreateBorrowerCommand> sut = CreateSut();

			Mock<ICreateBorrowerCommand> createBorrowerCommandMock = BuildCreateBorrowerCommandMock();
			await sut.ExecuteAsync(createBorrowerCommandMock.Object);

			createBorrowerCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnCreateBorrowerCommand()
		{
			ICommandHandler<ICreateBorrowerCommand> sut = CreateSut();

			Mock<ICreateBorrowerCommand> createBorrowerCommandMock = BuildCreateBorrowerCommandMock();
			await sut.ExecuteAsync(createBorrowerCommandMock.Object);

			createBorrowerCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertCreateBorrowerAsyncWasCalledOnMediaLibraryRepositoryWithBorrowerFromToDomainAsyncOnCreateBorrowerCommand()
		{
			ICommandHandler<ICreateBorrowerCommand> sut = CreateSut();

			IBorrower borrower = _fixture.BuildBorrowerMock().Object;
			ICreateBorrowerCommand createBorrowerCommand = BuildCreateBorrowerCommand(borrower);
			await sut.ExecuteAsync(createBorrowerCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.CreateBorrowerAsync(It.Is<IBorrower>(value => value != null && value == borrower)), Times.Once);
		}

		private ICommandHandler<ICreateBorrowerCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.CreateBorrowerAsync(It.IsAny<IBorrower>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.CreateBorrowerCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private ICreateBorrowerCommand BuildCreateBorrowerCommand(IBorrower borrower = null)
		{
			return BuildCreateBorrowerCommandMock(borrower).Object;
		}

		private Mock<ICreateBorrowerCommand> BuildCreateBorrowerCommandMock(IBorrower borrower = null)
		{
			Mock<ICreateBorrowerCommand> createBorrowerCommandMock = new Mock<ICreateBorrowerCommand>();
			createBorrowerCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(borrower ?? _fixture.BuildBorrowerMock().Object));
			return createBorrowerCommandMock;
		}
	}
}