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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.CreateMovieGenreCommandHandler
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
			ICommandHandler<ICreateMovieGenreCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCreateMovieGenreCommand()
		{
			ICommandHandler<ICreateMovieGenreCommand> sut = CreateSut();

			Mock<ICreateMovieGenreCommand> createMovieGenreCommandMock = BuildCreateMovieGenreCommandMock();
			await sut.ExecuteAsync(createMovieGenreCommandMock.Object);

			createMovieGenreCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<bool>>(),
					It.IsNotNull<Func<int, Task<IMovieGenre>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCreateMovieGenreCommand()
		{
			ICommandHandler<ICreateMovieGenreCommand> sut = CreateSut();

			Mock<ICreateMovieGenreCommand> createMovieGenreCommandMock = BuildCreateMovieGenreCommandMock();
			await sut.ExecuteAsync(createMovieGenreCommandMock.Object);

			createMovieGenreCommandMock.Verify(m => m.ToDomain(), Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertCreateMovieGenreAsyncWasCalledOnMediaLibraryRepositoryWithMovieGenreFromCreateMovieGenreCommand()
		{
			ICommandHandler<ICreateMovieGenreCommand> sut = CreateSut();

			IMovieGenre movieGenre = _fixture.BuildMovieGenreMock().Object;
			await sut.ExecuteAsync(BuildCreateMovieGenreCommand(movieGenre));

			_mediaLibraryRepositoryMock.Verify(m => m.CreateMovieGenreAsync(It.Is<IMovieGenre>(value => value != null && value == movieGenre)), Times.Once());
		}

		private ICommandHandler<ICreateMovieGenreCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.CreateMovieGenreAsync(It.IsAny<IMovieGenre>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.CreateMovieGenreCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private ICreateMovieGenreCommand BuildCreateMovieGenreCommand(IMovieGenre movieGenre = null)
		{
			return BuildCreateMovieGenreCommandMock(movieGenre).Object;
		}

		private Mock<ICreateMovieGenreCommand> BuildCreateMovieGenreCommandMock(IMovieGenre movieGenre = null)
		{
			Mock<ICreateMovieGenreCommand> createMovieGenreCommandMock = new Mock<ICreateMovieGenreCommand>();
			createMovieGenreCommandMock.Setup(m => m.ToDomain())
				.Returns(movieGenre ?? _fixture.BuildMovieGenreMock().Object);
			return createMovieGenreCommandMock;
		}
	}
}