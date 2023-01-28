using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.DeleteMusicGenreCommandHandler
{
	[TestFixture]
	public class ExecuteAsyncTests
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
		public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<IDeleteMusicGenreCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteMusicGenreCommand()
		{
			ICommandHandler<IDeleteMusicGenreCommand> sut = CreateSut();

			Mock<IDeleteMusicGenreCommand> deleteMusicGenreCommandMock = BuildDeleteMusicGenreCommandMock();
			await sut.ExecuteAsync(deleteMusicGenreCommandMock.Object);

			deleteMusicGenreCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<int, Task<IMusicGenre>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertNumberWasCalledOnDeleteMusicGenreCommand()
		{
			ICommandHandler<IDeleteMusicGenreCommand> sut = CreateSut();

			Mock<IDeleteMusicGenreCommand> deleteMusicGenreCommandMock = BuildDeleteMusicGenreCommandMock();
			await sut.ExecuteAsync(deleteMusicGenreCommandMock.Object);

			deleteMusicGenreCommandMock.Verify(m => m.Number, Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertDeleteMusicGenreAsyncWasCalledOnMediaLibraryRepositoryWithNumberFromDeleteMusicGenreCommand()
		{
			ICommandHandler<IDeleteMusicGenreCommand> sut = CreateSut();

			int number = _fixture.Create<int>();
			await sut.ExecuteAsync(BuildDeleteMusicGenreCommand(number));

			_mediaLibraryRepositoryMock.Verify(m => m.DeleteMusicGenreAsync(It.Is<int>(value => value == number)), Times.Once());
		}

		private ICommandHandler<IDeleteMusicGenreCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.DeleteMusicGenreAsync(It.IsAny<int>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.DeleteMusicGenreCommandHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private IDeleteMusicGenreCommand BuildDeleteMusicGenreCommand(int? number = null)
		{
			return BuildDeleteMusicGenreCommandMock(number).Object;
		}

		private Mock<IDeleteMusicGenreCommand> BuildDeleteMusicGenreCommandMock(int? number = null)
		{
			Mock<IDeleteMusicGenreCommand> deleteMusicGenreCommandMock = new Mock<IDeleteMusicGenreCommand>();
			deleteMusicGenreCommandMock.Setup(m => m.Number)
				.Returns(number ?? _fixture.Create<int>());
			return deleteMusicGenreCommandMock;
		}
	}
}