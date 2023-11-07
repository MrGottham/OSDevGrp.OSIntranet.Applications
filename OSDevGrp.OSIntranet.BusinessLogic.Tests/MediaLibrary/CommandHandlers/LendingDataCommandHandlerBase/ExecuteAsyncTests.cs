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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.LendingDataCommandHandlerBase
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
			ICommandHandler<ILendingDataCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnLendingDataCommand()
		{
			ICommandHandler<ILendingDataCommand> sut = CreateSut();

			Mock<ILendingDataCommand> lendingDataCommandMock = CreateLendingDataCommandMock();
			await sut.ExecuteAsync(lendingDataCommandMock.Object);

			lendingDataCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnLendingDataCommand()
		{
			ICommandHandler<ILendingDataCommand> sut = CreateSut();

			Mock<ILendingDataCommand> lendingDataCommandMock = CreateLendingDataCommandMock();
			await sut.ExecuteAsync(lendingDataCommandMock.Object);

			lendingDataCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledOnLendingDataCommandHandlerBase()
		{
			ICommandHandler<ILendingDataCommand> sut = CreateSut();

			await sut.ExecuteAsync(CreateLendingDataCommand());

			Assert.That(((MyLendingDataCommandHandler) sut).ManageAsyncWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledOnLendingDataCommandHandlerBaseWithLendingFromToDomainAsyncOnLendingDataCommand()
		{
			ICommandHandler<ILendingDataCommand> sut = CreateSut();

			ILending lending = _fixture.BuildLendingMock().Object;
			await sut.ExecuteAsync(CreateLendingDataCommand(lending));

			Assert.That(((MyLendingDataCommandHandler) sut).ManageAsyncCalledWithLending, Is.EqualTo(lending));
		}

		private ICommandHandler<ILendingDataCommand> CreateSut()
		{
			return new MyLendingDataCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private ILendingDataCommand CreateLendingDataCommand(ILending toDomainAsync = null)
		{
			return CreateLendingDataCommandMock(toDomainAsync).Object;
		}

		private Mock<ILendingDataCommand> CreateLendingDataCommandMock(ILending toDomainAsync = null)
		{
			Mock<ILendingDataCommand> lendingDataCommandMock = new Mock<ILendingDataCommand>();
			lendingDataCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(toDomainAsync ?? _fixture.BuildLendingMock().Object));
			return lendingDataCommandMock;
		}

		private class MyLendingDataCommandHandler : LendingDataCommandHandlerBase<ILendingDataCommand>
		{
			#region Constructor

			public MyLendingDataCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
				: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
			{
			}

			#endregion

			#region Properties

			public bool ManageAsyncWasCalled { get; private set; }

			public ILending ManageAsyncCalledWithLending { get; private set; }

			#endregion

			#region Methods

			protected override Task ManageAsync(ILending lending)
			{
				NullGuard.NotNull(lending, nameof(lending));

				ManageAsyncWasCalled = true;
				ManageAsyncCalledWithLending = lending;

				return Task.CompletedTask;
			}

			#endregion
		}
	}
}