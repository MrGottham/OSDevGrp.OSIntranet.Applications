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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.CreateBookGenreCommandHandler
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
			ICommandHandler<ICreateBookGenreCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCreateBookGenreCommand()
		{
			ICommandHandler<ICreateBookGenreCommand> sut = CreateSut();

			Mock<ICreateBookGenreCommand> createBookGenreCommandMock = BuildCreateBookGenreCommandMock();
			await sut.ExecuteAsync(createBookGenreCommandMock.Object);

			createBookGenreCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<bool>>(),
					It.IsNotNull<Func<int, Task<IBookGenre>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCreateBookGenreCommand()
		{
			ICommandHandler<ICreateBookGenreCommand> sut = CreateSut();

			Mock<ICreateBookGenreCommand> createBookGenreCommandMock = BuildCreateBookGenreCommandMock();
			await sut.ExecuteAsync(createBookGenreCommandMock.Object);

			createBookGenreCommandMock.Verify(m => m.ToDomain(), Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertCreateBookGenreAsyncWasCalledOnMediaLibraryRepositoryWithBookGenreFromCreateBookGenreCommand()
		{
			ICommandHandler<ICreateBookGenreCommand> sut = CreateSut();

			IBookGenre bookGenre = _fixture.BuildBookGenreMock().Object;
			await sut.ExecuteAsync(BuildCreateBookGenreCommand(bookGenre));

			_mediaLibraryRepositoryMock.Verify(m => m.CreateBookGenreAsync(It.Is<IBookGenre>(value => value != null && value == bookGenre)), Times.Once());
		}

		private ICommandHandler<ICreateBookGenreCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.CreateBookGenreAsync(It.IsAny<IBookGenre>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.CreateBookGenreCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private ICreateBookGenreCommand BuildCreateBookGenreCommand(IBookGenre bookGenre = null)
		{
			return BuildCreateBookGenreCommandMock(bookGenre).Object;
		}

		private Mock<ICreateBookGenreCommand> BuildCreateBookGenreCommandMock(IBookGenre bookGenre = null)
		{
			Mock<ICreateBookGenreCommand> createBookGenreCommandMock = new Mock<ICreateBookGenreCommand>();
			createBookGenreCommandMock.Setup(m => m.ToDomain())
				.Returns(bookGenre ?? _fixture.BuildBookGenreMock().Object);
			return createBookGenreCommandMock;
		}
	}
}