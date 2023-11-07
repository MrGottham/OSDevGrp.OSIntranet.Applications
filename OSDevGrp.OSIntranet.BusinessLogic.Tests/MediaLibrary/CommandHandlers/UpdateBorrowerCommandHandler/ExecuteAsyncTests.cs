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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.UpdateBorrowerCommandHandler
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
			ICommandHandler<IUpdateBorrowerCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnUpdateBorrowerCommand()
		{
			ICommandHandler<IUpdateBorrowerCommand> sut = CreateSut();

			Mock<IUpdateBorrowerCommand> updateBorrowerCommandMock = BuildUpdateBorrowerCommandMock();
			await sut.ExecuteAsync(updateBorrowerCommandMock.Object);

			updateBorrowerCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnUpdateBorrowerCommand()
		{
			ICommandHandler<IUpdateBorrowerCommand> sut = CreateSut();

			Mock<IUpdateBorrowerCommand> updateBorrowerCommandMock = BuildUpdateBorrowerCommandMock();
			await sut.ExecuteAsync(updateBorrowerCommandMock.Object);

			updateBorrowerCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertUpdateBorrowerAsyncWasCalledOnMediaLibraryRepositoryWithBorrowerFromToDomainAsyncOnUpdateBorrowerCommand()
		{
			ICommandHandler<IUpdateBorrowerCommand> sut = CreateSut();

			IBorrower borrower = _fixture.BuildBorrowerMock().Object;
			IUpdateBorrowerCommand updateBorrowerCommand = BuildUpdateBorrowerCommand(borrower);
			await sut.ExecuteAsync(updateBorrowerCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.UpdateBorrowerAsync(It.Is<IBorrower>(value => value != null && value == borrower)), Times.Once);
		}

		private ICommandHandler<IUpdateBorrowerCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.UpdateBorrowerAsync(It.IsAny<IBorrower>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.UpdateBorrowerCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IUpdateBorrowerCommand BuildUpdateBorrowerCommand(IBorrower borrower = null)
		{
			return BuildUpdateBorrowerCommandMock(borrower).Object;
		}

		private Mock<IUpdateBorrowerCommand> BuildUpdateBorrowerCommandMock(IBorrower borrower = null)
		{
			Mock<IUpdateBorrowerCommand> updateBorrowerCommandMock = new Mock<IUpdateBorrowerCommand>();
			updateBorrowerCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(borrower ?? _fixture.BuildBorrowerMock().Object));
			return updateBorrowerCommandMock;
		}
	}
}