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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.DeleteMovieCommandHandler
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
			ICommandHandler<IDeleteMovieCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteMovieCommand()
		{
			ICommandHandler<IDeleteMovieCommand> sut = CreateSut();

			Mock<IDeleteMovieCommand> deleteMovieCommandMock = BuildDeleteMovieCommandMock();
			await sut.ExecuteAsync(deleteMovieCommandMock.Object);

			deleteMovieCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertMediaIdentifierWasCalledOnDeleteMovieCommand()
		{
			ICommandHandler<IDeleteMovieCommand> sut = CreateSut();

			Mock<IDeleteMovieCommand> deleteMovieCommandMock = BuildDeleteMovieCommandMock();
			await sut.ExecuteAsync(deleteMovieCommandMock.Object);

			deleteMovieCommandMock.Verify(m => m.MediaIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertDeleteMediaAsyncWasCalledOnMediaLibraryRepositoryWithMediaIdentifierOnDeleteMovieCommand()
		{
			ICommandHandler<IDeleteMovieCommand> sut = CreateSut();

			Guid mediaIdentifier = Guid.NewGuid();
			IDeleteMovieCommand deleteMovieCommand = BuildDeleteMovieCommand(mediaIdentifier);
			await sut.ExecuteAsync(deleteMovieCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.DeleteMediaAsync<IMovie>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		private ICommandHandler<IDeleteMovieCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.DeleteMediaAsync<IMovie>(It.IsAny<Guid>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.DeleteMovieCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IDeleteMovieCommand BuildDeleteMovieCommand(Guid? mediaIdentifier = null)
		{
			return BuildDeleteMovieCommandMock(mediaIdentifier).Object;
		}

		private Mock<IDeleteMovieCommand> BuildDeleteMovieCommandMock(Guid? mediaIdentifier = null)
		{
			Mock<IDeleteMovieCommand> deleteMovieCommandMock = new Mock<IDeleteMovieCommand>();
			deleteMovieCommandMock.Setup(m => m.MediaIdentifier)
				.Returns(mediaIdentifier ?? Guid.NewGuid());
			return deleteMovieCommandMock;
		}
	}
}