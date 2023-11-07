using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.DeleteBorrowerCommandHandler
{
	[TestFixture]
	public class ExecuteAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<IClaimResolver> _claimResolverMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_claimResolverMock = new Mock<IClaimResolver>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<IDeleteBorrowerCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteBorrowerCommand()
		{
			ICommandHandler<IDeleteBorrowerCommand> sut = CreateSut();

			Mock<IDeleteBorrowerCommand> deleteBorrowerCommandMock = BuildDeleteBorrowerCommandMock();
			await sut.ExecuteAsync(deleteBorrowerCommandMock.Object);

			deleteBorrowerCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertBorrowerIdentifierWasCalledOnDeleteBorrowerCommand()
		{
			ICommandHandler<IDeleteBorrowerCommand> sut = CreateSut();

			Mock<IDeleteBorrowerCommand> deleteBorrowerCommandMock = BuildDeleteBorrowerCommandMock();
			await sut.ExecuteAsync(deleteBorrowerCommandMock.Object);

			deleteBorrowerCommandMock.Verify(m => m.BorrowerIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertDeleteBorrowerAsyncWasCalledOnMediaLibraryRepositoryWithBorrowerIdentifierOnDeleteBorrowerCommand()
		{
			ICommandHandler<IDeleteBorrowerCommand> sut = CreateSut();

			Guid borrowerIdentifier = Guid.NewGuid();
			IDeleteBorrowerCommand deleteBorrowerCommand = BuildDeleteBorrowerCommand(borrowerIdentifier);
			await sut.ExecuteAsync(deleteBorrowerCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.DeleteBorrowerAsync(It.Is<Guid>(value => value == borrowerIdentifier)), Times.Once);
		}

		private ICommandHandler<IDeleteBorrowerCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.DeleteBorrowerAsync(It.IsAny<Guid>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.DeleteBorrowerCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IDeleteBorrowerCommand BuildDeleteBorrowerCommand(Guid? borrowerIdentifier = null)
		{
			return BuildDeleteBorrowerCommandMock(borrowerIdentifier).Object;
		}

		private Mock<IDeleteBorrowerCommand> BuildDeleteBorrowerCommandMock(Guid? borrowerIdentifier = null)
		{
			Mock<IDeleteBorrowerCommand> deleteBorrowerCommandMock = new Mock<IDeleteBorrowerCommand>();
			deleteBorrowerCommandMock.Setup(m => m.BorrowerIdentifier)
				.Returns(borrowerIdentifier ?? Guid.NewGuid());
			return deleteBorrowerCommandMock;
		}
	}
}