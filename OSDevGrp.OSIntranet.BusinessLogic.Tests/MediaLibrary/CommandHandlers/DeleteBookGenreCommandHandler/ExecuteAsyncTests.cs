using AutoFixture;
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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.DeleteBookGenreCommandHandler
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
			ICommandHandler<IDeleteBookGenreCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteBookGenreCommand()
		{
			ICommandHandler<IDeleteBookGenreCommand> sut = CreateSut();

			Mock<IDeleteBookGenreCommand> deleteBookGenreCommandMock = BuildDeleteBookGenreCommandMock();
			await sut.ExecuteAsync(deleteBookGenreCommandMock.Object);

			deleteBookGenreCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<bool>>(),
					It.IsNotNull<Func<int, Task<IBookGenre>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertNumberWasCalledOnDeleteBookGenreCommand()
		{
			ICommandHandler<IDeleteBookGenreCommand> sut = CreateSut();

			Mock<IDeleteBookGenreCommand> deleteBookGenreCommandMock = BuildDeleteBookGenreCommandMock();
			await sut.ExecuteAsync(deleteBookGenreCommandMock.Object);

			deleteBookGenreCommandMock.Verify(m => m.Number, Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertDeleteBookGenreAsyncWasCalledOnMediaLibraryRepositoryWithNumberFromDeleteBookGenreCommand()
		{
			ICommandHandler<IDeleteBookGenreCommand> sut = CreateSut();

			int number = _fixture.Create<int>();
			await sut.ExecuteAsync(BuildDeleteBookGenreCommand(number));

			_mediaLibraryRepositoryMock.Verify(m => m.DeleteBookGenreAsync(It.Is<int>(value => value == number)), Times.Once());
		}

		private ICommandHandler<IDeleteBookGenreCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.DeleteBookGenreAsync(It.IsAny<int>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.DeleteBookGenreCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private IDeleteBookGenreCommand BuildDeleteBookGenreCommand(int? number = null)
		{
			return BuildDeleteBookGenreCommandMock(number).Object;
		}

		private Mock<IDeleteBookGenreCommand> BuildDeleteBookGenreCommandMock(int? number = null)
		{
			Mock<IDeleteBookGenreCommand> deleteBookGenreCommandMock = new Mock<IDeleteBookGenreCommand>();
			deleteBookGenreCommandMock.Setup(m => m.Number)
				.Returns(number ?? _fixture.Create<int>());
			return deleteBookGenreCommandMock;
		}
	}
}