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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.MediaLibraryCommandHandlerBase
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
			ICommandHandler<IMediaLibraryCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnMediaLibraryCommand()
		{
			ICommandHandler<IMediaLibraryCommand> sut = CreateSut();

			Mock<IMediaLibraryCommand> mediaLibraryCommandMock = CreateMediaLibraryCommandMock();
			await sut.ExecuteAsync(mediaLibraryCommandMock.Object);

			mediaLibraryCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledWasCalledOnMediaLibraryCommandHandlerBase()
		{
			ICommandHandler<IMediaLibraryCommand> sut = CreateSut();

			await sut.ExecuteAsync(CreateMediaLibraryCommand());

			Assert.That(((MyMediaLibraryCommandHandler) sut).ManageAsyncWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledWasCalledOnMediaLibraryCommandHandlerBaseWithSameMediaLibraryCommand()
		{
			ICommandHandler<IMediaLibraryCommand> sut = CreateSut();

			IMediaLibraryCommand mediaLibraryCommand = CreateMediaLibraryCommand();
			await sut.ExecuteAsync(mediaLibraryCommand);

			Assert.That(((MyMediaLibraryCommandHandler) sut).ManageAsyncCalledWithMediaLibraryCommand, Is.SameAs(mediaLibraryCommand));
		}

		private ICommandHandler<IMediaLibraryCommand> CreateSut()
		{
			return new MyMediaLibraryCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private static IMediaLibraryCommand CreateMediaLibraryCommand()
		{
			return CreateMediaLibraryCommandMock().Object;
		}

		private static Mock<IMediaLibraryCommand> CreateMediaLibraryCommandMock()
		{
			return new Mock<IMediaLibraryCommand>();
		}

		private class MyMediaLibraryCommandHandler : MediaLibraryCommandHandlerBase<IMediaLibraryCommand>
		{
			#region Constructor

			public MyMediaLibraryCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
				: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
			{
			}

			#endregion

			#region Properties

			public bool ManageAsyncWasCalled { get; private set; }

			public IMediaLibraryCommand ManageAsyncCalledWithMediaLibraryCommand { get; private set; }

			#endregion

			#region Methods

			protected override Task ManageAsync(IMediaLibraryCommand command)
			{
				NullGuard.NotNull(command, nameof(command));

				ManageAsyncWasCalled = true;
				ManageAsyncCalledWithMediaLibraryCommand = command;

				return Task.CompletedTask;
			}

			#endregion
		}
	}
}