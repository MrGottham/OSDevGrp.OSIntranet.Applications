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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.BorrowerDataCommandHandlerBase
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
			ICommandHandler<IBorrowerDataCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnBorrowerDataCommand()
		{
			ICommandHandler<IBorrowerDataCommand> sut = CreateSut();

			Mock<IBorrowerDataCommand> borrowerDataCommandMock = CreateBorrowerDataCommandMock();
			await sut.ExecuteAsync(borrowerDataCommandMock.Object);

			borrowerDataCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnBorrowerDataCommand()
		{
			ICommandHandler<IBorrowerDataCommand> sut = CreateSut();

			Mock<IBorrowerDataCommand> borrowerDataCommandMock = CreateBorrowerDataCommandMock();
			await sut.ExecuteAsync(borrowerDataCommandMock.Object);

			borrowerDataCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledOnBorrowerDataCommandHandlerBase()
		{
			ICommandHandler<IBorrowerDataCommand> sut = CreateSut();

			await sut.ExecuteAsync(CreateBorrowerDataCommand());

			Assert.That(((MyBorrowerDataCommandHandler) sut).ManageAsyncWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledOnBorrowerDataCommandHandlerBaseWithBorrowerFromToDomainAsyncOnBorrowerDataCommand()
		{
			ICommandHandler<IBorrowerDataCommand> sut = CreateSut();

			IBorrower borrower = _fixture.BuildBorrowerMock().Object;
			await sut.ExecuteAsync(CreateBorrowerDataCommand(borrower));

			Assert.That(((MyBorrowerDataCommandHandler) sut).ManageAsyncCalledWithBorrower, Is.EqualTo(borrower));
		}

		private ICommandHandler<IBorrowerDataCommand> CreateSut()
		{
			return new MyBorrowerDataCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IBorrowerDataCommand CreateBorrowerDataCommand(IBorrower toDomainAsync = null)
		{
			return CreateBorrowerDataCommandMock(toDomainAsync).Object;
		}

		private Mock<IBorrowerDataCommand> CreateBorrowerDataCommandMock(IBorrower toDomainAsync = null)
		{
			Mock<IBorrowerDataCommand> borrowerDataCommandMock = new Mock<IBorrowerDataCommand>();
			borrowerDataCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(toDomainAsync ?? _fixture.BuildBorrowerMock().Object));
			return borrowerDataCommandMock;
		}

		private class MyBorrowerDataCommandHandler : BorrowerDataCommandHandlerBase<IBorrowerDataCommand>
		{
			#region Constructor

			public MyBorrowerDataCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
				: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
			{
			}

			#endregion

			#region Properties

			public bool ManageAsyncWasCalled { get; private set; }

			public IBorrower ManageAsyncCalledWithBorrower { get; private set; }

			#endregion

			#region Methods

			protected override Task ManageAsync(IBorrower borrower)
			{
				NullGuard.NotNull(borrower, nameof(borrower));

				ManageAsyncWasCalled = true;
				ManageAsyncCalledWithBorrower = borrower;

				return Task.CompletedTask;
			}

			#endregion
		}
	}
}