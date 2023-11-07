using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.MediaPersonalityDataCommandHandlerBase
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
			ICommandHandler<IMediaPersonalityDataCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnMediaPersonalityDataCommand()
		{
			ICommandHandler<IMediaPersonalityDataCommand> sut = CreateSut();

			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();
			await sut.ExecuteAsync(mediaPersonalityDataCommandMock.Object);

			mediaPersonalityDataCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnMediaPersonalityDataCommand()
		{
			ICommandHandler<IMediaPersonalityDataCommand> sut = CreateSut();

			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();
			await sut.ExecuteAsync(mediaPersonalityDataCommandMock.Object);

			mediaPersonalityDataCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledOnMediaPersonalityDataCommandHandlerBase()
		{
			ICommandHandler<IMediaPersonalityDataCommand> sut = CreateSut();

			await sut.ExecuteAsync(CreateMediaPersonalityDataCommand());

			Assert.That(((MyMediaPersonalityDataCommandHandler) sut).ManageAsyncWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledOnMediaPersonalityDataCommandHandlerBaseWithMediaPersonalityFromToDomainAsyncOnMediaPersonalityDataCommand()
		{
			ICommandHandler<IMediaPersonalityDataCommand> sut = CreateSut();

			IMediaPersonality mediaPersonality = _fixture.BuildMediaPersonalityMock().Object;
			await sut.ExecuteAsync(CreateMediaPersonalityDataCommand(mediaPersonality));

			Assert.That(((MyMediaPersonalityDataCommandHandler) sut).ManageAsyncCalledWithMediaPersonality, Is.EqualTo(mediaPersonality));
		}

		private ICommandHandler<IMediaPersonalityDataCommand> CreateSut()
		{
			return new MyMediaPersonalityDataCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IMediaPersonalityDataCommand CreateMediaPersonalityDataCommand(IMediaPersonality toDomainAsync = null)
		{
			return CreateMediaPersonalityDataCommandMock(toDomainAsync).Object;
		}

		private Mock<IMediaPersonalityDataCommand> CreateMediaPersonalityDataCommandMock(IMediaPersonality toDomainAsync = null)
		{
			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = new Mock<IMediaPersonalityDataCommand>();
			mediaPersonalityDataCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(toDomainAsync ?? _fixture.BuildMediaPersonalityMock().Object));
			return mediaPersonalityDataCommandMock;
		}

		private class MyMediaPersonalityDataCommandHandler : MediaPersonalityDataCommandHandlerBase<IMediaPersonalityDataCommand>
		{
			#region Constructor

			public MyMediaPersonalityDataCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
				: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
			{
			}

			#endregion

			#region Properties

			public bool ManageAsyncWasCalled { get; private set; }

			public IMediaPersonality ManageAsyncCalledWithMediaPersonality { get; private set; }

			#endregion

			#region Methods

			protected override Task ManageAsync(IMediaPersonality mediaPersonality)
			{
				NullGuard.NotNull(mediaPersonality, nameof(mediaPersonality));

				ManageAsyncWasCalled = true;
				ManageAsyncCalledWithMediaPersonality = mediaPersonality;

				return Task.CompletedTask;
			}

			#endregion
		}
	}
}