using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.DeleteMediaTypeCommandHandler
{
	[TestFixture]
	public class ExecuteAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<IClaimResolver> _claimResolverMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_claimResolverMock = new Mock<IClaimResolver>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<IDeleteMediaTypeCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteMediaTypeCommand()
		{
			ICommandHandler<IDeleteMediaTypeCommand> sut = CreateSut();

			Mock<IDeleteMediaTypeCommand> deleteMediaTypeCommandMock = BuildDeleteMediaTypeCommandMock();
			await sut.ExecuteAsync(deleteMediaTypeCommandMock.Object);

			deleteMediaTypeCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<bool>>(),
					It.IsNotNull<Func<int, Task<IMediaType>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertNumberWasCalledOnDeleteMediaTypeCommand()
		{
			ICommandHandler<IDeleteMediaTypeCommand> sut = CreateSut();

			Mock<IDeleteMediaTypeCommand> deleteMediaTypeCommandMock = BuildDeleteMediaTypeCommandMock();
			await sut.ExecuteAsync(deleteMediaTypeCommandMock.Object);

			deleteMediaTypeCommandMock.Verify(m => m.Number, Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertDeleteMediaTypeAsyncWasCalledOnMediaLibraryRepositoryWithNumberFromDeleteMediaTypeCommand()
		{
			ICommandHandler<IDeleteMediaTypeCommand> sut = CreateSut();

			int number = _fixture.Create<int>();
			await sut.ExecuteAsync(BuildDeleteMediaTypeCommand(number));

			_mediaLibraryRepositoryMock.Verify(m => m.DeleteMediaTypeAsync(It.Is<int>(value => value == number)), Times.Once());
		}

		private ICommandHandler<IDeleteMediaTypeCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.DeleteMediaTypeAsync(It.IsAny<int>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.DeleteMediaTypeCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private IDeleteMediaTypeCommand BuildDeleteMediaTypeCommand(int? number = null)
		{
			return BuildDeleteMediaTypeCommandMock(number).Object;
		}

		private Mock<IDeleteMediaTypeCommand> BuildDeleteMediaTypeCommandMock(int? number = null)
		{
			Mock<IDeleteMediaTypeCommand> deleteMediaTypeCommandMock = new Mock<IDeleteMediaTypeCommand>();
			deleteMediaTypeCommandMock.Setup(m => m.Number)
				.Returns(number ?? _fixture.Create<int>());
			return deleteMediaTypeCommandMock;
		}
	}
}