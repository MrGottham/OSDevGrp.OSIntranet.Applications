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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.LendingIdentificationCommandHandlerBase
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
			ICommandHandler<ILendingIdentificationCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnLendingIdentificationCommand()
		{
			ICommandHandler<ILendingIdentificationCommand> sut = CreateSut();

			Mock<ILendingIdentificationCommand> lendingIdentificationCommandMock = CreateLendingIdentificationCommandMock();
			await sut.ExecuteAsync(lendingIdentificationCommandMock.Object);

			lendingIdentificationCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledWasCalledOnLendingIdentificationCommandHandlerBase()
		{
			ICommandHandler<ILendingIdentificationCommand> sut = CreateSut();

			await sut.ExecuteAsync(CreateLendingIdentificationCommand());

			Assert.That(((MyLendingIdentificationCommandHandler) sut).ManageAsyncWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledWasCalledOnLendingIdentificationCommandHandlerBaseWithSameLendingIdentificationCommand()
		{
			ICommandHandler<ILendingIdentificationCommand> sut = CreateSut();

			ILendingIdentificationCommand lendingIdentificationCommand = CreateLendingIdentificationCommand();
			await sut.ExecuteAsync(lendingIdentificationCommand);

			Assert.That(((MyLendingIdentificationCommandHandler) sut).ManageAsyncCalledWithLendingIdentificationCommand, Is.SameAs(lendingIdentificationCommand));
		}

		private ICommandHandler<ILendingIdentificationCommand> CreateSut()
		{
			return new MyLendingIdentificationCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private static ILendingIdentificationCommand CreateLendingIdentificationCommand()
		{
			return CreateLendingIdentificationCommandMock().Object;
		}

		private static Mock<ILendingIdentificationCommand> CreateLendingIdentificationCommandMock()
		{
			return new Mock<ILendingIdentificationCommand>();
		}

		private class MyLendingIdentificationCommandHandler : LendingIdentificationCommandHandlerBase<ILendingIdentificationCommand>
		{
			#region Constructor

			public MyLendingIdentificationCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
				: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
			{
			}

			#endregion

			#region Properties

			public bool ManageAsyncWasCalled { get; private set; }

			public ILendingIdentificationCommand ManageAsyncCalledWithLendingIdentificationCommand { get; private set; }

			#endregion

			#region Methods

			protected override Task ManageAsync(ILendingIdentificationCommand command)
			{
				NullGuard.NotNull(command, nameof(command));

				ManageAsyncWasCalled = true;
				ManageAsyncCalledWithLendingIdentificationCommand = command;

				return Task.CompletedTask;
			}

			#endregion
		}
	}
}