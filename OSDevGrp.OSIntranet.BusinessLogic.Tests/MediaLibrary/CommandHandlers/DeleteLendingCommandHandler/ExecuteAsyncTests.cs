using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.DeleteLendingCommandHandler
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
			ICommandHandler<IDeleteLendingCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteLendingCommand()
		{
			ICommandHandler<IDeleteLendingCommand> sut = CreateSut();

			Mock<IDeleteLendingCommand> deleteLendingCommandMock = BuildDeleteLendingCommandMock();
			await sut.ExecuteAsync(deleteLendingCommandMock.Object);

			deleteLendingCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertLendingIdentifierWasCalledOnDeleteLendingCommand()
		{
			ICommandHandler<IDeleteLendingCommand> sut = CreateSut();

			Mock<IDeleteLendingCommand> deleteLendingCommandMock = BuildDeleteLendingCommandMock();
			await sut.ExecuteAsync(deleteLendingCommandMock.Object);

			deleteLendingCommandMock.Verify(m => m.LendingIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertDeleteLendingAsyncWasCalledOnMediaLibraryRepositoryWithLendingIdentifierOnDeleteLendingCommand()
		{
			ICommandHandler<IDeleteLendingCommand> sut = CreateSut();

			Guid lendingIdentifier = Guid.NewGuid();
			IDeleteLendingCommand deleteLendingCommand = BuildDeleteLendingCommand(lendingIdentifier);
			await sut.ExecuteAsync(deleteLendingCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.DeleteLendingAsync(It.Is<Guid>(value => value == lendingIdentifier)), Times.Once);
		}

		private ICommandHandler<IDeleteLendingCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.DeleteLendingAsync(It.IsAny<Guid>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.DeleteLendingCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IDeleteLendingCommand BuildDeleteLendingCommand(Guid? lendingIdentifier = null)
		{
			return BuildDeleteLendingCommandMock(lendingIdentifier).Object;
		}

		private Mock<IDeleteLendingCommand> BuildDeleteLendingCommandMock(Guid? lendingIdentifier = null)
		{
			Mock<IDeleteLendingCommand> deleteLendingCommandMock = new Mock<IDeleteLendingCommand>();
			deleteLendingCommandMock.Setup(m => m.LendingIdentifier)
				.Returns(lendingIdentifier ?? Guid.NewGuid());
			return deleteLendingCommandMock;
		}
	}
}