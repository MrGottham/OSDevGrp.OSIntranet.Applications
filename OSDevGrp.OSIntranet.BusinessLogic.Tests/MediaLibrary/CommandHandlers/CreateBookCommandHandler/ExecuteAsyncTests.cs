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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.CreateBookCommandHandler
{
	[TestFixture]
	public class ExecuteAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<IClaimResolver> _claimResolverMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_claimResolverMock = new Mock<IClaimResolver>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<ICreateBookCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCreateBookCommand()
		{
			ICommandHandler<ICreateBookCommand> sut = CreateSut();

			Mock<ICreateBookCommand> createBookCommandMock = BuildCreateBookCommandMock();
			await sut.ExecuteAsync(createBookCommandMock.Object);

			createBookCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnCreateBookCommand()
		{
			ICommandHandler<ICreateBookCommand> sut = CreateSut();

			Mock<ICreateBookCommand> createBookCommandMock = BuildCreateBookCommandMock();
			await sut.ExecuteAsync(createBookCommandMock.Object);

			createBookCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertCreateMediaAsyncWasCalledOnMediaLibraryRepositoryWithBookFromToDomainAsyncOnCreateBookCommand()
		{
			ICommandHandler<ICreateBookCommand> sut = CreateSut();

			IBook book = _fixture.BuildBookMock().Object;
			ICreateBookCommand createBookCommand = BuildCreateBookCommand(book);
			await sut.ExecuteAsync(createBookCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.CreateMediaAsync(It.Is<IBook>(value => value != null && value == book)), Times.Once);
		}

		private ICommandHandler<ICreateBookCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.CreateMediaAsync(It.IsAny<IBook>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.CreateBookCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private ICreateBookCommand BuildCreateBookCommand(IBook book = null)
		{
			return BuildCreateBookCommandMock(book).Object;
		}

		private Mock<ICreateBookCommand> BuildCreateBookCommandMock(IBook book = null)
		{
			Mock<ICreateBookCommand> createBookCommandMock = new Mock<ICreateBookCommand>();
			createBookCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(book ?? _fixture.BuildBookMock().Object));
			return createBookCommandMock;
		}
	}
}