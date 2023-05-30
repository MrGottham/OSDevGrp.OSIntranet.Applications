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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.CreateMusicGenreCommandHandler
{
	[TestFixture]
	public class ExecuteAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<IClaimResolver> _claimResolverMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_claimResolverMock = new Mock<IClaimResolver>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<ICreateMusicGenreCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCreateMusicGenreCommand()
		{
			ICommandHandler<ICreateMusicGenreCommand> sut = CreateSut();

			Mock<ICreateMusicGenreCommand> createMusicGenreCommandMock = BuildCreateMusicGenreCommandMock();
			await sut.ExecuteAsync(createMusicGenreCommandMock.Object);

			createMusicGenreCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<bool>>(),
					It.IsNotNull<Func<int, Task<IMusicGenre>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCreateMusicGenreCommand()
		{
			ICommandHandler<ICreateMusicGenreCommand> sut = CreateSut();

			Mock<ICreateMusicGenreCommand> createMusicGenreCommandMock = BuildCreateMusicGenreCommandMock();
			await sut.ExecuteAsync(createMusicGenreCommandMock.Object);

			createMusicGenreCommandMock.Verify(m => m.ToDomain(), Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertCreateMusicGenreAsyncWasCalledOnMediaLibraryRepositoryWithMusicGenreFromCreateMusicGenreCommand()
		{
			ICommandHandler<ICreateMusicGenreCommand> sut = CreateSut();

			IMusicGenre musicGenre = _fixture.BuildMusicGenreMock().Object;
			await sut.ExecuteAsync(BuildCreateMusicGenreCommand(musicGenre));

			_mediaLibraryRepositoryMock.Verify(m => m.CreateMusicGenreAsync(It.Is<IMusicGenre>(value => value != null && value == musicGenre)), Times.Once());
		}

		private ICommandHandler<ICreateMusicGenreCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.CreateMusicGenreAsync(It.IsAny<IMusicGenre>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.CreateMusicGenreCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private ICreateMusicGenreCommand BuildCreateMusicGenreCommand(IMusicGenre musicGenre = null)
		{
			return BuildCreateMusicGenreCommandMock(musicGenre).Object;
		}

		private Mock<ICreateMusicGenreCommand> BuildCreateMusicGenreCommandMock(IMusicGenre musicGenre = null)
		{
			Mock<ICreateMusicGenreCommand> createMusicGenreCommandMock = new Mock<ICreateMusicGenreCommand>();
			createMusicGenreCommandMock.Setup(m => m.ToDomain())
				.Returns(musicGenre ?? _fixture.BuildMusicGenreMock().Object);
			return createMusicGenreCommandMock;
		}
	}
}