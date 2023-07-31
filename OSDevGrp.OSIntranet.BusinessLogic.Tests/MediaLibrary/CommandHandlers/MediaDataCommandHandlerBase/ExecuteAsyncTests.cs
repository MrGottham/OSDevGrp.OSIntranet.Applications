using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.MediaDataCommandHandlerBase
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
			ICommandHandler<IMediaDataCommand<IMedia>> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnMediaDataCommand()
		{
			ICommandHandler<IMediaDataCommand<IMedia>> sut = CreateSut();

			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock();
			await sut.ExecuteAsync(mediaDataCommandMock.Object);

			mediaDataCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnMediaDataCommand()
		{
			ICommandHandler<IMediaDataCommand<IMedia>> sut = CreateSut();

			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock();
			await sut.ExecuteAsync(mediaDataCommandMock.Object);

			mediaDataCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledOnMediaDataCommandHandlerBase()
		{
			ICommandHandler<IMediaDataCommand<IMedia>> sut = CreateSut();

			await sut.ExecuteAsync(CreateMediaDataCommand());

			Assert.That(((MyMediaDataCommandHandler) sut).ManageAsyncWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledOnMediaDataCommandHandlerBaseWithSameMediaFromToDomainAsyncOnMediaDataCommand()
		{
			ICommandHandler<IMediaDataCommand<IMedia>> sut = CreateSut();

			IMedia media = _fixture.BuildMediaMock().Object;
			await sut.ExecuteAsync(CreateMediaDataCommand(media));

			Assert.That(((MyMediaDataCommandHandler) sut).ManageAsyncCalledWithMedia, Is.SameAs(media));
		}

		private ICommandHandler<IMediaDataCommand<IMedia>> CreateSut()
		{
			return new MyMediaDataCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IMediaDataCommand<IMedia> CreateMediaDataCommand(IMedia toDomainAsync = null)
		{
			return CreateMediaDataCommandMock(toDomainAsync).Object;
		}

		private Mock<IMediaDataCommand<IMedia>> CreateMediaDataCommandMock(IMedia toDomainAsync = null)
		{
			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = new Mock<IMediaDataCommand<IMedia>>();
			mediaDataCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(toDomainAsync ?? _fixture.BuildMediaMock().Object));
			return mediaDataCommandMock;
		}

		private class MyMediaDataCommandHandler : MediaDataCommandHandlerBase<IMediaDataCommand<IMedia>, IMedia>
		{
			#region Constructor

			public MyMediaDataCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
				: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
			{
			}

			#endregion

			#region Properties

			public bool ManageAsyncWasCalled { get; private set; }

			public IMedia ManageAsyncCalledWithMedia { get; private set; }

			#endregion

			#region Methods

			protected override Task ManageAsync(IMedia media)
			{
				NullGuard.NotNull(media, nameof(media));

				ManageAsyncWasCalled = true;
				ManageAsyncCalledWithMedia = media;

				return Task.CompletedTask;
			}

			#endregion
		}
	}
}