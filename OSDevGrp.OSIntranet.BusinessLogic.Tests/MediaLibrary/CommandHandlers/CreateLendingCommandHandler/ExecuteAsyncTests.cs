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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.CreateLendingCommandHandler
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
			ICommandHandler<ICreateLendingCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCreateLendingCommand()
		{
			ICommandHandler<ICreateLendingCommand> sut = CreateSut();

			Mock<ICreateLendingCommand> createLendingCommandMock = BuildCreateLendingCommandMock();
			await sut.ExecuteAsync(createLendingCommandMock.Object);

			createLendingCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnCreateLendingCommand()
		{
			ICommandHandler<ICreateLendingCommand> sut = CreateSut();

			Mock<ICreateLendingCommand> createLendingCommandMock = BuildCreateLendingCommandMock();
			await sut.ExecuteAsync(createLendingCommandMock.Object);

			createLendingCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertCreateLendingAsyncWasCalledOnMediaLibraryRepositoryWithLendingFromToDomainAsyncOnCreateLendingCommand()
		{
			ICommandHandler<ICreateLendingCommand> sut = CreateSut();

			ILending lending = _fixture.BuildLendingMock().Object;
			ICreateLendingCommand createLendingCommand = BuildCreateLendingCommand(lending);
			await sut.ExecuteAsync(createLendingCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.CreateLendingAsync(It.Is<ILending>(value => value != null && value == lending)), Times.Once);
		}

		private ICommandHandler<ICreateLendingCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.CreateLendingAsync(It.IsAny<ILending>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.CreateLendingCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private ICreateLendingCommand BuildCreateLendingCommand(ILending lending = null)
		{
			return BuildCreateLendingCommandMock(lending).Object;
		}

		private Mock<ICreateLendingCommand> BuildCreateLendingCommandMock(ILending lending = null)
		{
			Mock<ICreateLendingCommand> createLendingCommandMock = new Mock<ICreateLendingCommand>();
			createLendingCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(lending ?? _fixture.BuildLendingMock().Object));
			return createLendingCommandMock;
		}
	}
}