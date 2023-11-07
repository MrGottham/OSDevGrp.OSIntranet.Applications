using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.MediaResolver
{
	[TestFixture]
	public class ResolveAsyncTests
	{
		#region Private variables

		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveAsync_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(Guid.NewGuid(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsMovie_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForMovie()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock(isMovie: true, isMusic: false, isBook: false);
			await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(mediaIdentifier, mediaLibraryRepositoryMock.Object);

			mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMovie>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsMovie_AssertGetMediaAsyncWasNotCalledOnMediaLibraryRepositoryForMusic()
		{
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock(isMovie: true, isMusic: false, isBook: false);
			await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(Guid.NewGuid(), mediaLibraryRepositoryMock.Object);

			mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMusic>(It.IsAny<Guid>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsMovie_AssertGetMediaAsyncWasNotCalledOnMediaLibraryRepositoryForBook()
		{
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock(isMovie: true, isMusic: false, isBook: false);
			await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(Guid.NewGuid(), mediaLibraryRepositoryMock.Object);

			mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IBook>(It.IsAny<Guid>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsMovie_ReturnsNotNull()
		{
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(isMovie: true, isMusic: false, isBook: false);
			IMedia result = await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(Guid.NewGuid(), mediaLibraryRepository);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsMovie_ReturnsMovieFromMediaLibraryRepository()
		{
			IMovie movie = _fixture.BuildMovieMock().Object;
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(isMovie: true, movie: movie, isMusic: false, isBook: false);
			IMedia result = await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(Guid.NewGuid(), mediaLibraryRepository);

			Assert.That(result, Is.EqualTo(movie));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsMusic_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForMovie()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock(isMovie: false, isMusic: true, isBook: false);
			await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(mediaIdentifier, mediaLibraryRepositoryMock.Object);

			mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMovie>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsMusic_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForMusic()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock(isMovie: false, isMusic: true, isBook: false);
			await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(mediaIdentifier, mediaLibraryRepositoryMock.Object);

			mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMusic>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsMusic_AssertGetMediaAsyncWasNotCalledOnMediaLibraryRepositoryForBook()
		{
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock(isMovie: false, isMusic: true, isBook: false);
			await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(Guid.NewGuid(), mediaLibraryRepositoryMock.Object);

			mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IBook>(It.IsAny<Guid>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsMusic_ReturnsNotNull()
		{
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(isMovie: false, isMusic: true, isBook: false);
			IMedia result = await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(Guid.NewGuid(), mediaLibraryRepository);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsMusic_ReturnsMusicFromMediaLibraryRepository()
		{
			IMusic music = _fixture.BuildMusicMock().Object;
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(isMovie: false, isMusic: true, music: music, isBook: false);
			IMedia result = await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(Guid.NewGuid(), mediaLibraryRepository);

			Assert.That(result, Is.EqualTo(music));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsBook_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForMovie()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock(isMovie: false, isMusic: false, isBook: true);
			await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(mediaIdentifier, mediaLibraryRepositoryMock.Object);

			mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMovie>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsBook_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForMusic()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock(isMovie: false, isMusic: false, isBook: true);
			await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(mediaIdentifier, mediaLibraryRepositoryMock.Object);

			mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMusic>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsBook_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForBook()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock(isMovie: false, isMusic: false, isBook: true);
			await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(mediaIdentifier, mediaLibraryRepositoryMock.Object);

			mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IBook>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsBook_ReturnsNotNull()
		{
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(isMovie: false, isMusic: false, isBook: true);
			IMedia result = await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(Guid.NewGuid(), mediaLibraryRepository);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsBook_ReturnsMusicFromMediaLibraryRepository()
		{
			IBook book = _fixture.BuildBookMock().Object;
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(isMovie: false, isMusic: false, isBook: true, book: book);
			IMedia result = await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(Guid.NewGuid(), mediaLibraryRepository);

			Assert.That(result, Is.EqualTo(book));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsNeitherMovieMusicNorBook_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForMovie()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock(isMovie: false, isMusic: false, isBook: false);
			await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(mediaIdentifier, mediaLibraryRepositoryMock.Object);

			mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMovie>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsNeitherMovieMusicNorBook_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForMusic()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock(isMovie: false, isMusic: false, isBook: false);
			await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(mediaIdentifier, mediaLibraryRepositoryMock.Object);

			mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMusic>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsNeitherMovieMusicNorBook_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForBook()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock(isMovie: false, isMusic: false, isBook: false);
			await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(mediaIdentifier, mediaLibraryRepositoryMock.Object);

			mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IBook>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenMediaIdentifierIsNeitherMovieMusicNorBook_ReturnsNull()
		{
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(isMovie: false, isMusic: false, isBook: false);
			IMedia result = await BusinessLogic.MediaLibrary.Logic.MediaResolver.ResolveAsync(Guid.NewGuid(), mediaLibraryRepository);

			Assert.That(result, Is.Null);
		}

		private IMediaLibraryRepository CreateMediaLibraryRepository(bool? isMovie = null, IMovie movie = null, bool? isMusic = null, IMusic music = null, bool? isBook = null, IBook book = null)
		{
			return CreateMediaLibraryRepositoryMock(isMovie, movie, isMusic, music, isBook, book).Object;
		}

		private Mock<IMediaLibraryRepository> CreateMediaLibraryRepositoryMock(bool? isMovie = null, IMovie movie = null, bool? isMusic = null, IMusic music = null, bool? isBook = null, IBook book = null)
		{
			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();

			if ((isMovie.HasValue && isMovie.Value) || movie != null)
			{
				mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IMovie>(It.IsAny<Guid>()))
					.Returns(Task.FromResult(movie ?? _fixture.BuildMovieMock().Object));
				mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IMusic>(It.IsAny<Guid>()))
					.Returns(Task.FromResult<IMusic>(null));
				mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IBook>(It.IsAny<Guid>()))
					.Returns(Task.FromResult<IBook>(null));
				return mediaLibraryRepositoryMock;
			}

			if ((isMusic.HasValue && isMusic.Value) || music != null)
			{
				mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IMovie>(It.IsAny<Guid>()))
					.Returns(Task.FromResult<IMovie>(null));
				mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IMusic>(It.IsAny<Guid>()))
					.Returns(Task.FromResult(music ?? _fixture.BuildMusicMock().Object));
				mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IBook>(It.IsAny<Guid>()))
					.Returns(Task.FromResult<IBook>(null));
				return mediaLibraryRepositoryMock;
			}

			if ((isBook.HasValue && isBook.Value) || book != null)
			{
				mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IMovie>(It.IsAny<Guid>()))
					.Returns(Task.FromResult<IMovie>(null));
				mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IMusic>(It.IsAny<Guid>()))
					.Returns(Task.FromResult<IMusic>(null));
				mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IBook>(It.IsAny<Guid>()))
					.Returns(Task.FromResult(book ?? _fixture.BuildBookMock().Object));
				return mediaLibraryRepositoryMock;
			}

			mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IMovie>(It.IsAny<Guid>()))
				.Returns(Task.FromResult<IMovie>(null));
			mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IMusic>(It.IsAny<Guid>()))
				.Returns(Task.FromResult<IMusic>(null));
			mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IBook>(It.IsAny<Guid>()))
				.Returns(Task.FromResult<IBook>(null));

			return mediaLibraryRepositoryMock;
		}
	}
}