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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.BorrowerIdentificationCommandHandlerBase
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
			ICommandHandler<IBorrowerIdentificationCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnBorrowerIdentificationCommand()
		{
			ICommandHandler<IBorrowerIdentificationCommand> sut = CreateSut();

			Mock<IBorrowerIdentificationCommand> borrowerIdentificationCommandMock = CreateBorrowerIdentificationCommandMock();
			await sut.ExecuteAsync(borrowerIdentificationCommandMock.Object);

			borrowerIdentificationCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledWasCalledOnBorrowerIdentificationCommandHandlerBase()
		{
			ICommandHandler<IBorrowerIdentificationCommand> sut = CreateSut();

			await sut.ExecuteAsync(CreateBorrowerIdentificationCommand());

			Assert.That(((MyBorrowerIdentificationCommandHandler) sut).ManageAsyncWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledWasCalledOnBorrowerIdentificationCommandHandlerBaseWithSameBorrowerIdentificationCommand()
		{
			ICommandHandler<IBorrowerIdentificationCommand> sut = CreateSut();

			IBorrowerIdentificationCommand borrowerIdentificationCommand = CreateBorrowerIdentificationCommand();
			await sut.ExecuteAsync(borrowerIdentificationCommand);

			Assert.That(((MyBorrowerIdentificationCommandHandler) sut).ManageAsyncCalledWithBorrowerIdentificationCommand, Is.SameAs(borrowerIdentificationCommand));
		}

		private ICommandHandler<IBorrowerIdentificationCommand> CreateSut()
		{
			return new MyBorrowerIdentificationCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private static IBorrowerIdentificationCommand CreateBorrowerIdentificationCommand()
		{
			return CreateBorrowerIdentificationCommandMock().Object;
		}

		private static Mock<IBorrowerIdentificationCommand> CreateBorrowerIdentificationCommandMock()
		{
			return new Mock<IBorrowerIdentificationCommand>();
		}

		private class MyBorrowerIdentificationCommandHandler : BorrowerIdentificationCommandHandlerBase<IBorrowerIdentificationCommand>
		{
			#region Constructor

			public MyBorrowerIdentificationCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
				: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
			{
			}

			#endregion

			#region Properties

			public bool ManageAsyncWasCalled { get; private set; }

			public IBorrowerIdentificationCommand ManageAsyncCalledWithBorrowerIdentificationCommand { get; private set; }

			#endregion

			#region Methods

			protected override Task ManageAsync(IBorrowerIdentificationCommand command)
			{
				NullGuard.NotNull(command, nameof(command));

				ManageAsyncWasCalled = true;
				ManageAsyncCalledWithBorrowerIdentificationCommand = command;

				return Task.CompletedTask;
			}

			#endregion
		}
	}
}