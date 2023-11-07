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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.UpdateBookGenreCommandHandler
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
			ICommandHandler<IUpdateBookGenreCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnUpdateBookGenreCommand()
		{
			ICommandHandler<IUpdateBookGenreCommand> sut = CreateSut();

			Mock<IUpdateBookGenreCommand> updateBookGenreCommandMock = BuildUpdateBookGenreCommandMock();
			await sut.ExecuteAsync(updateBookGenreCommandMock.Object);

			updateBookGenreCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<bool>>(),
					It.IsNotNull<Func<int, Task<IBookGenre>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnUpdateBookGenreCommand()
		{
			ICommandHandler<IUpdateBookGenreCommand> sut = CreateSut();

			Mock<IUpdateBookGenreCommand> updateBookGenreCommandMock = BuildUpdateBookGenreCommandMock();
			await sut.ExecuteAsync(updateBookGenreCommandMock.Object);

			updateBookGenreCommandMock.Verify(m => m.ToDomain(), Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertUpdateBookGenreAsyncWasCalledOnMediaLibraryRepositoryWithBookGenreFromUpdateBookGenreCommand()
		{
			ICommandHandler<IUpdateBookGenreCommand> sut = CreateSut();

			IBookGenre bookGenre = _fixture.BuildBookGenreMock().Object;
			await sut.ExecuteAsync(BuildUpdateBookGenreCommand(bookGenre));

			_mediaLibraryRepositoryMock.Verify(m => m.UpdateBookGenreAsync(It.Is<IBookGenre>(value => value != null && value == bookGenre)), Times.Once());
		}

		private ICommandHandler<IUpdateBookGenreCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.UpdateBookGenreAsync(It.IsAny<IBookGenre>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.UpdateBookGenreCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private IUpdateBookGenreCommand BuildUpdateBookGenreCommand(IBookGenre bookGenre = null)
		{
			return BuildUpdateBookGenreCommandMock(bookGenre).Object;
		}

		private Mock<IUpdateBookGenreCommand> BuildUpdateBookGenreCommandMock(IBookGenre bookGenre = null)
		{
			Mock<IUpdateBookGenreCommand> updateBookGenreCommandMock = new Mock<IUpdateBookGenreCommand>();
			updateBookGenreCommandMock.Setup(m => m.ToDomain())
				.Returns(bookGenre ?? _fixture.BuildBookGenreMock().Object);
			return updateBookGenreCommandMock;
		}
	}
}