using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.MusicDataCommandBase
{
    [TestFixture]
	public class ToDomainAsyncTests
	{
		#region Private variables

		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void ToDomainAsync_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IMusicDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToDomainAsync(null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ToDomainAsync_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IMusicDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_AssertGetMusicGenreAsyncWasCalledOnMediaLibraryRepositoryWithMusicGenreIdentifierFromMusicDataCommand()
		{
			int musicGenreIdentifier = _fixture.Create<int>();
			IMusicDataCommand sut = CreateSut(musicGenreIdentifier: musicGenreIdentifier);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMusicGenreAsync(It.Is<int>(value => value == musicGenreIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_AssertGetMediaTypeAsyncWasCalledOnMediaLibraryRepositoryWithMediaTypeIdentifierFromMusicDataCommand()
		{
			int mediaTypeIdentifier = _fixture.Create<int>();
			IMusicDataCommand sut = CreateSut(mediaTypeIdentifier: mediaTypeIdentifier);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaTypeAsync(It.Is<int>(value => value == mediaTypeIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenArtistsIsSetOnMusicDataCommand_AssertGetMediaPersonalityAsyncWasCalledOnMediaLibraryRepositoryWithEachMediaPersonalityIdentifierInArtistsFromMusicDataCommand()
		{
			Guid[] mediaPersonalityIdentifiers = _fixture.CreateMany<Guid>(_random.Next(5, 15)).ToArray();
			IMusicDataCommand sut = CreateSut(hasArtists: true, artists: mediaPersonalityIdentifiers);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			foreach (Guid mediaPersonalityIdentifier in mediaPersonalityIdentifiers)
			{
				_mediaLibraryRepositoryMock.Verify(m => m.GetMediaPersonalityAsync(It.Is<Guid>(value => value == mediaPersonalityIdentifier)), Times.Once);
			}
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsNotNull()
		{
			IMusicDataCommand sut = CreateSut();

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMusic()
		{
			IMusicDataCommand sut = CreateSut();

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.TypeOf<Music>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMusicWhereMediaIdentifierIsEqualToMediaIdentifierFromMusicDataCommand()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			IMusicDataCommand sut = CreateSut(mediaIdentifier);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MediaIdentifier, Is.EqualTo(mediaIdentifier));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMusicWhereTitleIsNotNull()
		{
			IMusicDataCommand sut = CreateSut();

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Title, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMusicWhereTitleIsNotEmpty()
		{
			IMusicDataCommand sut = CreateSut();

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Title, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMusicWhereTitleIsEqualToTitleFromMusicDataCommand()
		{
			string title = _fixture.Create<string>();
			IMusicDataCommand sut = CreateSut(title: title);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Title, Is.EqualTo(title.ToUpper()));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSubtitleIsSetOnMusicDataCommand_ReturnsMusicWhereSubtitleIsNotNull()
		{
			IMusicDataCommand sut = CreateSut(hasSubtitle: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Subtitle, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSubtitleIsSetOnMusicDataCommand_ReturnsMusicWhereSubtitleIsNotEmpty()
		{
			IMusicDataCommand sut = CreateSut(hasSubtitle: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Subtitle, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSubtitleIsSetOnMusicDataCommand_ReturnsMusicWhereSubtitleIsEqualToSubtitleFromMusicDataCommand()
		{
			string subtitle = _fixture.Create<string>();
			IMusicDataCommand sut = CreateSut(hasSubtitle: true, subtitle: subtitle);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Subtitle, Is.EqualTo(subtitle.ToUpper()));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSubtitleIsNotSetOnMusicDataCommand_ReturnsMusicWhereSubtitleIsNull()
		{
			IMusicDataCommand sut = CreateSut(hasSubtitle: false);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Subtitle, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDescriptionIsSetOnMusicDataCommand_ReturnsMusicWhereDescriptionIsNotNull()
		{
			IMusicDataCommand sut = CreateSut(hasDescription: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Description, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDescriptionIsSetOnMusicDataCommand_ReturnsMusicWhereDescriptionIsNotEmpty()
		{
			IMusicDataCommand sut = CreateSut(hasDescription: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Description, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDescriptionIsSetOnMusicDataCommand_ReturnsMusicWhereDescriptionIsEqualToDescriptionFromMusicDataCommand()
		{
			string description = _fixture.Create<string>();
			IMusicDataCommand sut = CreateSut(hasDescription: true, description: description);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Description, Is.EqualTo(description));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDescriptionIsNotSetOnMusicDataCommand_ReturnsMusicWhereDescriptionIsNull()
		{
			IMusicDataCommand sut = CreateSut(hasDescription: false);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Description, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDetailsIsSetOnMusicDataCommand_ReturnsMusicWhereDetailsIsNotNull()
		{
			IMusicDataCommand sut = CreateSut(hasDetails: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Details, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDetailsIsSetOnMusicDataCommand_ReturnsMusicWhereDetailsIsNotEmpty()
		{
			IMusicDataCommand sut = CreateSut(hasDetails: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Details, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDetailsIsSetOnMusicDataCommand_ReturnsMusicWhereDetailsIsEqualToDetailsFromMusicDataCommand()
		{
			string details = _fixture.Create<string>();
			IMusicDataCommand sut = CreateSut(hasDetails: true, details: details);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Details, Is.EqualTo(details));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDetailsIsNotSetOnMusicDataCommand_ReturnsMusicWhereDetailsIsNull()
		{
			IMusicDataCommand sut = CreateSut(hasDetails: false);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Details, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMusicWhereMusicGenreIsNotNull()
		{
			IMusicDataCommand sut = CreateSut();

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MusicGenre, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMusicWhereMusicGenreIsEqualToMatchingMusicGenreFromMediaLibraryRepository()
		{
			IMusicGenre musicGenre = _fixture.BuildMusicGenreMock().Object;
			IMusicDataCommand sut = CreateSut(musicGenre: musicGenre);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MusicGenre, Is.EqualTo(musicGenre));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMusicWhereMediaTypeIsNotNull()
		{
			IMusicDataCommand sut = CreateSut();

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MediaType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMusicWhereMediaTypeIsEqualToMatchingMediaTypeFromMediaLibraryRepository()
		{
			IMediaType mediaType = _fixture.BuildMediaTypeMock().Object;
			IMusicDataCommand sut = CreateSut(mediaType: mediaType);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MediaType, Is.EqualTo(mediaType));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPublishedIsSetOnMusicDataCommand_ReturnsMusicWherePublishedIsNotNull()
		{
			IMusicDataCommand sut = CreateSut(hasPublished: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Published, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPublishedIsSetOnMusicDataCommand_ReturnsMusicWherePublishedIsEqualToPublishedFromMusicDataCommand()
		{
			short published = _fixture.Create<short>();
			IMusicDataCommand sut = CreateSut(hasPublished: true, published: published);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Published, Is.EqualTo(published));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPublishedIsNotSetOnMusicDataCommand_ReturnsMusicWherePublishedIsNull()
		{
			IMusicDataCommand sut = CreateSut(hasPublished: false);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Published, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenTracksIsSetOnMusicDataCommand_ReturnsMusicWhereTracksIsNotNull()
		{
			IMusicDataCommand sut = CreateSut(hasTracks: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Tracks, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenTracksIsSetOnMusicDataCommand_ReturnsMusicWhereTracksIsEqualToTracksFromMusicDataCommand()
		{
			short tracks = _fixture.Create<short>();
			IMusicDataCommand sut = CreateSut(hasTracks: true, tracks: tracks);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Tracks, Is.EqualTo(tracks));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenTracksIsNotSetOnMusicDataCommand_ReturnsMusicWhereTracksIsNull()
		{
			IMusicDataCommand sut = CreateSut(hasTracks: false);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Tracks, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenUrlIsSetOnMusicDataCommand_ReturnsMusicWhereUrlIsNotNull()
		{
			IMusicDataCommand sut = CreateSut(hasUrl: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Url, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenUrlIsSetOnMusicDataCommand_ReturnsMusicWhereUrlIsEqualToUrlFromMusicDataCommand()
		{
			string url = CreateValidUrl();
			IMusicDataCommand sut = CreateSut(hasUrl: true, url: url);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Url.AbsoluteUri, Is.EqualTo(url));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenUrlIsNotSetOnMusicDataCommand_ReturnsMusicWhereUrlIsNull()
		{
			IMusicDataCommand sut = CreateSut(hasUrl: false);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Url, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetOnMusicDataCommand_ReturnsMusicWhereImageIsNotNull()
		{
			IMusicDataCommand sut = CreateSut(hasImage: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetOnMusicDataCommand_ReturnsMusicWhereImageIsNotEmpty()
		{
			IMusicDataCommand sut = CreateSut(hasImage: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetOnMusicDataCommand_ReturnsMusicWhereImageIsEqualToImageFromMusicDataCommand()
		{
			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMusicDataCommand sut = CreateSut(hasImage: true, image: image);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.EqualTo(image));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsNotSetOnMusicDataCommand_ReturnsMusicWhereImageIsNotNull()
		{
			IMusicDataCommand sut = CreateSut(hasImage: false);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetNotOnMusicDataCommand_ReturnsMusicWhereImageIsEmpty()
		{
			IMusicDataCommand sut = CreateSut(hasImage: false);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenArtistsIsSetOnMusicDataCommand_ReturnsMusicWhereArtistsIsNotNull()
		{
			IMusicDataCommand sut = CreateSut(hasArtists: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Artists, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenArtistsIsSetOnMusicDataCommand_ReturnsMusicWhereArtistsIsNotEmpty()
		{
			IMusicDataCommand sut = CreateSut(hasArtists: true);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Artists, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenArtistsIsSetOnMusicDataCommand_ReturnsMusicWhereArtistsIsNonEmptyCollectionOfMatchingMediaPersonalitiesInArtistsFromMusicDataCommand()
		{
			Guid[] mediaPersonalityIdentifiers = _fixture.CreateMany<Guid>(_random.Next(5, 15)).ToArray();
			IMusicDataCommand sut = CreateSut(hasArtists: true, artists: mediaPersonalityIdentifiers);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(mediaPersonalityIdentifiers.All(mediaPersonalityIdentifier => result.Artists.SingleOrDefault(mediaPersonality => mediaPersonality.MediaPersonalityIdentifier == mediaPersonalityIdentifier) != null), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenArtistsIsNotSetOnMusicDataCommand_ReturnsMusicWhereArtistsIsNotNull()
		{
			IMusicDataCommand sut = CreateSut(hasArtists: false);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Artists, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenArtistsIsNotSetOnMusicDataCommand_ReturnsMusicWhereArtistsIsEmpty()
		{
			IMusicDataCommand sut = CreateSut(hasArtists: false);

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Artists, Is.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMusicWhereLendingsIsNotNull()
		{
			IMusicDataCommand sut = CreateSut();

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Lendings, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMusicWhereLendingsIsEmpty()
		{
			IMusicDataCommand sut = CreateSut();

			IMusic result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Lendings, Is.Empty);
		}

		private IMusicDataCommand CreateSut(Guid? mediaIdentifier = null, string title = null, bool hasSubtitle = true, string subtitle = null, bool hasDescription = true, string description = null, bool hasDetails = true, string details = null, int? musicGenreIdentifier = null, IMusicGenre musicGenre = null, int? mediaTypeIdentifier = null, IMediaType mediaType = null, bool hasPublished = true, short? published = null, bool hasTracks = true, short? tracks = null, bool hasUrl = true, string url = null, bool hasImage = true, byte[] image = null, bool hasArtists = true, IEnumerable<Guid> artists = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaPersonalityAsync(It.IsAny<Guid>()))
				.Returns<Guid>(mediaPersonalityIdentifier => Task.FromResult(_fixture.BuildMediaPersonalityMock(mediaPersonalityIdentifier).Object));
			_mediaLibraryRepositoryMock.Setup(m => m.GetMusicGenreAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(musicGenre ?? _fixture.BuildMusicGenreMock().Object));
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaTypeAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(mediaType ?? _fixture.BuildMediaTypeMock().Object));

			return new MyMusicDataCommand(mediaIdentifier ?? Guid.NewGuid(), title ?? _fixture.Create<string>(), hasSubtitle ? subtitle ?? _fixture.Create<string>() : null, hasDescription ? description ?? _fixture.Create<string>() : null, hasDetails ? details ?? _fixture.Create<string>() : null, musicGenreIdentifier ?? _fixture.Create<int>(), mediaTypeIdentifier ?? _fixture.Create<int>(), hasPublished ? published ?? _fixture.Create<short>() : null, hasTracks ? tracks ?? _fixture.Create<short>() : null, hasUrl ? url ?? CreateValidUrl() : null, hasImage ? image ?? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null, hasArtists ? artists ?? _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray() : null);
		}

		private string CreateValidUrl()
		{
			return _fixture.CreateEndpointString(path: $"api/music/{_fixture.Create<string>()}");
		}

		private class MyMusicDataCommand : BusinessLogic.MediaLibrary.Commands.MusicDataCommandBase
		{
			#region Constructor

			public MyMusicDataCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int musicGenreIdentifier, int mediaTypeIdentifier, short? published, short? tracks, string url, byte[] image, IEnumerable<Guid> artists) 
				: base(mediaIdentifier, title, subtitle, description, details, musicGenreIdentifier, mediaTypeIdentifier, published, tracks, url, image, artists)
			{
			}

			#endregion
		}
	}
}