using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.DeleteMediaPersonalityCommandHandler
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
			ICommandHandler<IDeleteMediaPersonalityCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteMediaPersonalityCommand()
		{
			ICommandHandler<IDeleteMediaPersonalityCommand> sut = CreateSut();

			Mock<IDeleteMediaPersonalityCommand> deleteMediaPersonalityCommandMock = BuildDeleteMediaPersonalityCommandMock();
			await sut.ExecuteAsync(deleteMediaPersonalityCommandMock.Object);

			deleteMediaPersonalityCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertMediaPersonalityIdentifierWasCalledOnDeleteMediaPersonalityCommand()
		{
			ICommandHandler<IDeleteMediaPersonalityCommand> sut = CreateSut();

			Mock<IDeleteMediaPersonalityCommand> deleteMediaPersonalityCommandMock = BuildDeleteMediaPersonalityCommandMock();
			await sut.ExecuteAsync(deleteMediaPersonalityCommandMock.Object);

			deleteMediaPersonalityCommandMock.Verify(m => m.MediaPersonalityIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertDeleteMediaPersonalityAsyncWasCalledOnMediaLibraryRepositoryWithMediaPersonalityIdentifierOnDeleteMediaPersonalityCommand()
		{
			ICommandHandler<IDeleteMediaPersonalityCommand> sut = CreateSut();

			Guid mediaPersonalityIdentifier = Guid.NewGuid();
			IDeleteMediaPersonalityCommand deleteMediaPersonalityCommand = BuildDeleteMediaPersonalityCommand(mediaPersonalityIdentifier);
			await sut.ExecuteAsync(deleteMediaPersonalityCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.DeleteMediaPersonalityAsync(It.Is<Guid>(value => value == mediaPersonalityIdentifier)), Times.Once);
		}

		private ICommandHandler<IDeleteMediaPersonalityCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.DeleteMediaPersonalityAsync(It.IsAny<Guid>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.DeleteMediaPersonalityCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IDeleteMediaPersonalityCommand BuildDeleteMediaPersonalityCommand(Guid? mediaPersonalityIdentifier = null)
		{
			return BuildDeleteMediaPersonalityCommandMock(mediaPersonalityIdentifier).Object;
		}

		private Mock<IDeleteMediaPersonalityCommand> BuildDeleteMediaPersonalityCommandMock(Guid? mediaPersonalityIdentifier = null)
		{
			Mock<IDeleteMediaPersonalityCommand> deleteMediaPersonalityCommandMock = new Mock<IDeleteMediaPersonalityCommand>();
			deleteMediaPersonalityCommandMock.Setup(m => m.MediaPersonalityIdentifier)
				.Returns(mediaPersonalityIdentifier ?? Guid.NewGuid());
			return deleteMediaPersonalityCommandMock;
		}
	}
}