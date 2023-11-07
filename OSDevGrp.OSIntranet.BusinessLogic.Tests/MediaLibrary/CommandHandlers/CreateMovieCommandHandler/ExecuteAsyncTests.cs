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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.CreateMovieCommandHandler
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
			ICommandHandler<ICreateMovieCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCreateMovieCommand()
		{
			ICommandHandler<ICreateMovieCommand> sut = CreateSut();

			Mock<ICreateMovieCommand> createMovieCommandMock = BuildCreateMovieCommandMock();
			await sut.ExecuteAsync(createMovieCommandMock.Object);

			createMovieCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainAsyncWasCalledOnCreateMovieCommand()
		{
			ICommandHandler<ICreateMovieCommand> sut = CreateSut();

			Mock<ICreateMovieCommand> createMovieCommandMock = BuildCreateMovieCommandMock();
			await sut.ExecuteAsync(createMovieCommandMock.Object);

			createMovieCommandMock.Verify(m => m.ToDomainAsync(
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertCreateMediaAsyncWasCalledOnMediaLibraryRepositoryWithMovieFromToDomainAsyncOnCreateMovieCommand()
		{
			ICommandHandler<ICreateMovieCommand> sut = CreateSut();

			IMovie movie = _fixture.BuildMovieMock().Object;
			ICreateMovieCommand createMovieCommand = BuildCreateMovieCommand(movie);
			await sut.ExecuteAsync(createMovieCommand);

			_mediaLibraryRepositoryMock.Verify(m => m.CreateMediaAsync(It.Is<IMovie>(value => value != null && value == movie)), Times.Once);
		}

		private ICommandHandler<ICreateMovieCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.CreateMediaAsync(It.IsAny<IMovie>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.CreateMovieCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private ICreateMovieCommand BuildCreateMovieCommand(IMovie movie = null)
		{
			return BuildCreateMovieCommandMock(movie).Object;
		}

		private Mock<ICreateMovieCommand> BuildCreateMovieCommandMock(IMovie movie = null)
		{
			Mock<ICreateMovieCommand> createMovieCommandMock = new Mock<ICreateMovieCommand>();
			createMovieCommandMock.Setup(m => m.ToDomainAsync(It.IsAny<IMediaLibraryRepository>(), It.IsAny<ICommonRepository>()))
				.Returns(Task.FromResult(movie ?? _fixture.BuildMovieMock().Object));
			return createMovieCommandMock;
		}
	}
}