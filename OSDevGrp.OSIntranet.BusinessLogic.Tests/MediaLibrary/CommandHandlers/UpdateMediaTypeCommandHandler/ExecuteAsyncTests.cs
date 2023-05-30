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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.UpdateMediaTypeCommandHandler
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
			ICommandHandler<IUpdateMediaTypeCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnUpdateMediaTypeCommand()
		{
			ICommandHandler<IUpdateMediaTypeCommand> sut = CreateSut();

			Mock<IUpdateMediaTypeCommand> updateMediaTypeCommandMock = BuildUpdateMediaTypeCommandMock();
			await sut.ExecuteAsync(updateMediaTypeCommandMock.Object);

			updateMediaTypeCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<bool>>(),
					It.IsNotNull<Func<int, Task<IMediaType>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnUpdateMediaTypeCommand()
		{
			ICommandHandler<IUpdateMediaTypeCommand> sut = CreateSut();

			Mock<IUpdateMediaTypeCommand> updateMediaTypeCommandMock = BuildUpdateMediaTypeCommandMock();
			await sut.ExecuteAsync(updateMediaTypeCommandMock.Object);

			updateMediaTypeCommandMock.Verify(m => m.ToDomain(), Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertUpdateMediaTypeAsyncWasCalledOnMediaLibraryRepositoryWithMediaTypeFromUpdateMediaTypeCommand()
		{
			ICommandHandler<IUpdateMediaTypeCommand> sut = CreateSut();

			IMediaType mediaType = _fixture.BuildMediaTypeMock().Object;
			await sut.ExecuteAsync(BuildUpdateMediaTypeCommand(mediaType));

			_mediaLibraryRepositoryMock.Verify(m => m.UpdateMediaTypeAsync(It.Is<IMediaType>(value => value != null && value == mediaType)), Times.Once());
		}

		private ICommandHandler<IUpdateMediaTypeCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.UpdateMediaTypeAsync(It.IsAny<IMediaType>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.UpdateMediaTypeCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private IUpdateMediaTypeCommand BuildUpdateMediaTypeCommand(IMediaType mediaType = null)
		{
			return BuildUpdateMediaTypeCommandMock(mediaType).Object;
		}

		private Mock<IUpdateMediaTypeCommand> BuildUpdateMediaTypeCommandMock(IMediaType mediaType = null)
		{
			Mock<IUpdateMediaTypeCommand> updateMediaTypeCommandMock = new Mock<IUpdateMediaTypeCommand>();
			updateMediaTypeCommandMock.Setup(m => m.ToDomain())
				.Returns(mediaType ?? _fixture.BuildMediaTypeMock().Object);
			return updateMediaTypeCommandMock;
		}
	}
}