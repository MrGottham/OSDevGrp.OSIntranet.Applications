using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.DeleteMusicCommandHandler
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
			ICommandHandler<IDeleteMusicCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteMusicCommand()
		{
			ICommandHandler<IDeleteMusicCommand> sut = CreateSut();

			Mock<IDeleteMusicCommand> deleteMusicCommandMock = BuildDeleteMusicCommandMock();
			await sut.ExecuteAsync(deleteMusicCommandMock.Object);

			deleteMusicCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertMediaIdentifierWasCalledOnDeleteMusicCommand()
		{
			ICommandHandler<IDeleteMusicCommand> sut = CreateSut();

			Mock<IDeleteMusicCommand> deleteMusicCommandMock = BuildDeleteMusicCommandMock();
			await sut.ExecuteAsync(deleteMusicCommandMock.Object);

			deleteMusicCommandMock.Verify(m => m.MediaIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertDeleteMediaAsyncWasCalledOnMediaLibraryRepositoryWithMediaIdentifierOnDeleteMusicCommand()
		{
			ICommandHandler<IDeleteMusicCommand> sut = CreateSut();

			Guid mediaIdentifier = Guid.NewGuid();
			IDeleteMusicCommand deleteMusicCommand = BuildDeleteMusicCommand(mediaIdentifier);
			await sut.ExecuteAsync(deleteMusicCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.DeleteMediaAsync<IMusic>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		private ICommandHandler<IDeleteMusicCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.DeleteMediaAsync<IMusic>(It.IsAny<Guid>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.DeleteMusicCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IDeleteMusicCommand BuildDeleteMusicCommand(Guid? mediaIdentifier = null)
		{
			return BuildDeleteMusicCommandMock(mediaIdentifier).Object;
		}

		private Mock<IDeleteMusicCommand> BuildDeleteMusicCommandMock(Guid? mediaIdentifier = null)
		{
			Mock<IDeleteMusicCommand> deleteMusicCommandMock = new Mock<IDeleteMusicCommand>();
			deleteMusicCommandMock.Setup(m => m.MediaIdentifier)
				.Returns(mediaIdentifier ?? Guid.NewGuid());
			return deleteMusicCommandMock;
		}
	}
}