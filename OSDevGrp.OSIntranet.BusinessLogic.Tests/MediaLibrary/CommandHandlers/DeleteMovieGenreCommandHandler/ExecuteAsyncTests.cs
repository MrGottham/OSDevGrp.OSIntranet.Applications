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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.DeleteMovieGenreCommandHandler
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
			ICommandHandler<IDeleteMovieGenreCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteMovieGenreCommand()
		{
			ICommandHandler<IDeleteMovieGenreCommand> sut = CreateSut();

			Mock<IDeleteMovieGenreCommand> deleteMovieGenreCommandMock = BuildDeleteMovieGenreCommandMock();
			await sut.ExecuteAsync(deleteMovieGenreCommandMock.Object);

			deleteMovieGenreCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<int, Task<IMovieGenre>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertNumberWasCalledOnDeleteMovieGenreCommand()
		{
			ICommandHandler<IDeleteMovieGenreCommand> sut = CreateSut();

			Mock<IDeleteMovieGenreCommand> deleteMovieGenreCommandMock = BuildDeleteMovieGenreCommandMock();
			await sut.ExecuteAsync(deleteMovieGenreCommandMock.Object);

			deleteMovieGenreCommandMock.Verify(m => m.Number, Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertDeleteMovieGenreAsyncWasCalledOnMediaLibraryRepositoryWithNumberFromDeleteMovieGenreCommand()
		{
			ICommandHandler<IDeleteMovieGenreCommand> sut = CreateSut();

			int number = _fixture.Create<int>();
			await sut.ExecuteAsync(BuildDeleteMovieGenreCommand(number));

			_mediaLibraryRepositoryMock.Verify(m => m.DeleteMovieGenreAsync(It.Is<int>(value => value == number)), Times.Once());
		}

		private ICommandHandler<IDeleteMovieGenreCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.DeleteMovieGenreAsync(It.IsAny<int>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.DeleteMovieGenreCommandHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private IDeleteMovieGenreCommand BuildDeleteMovieGenreCommand(int? number = null)
		{
			return BuildDeleteMovieGenreCommandMock(number).Object;
		}

		private Mock<IDeleteMovieGenreCommand> BuildDeleteMovieGenreCommandMock(int? number = null)
		{
			Mock<IDeleteMovieGenreCommand> deleteMovieGenreCommandMock = new Mock<IDeleteMovieGenreCommand>();
			deleteMovieGenreCommandMock.Setup(m => m.Number)
				.Returns(number ?? _fixture.Create<int>());
			return deleteMovieGenreCommandMock;
		}
	}
}