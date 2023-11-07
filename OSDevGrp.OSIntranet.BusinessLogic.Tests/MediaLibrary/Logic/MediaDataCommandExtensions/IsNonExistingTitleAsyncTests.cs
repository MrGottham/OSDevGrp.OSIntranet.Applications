using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.MediaDataCommandExtensions
{
	[TestFixture]
	public class IsNonExistingTitleAsyncTests
	{
		#region Private variables

		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void IsNonExistingTitleAsync_WhenMediaDataCommandIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(null, CreateMediaLibraryRepository()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaDataCommand"));
		}

		[Test]
		[Category("UnitTest")]
		public void IsNonExistingTitleAsync_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(CreateMediaDataCommand<IMedia>(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingTitleAsync_WhenCalled_AssertTitleWasCalledOnMediaDataCommand()
		{
			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock<IMedia>();

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommandMock.Object, CreateMediaLibraryRepository());

			mediaDataCommandMock.Verify(m => m.Title, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingTitleAsync_WhenCalled_AssertSubtitleWasCalledOnMediaDataCommand()
		{
			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock<IMedia>();

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommandMock.Object, CreateMediaLibraryRepository());

			mediaDataCommandMock.Verify(m => m.Subtitle, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingTitleAsync_WhenCalled_AssertMediaTypeIdentifierWasCalledOnMediaDataCommand()
		{
			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock<IMedia>();

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommandMock.Object, CreateMediaLibraryRepository());

			mediaDataCommandMock.Verify(m => m.MediaTypeIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsNonExistingTitleAsync_WhenCalled_AssertMediaExistsAsyncWasCalledOnMediaLibraryRepositoryForMovie(bool hasSubtitle)
		{
			string title = _fixture.Create<string>().ToUpper();
			string subtitle = _fixture.Create<string>().ToUpper();
			int mediaTypeIdentifier = _fixture.Create<int>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>(title, hasSubtitle, subtitle, mediaTypeIdentifier);
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository();

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IMovie>(
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, title) == 0),
					It.Is<string>(value => hasSubtitle ? string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, subtitle) == 0 : value == null),
					It.Is<int>(value => value == mediaTypeIdentifier)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingTitleAsync_WhenTitleExistsForMovie_AssertMediaExistsAsyncWasNotCalledOnMediaLibraryRepositoryForMusic()
		{
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>();
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(titleExistsForMovie: true);

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IMusic>(
					It.IsAny<string>(),
					It.IsAny<string>(),
					It.IsAny<int>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingTitleAsync_WhenTitleExistsForMovie_AssertMediaExistsAsyncWasNotCalledOnMediaLibraryRepositoryForBook()
		{
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>();
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(titleExistsForMovie: true);

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IBook>(
					It.IsAny<string>(),
					It.IsAny<string>(),
					It.IsAny<int>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsNonExistingTitleAsync_WhenTitleDoesNotExistForMovie_AssertMediaExistsAsyncWasCalledOnMediaLibraryRepositoryForMovie(bool hasSubtitle)
		{
			string title = _fixture.Create<string>().ToUpper();
			string subtitle = _fixture.Create<string>().ToUpper();
			int mediaTypeIdentifier = _fixture.Create<int>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>(title, hasSubtitle, subtitle, mediaTypeIdentifier);
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(titleExistsForMovie: false);

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IMovie>(
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, title) == 0),
					It.Is<string>(value => hasSubtitle ? string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, subtitle) == 0 : value == null),
					It.Is<int>(value => value == mediaTypeIdentifier)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsNonExistingTitleAsync_WhenTitleDoesNotExistForMovie_AssertMediaExistsAsyncWasCalledOnMediaLibraryRepositoryForMusic(bool hasSubtitle)
		{
			string title = _fixture.Create<string>().ToUpper();
			string subtitle = _fixture.Create<string>().ToUpper();
			int mediaTypeIdentifier = _fixture.Create<int>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>(title, hasSubtitle, subtitle, mediaTypeIdentifier);
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(titleExistsForMovie: false);

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IMusic>(
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, title) == 0),
					It.Is<string>(value => hasSubtitle ? string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, subtitle) == 0 : value == null),
					It.Is<int>(value => value == mediaTypeIdentifier)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingTitleAsync_WhenTitleDoesNotExistForMovieButForMusic_AssertMediaExistsAsyncWasNotCalledOnMediaLibraryRepositoryForBook()
		{
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>();
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(titleExistsForMovie: false, titleExistsForMusic: true);

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IBook>(
					It.IsAny<string>(),
					It.IsAny<string>(),
					It.IsAny<int>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsNonExistingTitleAsync_WhenTitleDoesNotExistForMovieAndNotForMusic_AssertMediaExistsAsyncWasCalledOnMediaLibraryRepositoryForMovie(bool hasSubtitle)
		{
			string title = _fixture.Create<string>().ToUpper();
			string subtitle = _fixture.Create<string>().ToUpper();
			int mediaTypeIdentifier = _fixture.Create<int>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>(title, hasSubtitle, subtitle, mediaTypeIdentifier);
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(titleExistsForMovie: false, titleExistsForMusic: false);

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IMovie>(
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, title) == 0),
					It.Is<string>(value => hasSubtitle ? string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, subtitle) == 0 : value == null),
					It.Is<int>(value => value == mediaTypeIdentifier)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsNonExistingTitleAsync_WhenTitleDoesNotExistForMovieAndNotForMusic_AssertMediaExistsAsyncWasCalledOnMediaLibraryRepositoryForMusic(bool hasSubtitle)
		{
			string title = _fixture.Create<string>().ToUpper();
			string subtitle = _fixture.Create<string>().ToUpper();
			int mediaTypeIdentifier = _fixture.Create<int>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>(title, hasSubtitle, subtitle, mediaTypeIdentifier);
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(titleExistsForMovie: false, titleExistsForMusic: false);

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IMusic>(
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, title) == 0),
					It.Is<string>(value => hasSubtitle ? string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, subtitle) == 0 : value == null),
					It.Is<int>(value => value == mediaTypeIdentifier)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsNonExistingTitleAsync_WhenTitleDoesNotExistForMovieAndNotForMusic_AssertMediaExistsAsyncWasCalledOnMediaLibraryRepositoryForBook(bool hasSubtitle)
		{
			string title = _fixture.Create<string>().ToUpper();
			string subtitle = _fixture.Create<string>().ToUpper();
			int mediaTypeIdentifier = _fixture.Create<int>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>(title, hasSubtitle, subtitle, mediaTypeIdentifier);
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(titleExistsForMovie: false, titleExistsForMusic: false);

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IBook>(
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, title) == 0),
					It.Is<string>(value => hasSubtitle ? string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, subtitle) == 0 : value == null),
					It.Is<int>(value => value == mediaTypeIdentifier)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingTitleAsync_WhenTitleExistsForMovie_ReturnsFalse()
		{
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>();
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(titleExistsForMovie: true, titleExistsForMusic: false, titleExistsForBook: false);

			bool result = await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingTitleAsync_WhenTitleExistsForMusic_ReturnsFalse()
		{
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>();
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(titleExistsForMovie: false, titleExistsForMusic: true, titleExistsForBook: false);

			bool result = await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingTitleAsync_WhenTitleExistsForBook_ReturnsFalse()
		{
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>();
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(titleExistsForMovie: false, titleExistsForMusic: false, titleExistsForBook: true);

			bool result = await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsNonExistingTitleAsync_WhenTitleDoesNotExistForMovieAndNotForMusicAndNotForBook_ReturnsTrue()
		{
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>();
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository(titleExistsForMovie: false, titleExistsForMusic: false, titleExistsForBook: false);

			bool result = await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsNonExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, mediaLibraryRepository);

			Assert.That(result, Is.True);
		}

		private IMediaDataCommand<TMedia> CreateMediaDataCommand<TMedia>(string title = null, bool hasSubtitle = false, string subtitle = null, int? mediaTypeIdentifier = null) where TMedia : IMedia
		{
			return CreateMediaDataCommandMock<TMedia>(title, hasSubtitle, subtitle, mediaTypeIdentifier).Object;
		}

		private Mock<IMediaDataCommand<TMedia>> CreateMediaDataCommandMock<TMedia>(string title = null, bool hasSubtitle = false, string subtitle = null, int? mediaTypeIdentifier = null) where TMedia : IMedia
		{
			Mock<IMediaDataCommand<TMedia>> mediaDataCommandMock = new Mock<IMediaDataCommand<TMedia>>();
			mediaDataCommandMock.Setup(m => m.Title)
				.Returns(title ?? _fixture.Create<string>().ToUpper());
			mediaDataCommandMock.Setup(m => m.Subtitle)
				.Returns(hasSubtitle ? subtitle ?? _fixture.Create<string>().ToUpper() : null);
			mediaDataCommandMock.Setup(m => m.MediaTypeIdentifier)
				.Returns(mediaTypeIdentifier ?? _fixture.Create<int>());
			return mediaDataCommandMock;
		}

		private IMediaLibraryRepository CreateMediaLibraryRepository(bool? titleExistsForMovie = null, bool? titleExistsForMusic = null, bool? titleExistsForBook = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.MediaExistsAsync<IMovie>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
				.Returns(Task.FromResult(titleExistsForMovie ?? _fixture.Create<bool>()));
			_mediaLibraryRepositoryMock.Setup(m => m.MediaExistsAsync<IMusic>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
				.Returns(Task.FromResult(titleExistsForMusic ?? _fixture.Create<bool>()));
			_mediaLibraryRepositoryMock.Setup(m => m.MediaExistsAsync<IBook>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
				.Returns(Task.FromResult(titleExistsForBook ?? _fixture.Create<bool>()));

			return _mediaLibraryRepositoryMock.Object;
		}
	}
}