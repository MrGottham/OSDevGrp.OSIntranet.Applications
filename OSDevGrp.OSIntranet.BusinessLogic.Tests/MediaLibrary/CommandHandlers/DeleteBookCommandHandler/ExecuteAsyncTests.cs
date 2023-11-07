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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.DeleteBookCommandHandler
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
			ICommandHandler<IDeleteBookCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteBookCommand()
		{
			ICommandHandler<IDeleteBookCommand> sut = CreateSut();

			Mock<IDeleteBookCommand> deleteBookCommandMock = BuildDeleteBookCommandMock();
			await sut.ExecuteAsync(deleteBookCommandMock.Object);

			deleteBookCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertMediaIdentifierWasCalledOnDeleteBookCommand()
		{
			ICommandHandler<IDeleteBookCommand> sut = CreateSut();

			Mock<IDeleteBookCommand> deleteBookCommandMock = BuildDeleteBookCommandMock();
			await sut.ExecuteAsync(deleteBookCommandMock.Object);

			deleteBookCommandMock.Verify(m => m.MediaIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertDeleteMediaAsyncWasCalledOnMediaLibraryRepositoryWithMediaIdentifierOnDeleteBookCommand()
		{
			ICommandHandler<IDeleteBookCommand> sut = CreateSut();

			Guid mediaIdentifier = Guid.NewGuid();
			IDeleteBookCommand deleteBookCommand = BuildDeleteBookCommand(mediaIdentifier);
			await sut.ExecuteAsync(deleteBookCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.DeleteMediaAsync<IBook>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		private ICommandHandler<IDeleteBookCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.DeleteMediaAsync<IBook>(It.IsAny<Guid>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.DeleteBookCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IDeleteBookCommand BuildDeleteBookCommand(Guid? mediaIdentifier = null)
		{
			return BuildDeleteBookCommandMock(mediaIdentifier).Object;
		}

		private Mock<IDeleteBookCommand> BuildDeleteBookCommandMock(Guid? mediaIdentifier = null)
		{
			Mock<IDeleteBookCommand> deleteBookCommandMock = new Mock<IDeleteBookCommand>();
			deleteBookCommandMock.Setup(m => m.MediaIdentifier)
				.Returns(mediaIdentifier ?? Guid.NewGuid());
			return deleteBookCommandMock;
		}
	}
}