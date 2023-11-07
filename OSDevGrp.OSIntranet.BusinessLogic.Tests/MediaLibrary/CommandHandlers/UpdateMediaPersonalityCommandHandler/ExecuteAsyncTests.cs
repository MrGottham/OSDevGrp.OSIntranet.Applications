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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.UpdateMediaPersonalityCommandHandler
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
			ICommandHandler<IUpdateMediaPersonalityCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnUpdateMediaPersonalityCommand()
		{
			ICommandHandler<IUpdateMediaPersonalityCommand> sut = CreateSut();

			Mock<IUpdateMediaPersonalityCommand> updateMediaPersonalityCommandMock = BuildUpdateMediaPersonalityCommandMock();
			await sut.ExecuteAsync(updateMediaPersonalityCommandMock.Object);

			updateMediaPersonalityCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnUpdateMediaPersonalityCommand()
		{
			ICommandHandler<IUpdateMediaPersonalityCommand> sut = CreateSut();

			Mock<IUpdateMediaPersonalityCommand> updateMediaPersonalityCommandMock = BuildUpdateMediaPersonalityCommandMock();
			await sut.ExecuteAsync(updateMediaPersonalityCommandMock.Object);

			updateMediaPersonalityCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertUpdateMediaPersonalityAsyncWasCalledOnMediaLibraryRepositoryWithMediaPersonalityFromToDomainAsyncOnUpdateMediaPersonalityCommand()
		{
			ICommandHandler<IUpdateMediaPersonalityCommand> sut = CreateSut();

			IMediaPersonality mediaPersonality = _fixture.BuildMediaPersonalityMock().Object;
			IUpdateMediaPersonalityCommand updateMediaPersonalityCommand = BuildUpdateMediaPersonalityCommand(mediaPersonality);
			await sut.ExecuteAsync(updateMediaPersonalityCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.UpdateMediaPersonalityAsync(It.Is<IMediaPersonality>(value => value != null && value == mediaPersonality)), Times.Once);
		}

		private ICommandHandler<IUpdateMediaPersonalityCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.UpdateMediaPersonalityAsync(It.IsAny<IMediaPersonality>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.UpdateMediaPersonalityCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IUpdateMediaPersonalityCommand BuildUpdateMediaPersonalityCommand(IMediaPersonality mediaPersonality = null)
		{
			return BuildUpdateMediaPersonalityCommandMock(mediaPersonality).Object;
		}

		private Mock<IUpdateMediaPersonalityCommand> BuildUpdateMediaPersonalityCommandMock(IMediaPersonality mediaPersonality = null)
		{
			Mock<IUpdateMediaPersonalityCommand> updateMediaPersonalityCommandMock = new Mock<IUpdateMediaPersonalityCommand>();
			updateMediaPersonalityCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(mediaPersonality ?? _fixture.BuildMediaPersonalityMock().Object));
			return updateMediaPersonalityCommandMock;
		}
	}
}