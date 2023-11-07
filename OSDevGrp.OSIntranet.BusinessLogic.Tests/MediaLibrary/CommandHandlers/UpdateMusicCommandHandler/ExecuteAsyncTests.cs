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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.UpdateMusicCommandHandler
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
			ICommandHandler<IUpdateMusicCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnUpdateMusicCommand()
		{
			ICommandHandler<IUpdateMusicCommand> sut = CreateSut();

			Mock<IUpdateMusicCommand> updateMusicCommandMock = BuildUpdateMusicCommandMock();
			await sut.ExecuteAsync(updateMusicCommandMock.Object);

			updateMusicCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnUpdateMusicCommand()
		{
			ICommandHandler<IUpdateMusicCommand> sut = CreateSut();

			Mock<IUpdateMusicCommand> updateMusicCommandMock = BuildUpdateMusicCommandMock();
			await sut.ExecuteAsync(updateMusicCommandMock.Object);

			updateMusicCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertUpdateMediaAsyncWasCalledOnMediaLibraryRepositoryWithMusicFromToDomainAsyncOnUpdateMusicCommand()
		{
			ICommandHandler<IUpdateMusicCommand> sut = CreateSut();

			IMusic music = _fixture.BuildMusicMock().Object;
			IUpdateMusicCommand updateMusicCommand = BuildUpdateMusicCommand(music);
			await sut.ExecuteAsync(updateMusicCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.UpdateMediaAsync(It.Is<IMusic>(value => value != null && value == music)), Times.Once);
		}

		private ICommandHandler<IUpdateMusicCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.UpdateMediaAsync(It.IsAny<IMusic>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.UpdateMusicCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IUpdateMusicCommand BuildUpdateMusicCommand(IMusic music = null)
		{
			return BuildUpdateMusicCommandMock(music).Object;
		}

		private Mock<IUpdateMusicCommand> BuildUpdateMusicCommandMock(IMusic music = null)
		{
			Mock<IUpdateMusicCommand> updateMusicCommandMock = new Mock<IUpdateMusicCommand>();
			updateMusicCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(music ?? _fixture.BuildMusicMock().Object));
			return updateMusicCommandMock;
		}
	}
}