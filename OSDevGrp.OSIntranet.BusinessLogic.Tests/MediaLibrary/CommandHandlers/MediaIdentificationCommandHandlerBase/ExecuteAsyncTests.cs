using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.MediaIdentificationCommandHandlerBase
{
	[TestFixture]
	public class ExecuteAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<IClaimResolver> _claimResolverMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_claimResolverMock = new Mock<IClaimResolver>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<IMediaIdentificationCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnMediaIdentificationCommand()
		{
			ICommandHandler<IMediaIdentificationCommand> sut = CreateSut();

			Mock<IMediaIdentificationCommand> mediaIdentificationCommandMock = CreateMediaIdentificationCommandMock();
			await sut.ExecuteAsync(mediaIdentificationCommandMock.Object);

			mediaIdentificationCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledWasCalledOnMediaIdentificationCommandHandlerBase()
		{
			ICommandHandler<IMediaIdentificationCommand> sut = CreateSut();

			await sut.ExecuteAsync(CreateMediaIdentificationCommand());

			Assert.That(((MyMediaIdentificationCommandHandler) sut).ManageAsyncWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledWasCalledOnMediaIdentificationCommandHandlerBaseWithSameMediaIdentificationCommand()
		{
			ICommandHandler<IMediaIdentificationCommand> sut = CreateSut();

			IMediaIdentificationCommand mediaIdentificationCommand = CreateMediaIdentificationCommand();
			await sut.ExecuteAsync(mediaIdentificationCommand);

			Assert.That(((MyMediaIdentificationCommandHandler) sut).ManageAsyncCalledWithMediaIdentificationCommand, Is.SameAs(mediaIdentificationCommand));
		}

		private ICommandHandler<IMediaIdentificationCommand> CreateSut()
		{
			return new MyMediaIdentificationCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private static IMediaIdentificationCommand CreateMediaIdentificationCommand()
		{
			return CreateMediaIdentificationCommandMock().Object;
		}

		private static Mock<IMediaIdentificationCommand> CreateMediaIdentificationCommandMock()
		{
			return new Mock<IMediaIdentificationCommand>();
		}

		private class MyMediaIdentificationCommandHandler : MediaIdentificationCommandHandlerBase<IMediaIdentificationCommand>
		{
			#region Constructor

			public MyMediaIdentificationCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
				: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
			{
			}

			#endregion

			#region Properties

			public bool ManageAsyncWasCalled { get; private set; }

			public IMediaIdentificationCommand ManageAsyncCalledWithMediaIdentificationCommand { get; private set; }

			#endregion

			#region Methods

			protected override Task ManageAsync(IMediaIdentificationCommand command)
			{
				NullGuard.NotNull(command, nameof(command));

				ManageAsyncWasCalled = true;
				ManageAsyncCalledWithMediaIdentificationCommand = command;

				return Task.CompletedTask;
			}

			#endregion
		}
	}
}