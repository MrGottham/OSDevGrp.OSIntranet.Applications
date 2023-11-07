using System;
using System.Threading.Tasks;
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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.CreateMusicCommandHandler
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
			ICommandHandler<ICreateMusicCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCreateMusicCommand()
		{
			ICommandHandler<ICreateMusicCommand> sut = CreateSut();

			Mock<ICreateMusicCommand> createMusicCommandMock = BuildCreateMusicCommandMock();
			await sut.ExecuteAsync(createMusicCommandMock.Object);

			createMusicCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnCreateMusicCommand()
		{
			ICommandHandler<ICreateMusicCommand> sut = CreateSut();

			Mock<ICreateMusicCommand> createMusicCommandMock = BuildCreateMusicCommandMock();
			await sut.ExecuteAsync(createMusicCommandMock.Object);

			createMusicCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertCreateMediaAsyncWasCalledOnMediaLibraryRepositoryWithMusicFromToDomainAsyncOnCreateMusicCommand()
		{
			ICommandHandler<ICreateMusicCommand> sut = CreateSut();

			IMusic music = _fixture.BuildMusicMock().Object;
			ICreateMusicCommand createMusicCommand = BuildCreateMusicCommand(music);
			await sut.ExecuteAsync(createMusicCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.CreateMediaAsync(It.Is<IMusic>(value => value != null && value == music)), Times.Once);
		}

		private ICommandHandler<ICreateMusicCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.CreateMediaAsync(It.IsAny<IMusic>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.CreateMusicCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private ICreateMusicCommand BuildCreateMusicCommand(IMusic music = null)
		{
			return BuildCreateMusicCommandMock(music).Object;
		}

		private Mock<ICreateMusicCommand> BuildCreateMusicCommandMock(IMusic music = null)
		{
			Mock<ICreateMusicCommand> createMusicCommandMock = new Mock<ICreateMusicCommand>();
			createMusicCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(music ?? _fixture.BuildMusicMock().Object));
			return createMusicCommandMock;
		}
	}
}