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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.UpdateMovieGenreCommandHandler
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
			ICommandHandler<IUpdateMovieGenreCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnUpdateMovieGenreCommand()
		{
			ICommandHandler<IUpdateMovieGenreCommand> sut = CreateSut();

			Mock<IUpdateMovieGenreCommand> updateMovieGenreCommandMock = BuildUpdateMovieGenreCommandMock();
			await sut.ExecuteAsync(updateMovieGenreCommandMock.Object);

			updateMovieGenreCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<bool>>(),
					It.IsNotNull<Func<int, Task<IMovieGenre>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnUpdateMovieGenreCommand()
		{
			ICommandHandler<IUpdateMovieGenreCommand> sut = CreateSut();

			Mock<IUpdateMovieGenreCommand> updateMovieGenreCommandMock = BuildUpdateMovieGenreCommandMock();
			await sut.ExecuteAsync(updateMovieGenreCommandMock.Object);

			updateMovieGenreCommandMock.Verify(m => m.ToDomain(), Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertUpdateMovieGenreAsyncWasCalledOnMediaLibraryRepositoryWithMovieGenreFromUpdateMovieGenreCommand()
		{
			ICommandHandler<IUpdateMovieGenreCommand> sut = CreateSut();

			IMovieGenre movieGenre = _fixture.BuildMovieGenreMock().Object;
			await sut.ExecuteAsync(BuildUpdateMovieGenreCommand(movieGenre));

			_mediaLibraryRepositoryMock.Verify(m => m.UpdateMovieGenreAsync(It.Is<IMovieGenre>(value => value != null && value == movieGenre)), Times.Once());
		}

		private ICommandHandler<IUpdateMovieGenreCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.UpdateMovieGenreAsync(It.IsAny<IMovieGenre>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.UpdateMovieGenreCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private IUpdateMovieGenreCommand BuildUpdateMovieGenreCommand(IMovieGenre movieGenre = null)
		{
			return BuildUpdateMovieGenreCommandMock(movieGenre).Object;
		}

		private Mock<IUpdateMovieGenreCommand> BuildUpdateMovieGenreCommandMock(IMovieGenre movieGenre = null)
		{
			Mock<IUpdateMovieGenreCommand> updateMovieGenreCommandMock = new Mock<IUpdateMovieGenreCommand>();
			updateMovieGenreCommandMock.Setup(m => m.ToDomain())
				.Returns(movieGenre ?? _fixture.BuildMovieGenreMock().Object);
			return updateMovieGenreCommandMock;
		}
	}
}