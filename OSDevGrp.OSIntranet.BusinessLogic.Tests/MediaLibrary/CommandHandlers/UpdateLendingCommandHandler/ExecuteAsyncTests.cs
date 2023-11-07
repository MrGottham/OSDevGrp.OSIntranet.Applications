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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.UpdateLendingCommandHandler
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
			ICommandHandler<IUpdateLendingCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnUpdateLendingCommand()
		{
			ICommandHandler<IUpdateLendingCommand> sut = CreateSut();

			Mock<IUpdateLendingCommand> updateLendingCommandMock = BuildUpdateLendingCommandMock();
			await sut.ExecuteAsync(updateLendingCommandMock.Object);

			updateLendingCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnUpdateLendingCommand()
		{
			ICommandHandler<IUpdateLendingCommand> sut = CreateSut();

			Mock<IUpdateLendingCommand> updateLendingCommandMock = BuildUpdateLendingCommandMock();
			await sut.ExecuteAsync(updateLendingCommandMock.Object);

			updateLendingCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertUpdateLendingAsyncWasCalledOnMediaLibraryRepositoryWithLendingFromToDomainAsyncOnUpdateLendingCommand()
		{
			ICommandHandler<IUpdateLendingCommand> sut = CreateSut();

			ILending lending = _fixture.BuildLendingMock().Object;
			IUpdateLendingCommand updateLendingCommand = BuildUpdateLendingCommand(lending);
			await sut.ExecuteAsync(updateLendingCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.UpdateLendingAsync(It.Is<ILending>(value => value != null && value == lending)), Times.Once);
		}

		private ICommandHandler<IUpdateLendingCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.UpdateLendingAsync(It.IsAny<ILending>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.UpdateLendingCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IUpdateLendingCommand BuildUpdateLendingCommand(ILending lending = null)
		{
			return BuildUpdateLendingCommandMock(lending).Object;
		}

		private Mock<IUpdateLendingCommand> BuildUpdateLendingCommandMock(ILending lending = null)
		{
			Mock<IUpdateLendingCommand> updateLendingCommandMock = new Mock<IUpdateLendingCommand>();
			updateLendingCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(lending ?? _fixture.BuildLendingMock().Object));
			return updateLendingCommandMock;
		}
	}
}