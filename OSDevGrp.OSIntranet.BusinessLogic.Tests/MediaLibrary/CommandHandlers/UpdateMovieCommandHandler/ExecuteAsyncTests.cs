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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.UpdateMovieCommandHandler
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
			ICommandHandler<IUpdateMovieCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnUpdateMovieCommand()
		{
			ICommandHandler<IUpdateMovieCommand> sut = CreateSut();

			Mock<IUpdateMovieCommand> updateMovieCommandMock = BuildUpdateMovieCommandMock();
			await sut.ExecuteAsync(updateMovieCommandMock.Object);

			updateMovieCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnUpdateMovieCommand()
		{
			ICommandHandler<IUpdateMovieCommand> sut = CreateSut();

			Mock<IUpdateMovieCommand> updateMovieCommandMock = BuildUpdateMovieCommandMock();
			await sut.ExecuteAsync(updateMovieCommandMock.Object);

			updateMovieCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertUpdateMediaAsyncWasCalledOnMediaLibraryRepositoryWithMovieFromToDomainAsyncOnUpdateMovieCommand()
		{
			ICommandHandler<IUpdateMovieCommand> sut = CreateSut();

			IMovie movie = _fixture.BuildMovieMock().Object;
			IUpdateMovieCommand updateMovieCommand = BuildUpdateMovieCommand(movie);
			await sut.ExecuteAsync(updateMovieCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.UpdateMediaAsync(It.Is<IMovie>(value => value != null && value == movie)), Times.Once);
		}

		private ICommandHandler<IUpdateMovieCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.UpdateMediaAsync(It.IsAny<IMovie>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.UpdateMovieCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private IUpdateMovieCommand BuildUpdateMovieCommand(IMovie movie = null)
		{
			return BuildUpdateMovieCommandMock(movie).Object;
		}

		private Mock<IUpdateMovieCommand> BuildUpdateMovieCommandMock(IMovie movie = null)
		{
			Mock<IUpdateMovieCommand> updateMovieCommandMock = new Mock<IUpdateMovieCommand>();
			updateMovieCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(movie ?? _fixture.BuildMovieMock().Object));
			return updateMovieCommandMock;
		}
	}
}