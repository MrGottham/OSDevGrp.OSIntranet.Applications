using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.MovieDataCommandBase
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
			IMovieDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToDomainAsync(null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ToDomainAsync_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IMovieDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_AssertGetMovieGenreAsyncWasCalledOnMediaLibraryRepositoryWithMovieGenreIdentifierFromMovieDataCommand()
		{
			int movieGenreIdentifier = _fixture.Create<int>();
			IMovieDataCommand sut = CreateSut(movieGenreIdentifier: movieGenreIdentifier);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMovieGenreAsync(It.Is<int>(value => value == movieGenreIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSpokenLanguageIdentifierIsSetOnMovieDataCommand_AssertGetLanguageAsyncWasCalledOnCommonRepositoryWithSpokenLanguageIdentifierFromMovieDataCommand()
		{
			int spokenLanguageIdentifier = _fixture.Create<int>();
			IMovieDataCommand sut = CreateSut(hasSpokenLanguageIdentifier: true, spokenLanguageIdentifier: spokenLanguageIdentifier);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_commonRepositoryMock.Verify(m => m.GetLanguageAsync(It.Is<int>(value => value == spokenLanguageIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSpokenLanguageIdentifierIsNotSetOnMovieDataCommand_AssertGetLanguageAsyncWasNotCalledOnCommonRepository()
		{
			IMovieDataCommand sut = CreateSut(hasSpokenLanguageIdentifier: false);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_commonRepositoryMock.Verify(m => m.GetLanguageAsync(It.IsAny<int>()), Times.Never());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_AssertGetMediaTypeAsyncWasCalledOnMediaLibraryRepositoryWithMediaTypeIdentifierFromMovieDataCommand()
		{
			int mediaTypeIdentifier = _fixture.Create<int>();
			IMovieDataCommand sut = CreateSut(mediaTypeIdentifier: mediaTypeIdentifier);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaTypeAsync(It.Is<int>(value => value == mediaTypeIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDirectorsIsSetOnMovieDataCommand_AssertGetMediaPersonalityAsyncWasCalledOnMediaLibraryRepositoryWithEachMediaPersonalityIdentifierInDirectorsFromMovieDataCommand()
		{
			Guid[] mediaPersonalityIdentifiers = _fixture.CreateMany<Guid>(_random.Next(5, 15)).ToArray();
			IMovieDataCommand sut = CreateSut(hasDirectors: true, directors: mediaPersonalityIdentifiers);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			foreach (Guid mediaPersonalityIdentifier in mediaPersonalityIdentifiers)
			{
				_mediaLibraryRepositoryMock.Verify(m => m.GetMediaPersonalityAsync(It.Is<Guid>(value => value == mediaPersonalityIdentifier)), Times.Once);
			}
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenActorsIsSetOnMovieDataCommand_AssertGetMediaPersonalityAsyncWasCalledOnMediaLibraryRepositoryWithEachMediaPersonalityIdentifierInActorsFromMovieDataCommand()
		{
			Guid[] mediaPersonalityIdentifiers = _fixture.CreateMany<Guid>(_random.Next(5, 15)).ToArray();
			IMovieDataCommand sut = CreateSut(hasActors: true, actors: mediaPersonalityIdentifiers);

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
			IMovieDataCommand sut = CreateSut();

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMovie()
		{
			IMovieDataCommand sut = CreateSut();

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.TypeOf<Movie>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMovieWhereMediaIdentifierIsEqualToMediaIdentifierFromMovieDataCommand()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			IMovieDataCommand sut = CreateSut(mediaIdentifier);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MediaIdentifier, Is.EqualTo(mediaIdentifier));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMovieWhereTitleIsNotNull()
		{
			IMovieDataCommand sut = CreateSut();

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Title, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMovieWhereTitleIsNotEmpty()
		{
			IMovieDataCommand sut = CreateSut();

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Title, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMovieWhereTitleIsEqualToTitleFromMovieDataCommand()
		{
			string title = _fixture.Create<string>();
			IMovieDataCommand sut = CreateSut(title: title);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Title, Is.EqualTo(title.ToUpper()));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSubtitleIsSetOnMovieDataCommand_ReturnsMovieWhereSubtitleIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasSubtitle: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Subtitle, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSubtitleIsSetOnMovieDataCommand_ReturnsMovieWhereSubtitleIsNotEmpty()
		{
			IMovieDataCommand sut = CreateSut(hasSubtitle: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Subtitle, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSubtitleIsSetOnMovieDataCommand_ReturnsMovieWhereSubtitleIsEqualToSubtitleFromMovieDataCommand()
		{
			string subtitle = _fixture.Create<string>();
			IMovieDataCommand sut = CreateSut(hasSubtitle: true, subtitle: subtitle);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Subtitle, Is.EqualTo(subtitle.ToUpper()));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSubtitleIsNotSetOnMovieDataCommand_ReturnsMovieWhereSubtitleIsNull()
		{
			IMovieDataCommand sut = CreateSut(hasSubtitle: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Subtitle, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDescriptionIsSetOnMovieDataCommand_ReturnsMovieWhereDescriptionIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasDescription: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Description, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDescriptionIsSetOnMovieDataCommand_ReturnsMovieWhereDescriptionIsNotEmpty()
		{
			IMovieDataCommand sut = CreateSut(hasDescription: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Description, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDescriptionIsSetOnMovieDataCommand_ReturnsMovieWhereDescriptionIsEqualToDescriptionFromMovieDataCommand()
		{
			string description = _fixture.Create<string>();
			IMovieDataCommand sut = CreateSut(hasDescription: true, description: description);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Description, Is.EqualTo(description));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDescriptionIsNotSetOnMovieDataCommand_ReturnsMovieWhereDescriptionIsNull()
		{
			IMovieDataCommand sut = CreateSut(hasDescription: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Description, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDetailsIsSetOnMovieDataCommand_ReturnsMovieWhereDetailsIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasDetails: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Details, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDetailsIsSetOnMovieDataCommand_ReturnsMovieWhereDetailsIsNotEmpty()
		{
			IMovieDataCommand sut = CreateSut(hasDetails: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Details, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDetailsIsSetOnMovieDataCommand_ReturnsMovieWhereDetailsIsEqualToDetailsFromMovieDataCommand()
		{
			string details = _fixture.Create<string>();
			IMovieDataCommand sut = CreateSut(hasDetails: true, details: details);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Details, Is.EqualTo(details));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDetailsIsNotSetOnMovieDataCommand_ReturnsMovieWhereDetailsIsNull()
		{
			IMovieDataCommand sut = CreateSut(hasDetails: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Details, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMovieWhereMovieGenreIsNotNull()
		{
			IMovieDataCommand sut = CreateSut();

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MovieGenre, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMovieWhereMovieGenreIsEqualToMatchingMovieGenreFromMediaLibraryRepository()
		{
			IMovieGenre movieGenre = _fixture.BuildMovieGenreMock().Object;
			IMovieDataCommand sut = CreateSut(movieGenre: movieGenre);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MovieGenre, Is.EqualTo(movieGenre));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSpokenLanguageIdentifierIsSetOnMovieDataCommand_ReturnsMovieWhereSpokenLanguageIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasSpokenLanguageIdentifier: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.SpokenLanguage, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSpokenLanguageIdentifierIsSetOnMovieDataCommand_ReturnsMovieWhereSpokenLanguageIsEqualToMatchingLanguageFromCommonRepository()
		{
			ILanguage spokenLanguage = _fixture.BuildLanguageMock().Object;
			IMovieDataCommand sut = CreateSut(hasSpokenLanguageIdentifier: true, spokenLanguage: spokenLanguage);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.SpokenLanguage, Is.EqualTo(spokenLanguage));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSpokenLanguageIdentifierIsNotSetOnMovieDataCommand_ReturnsMovieWhereSpokenLanguageIsNull()
		{
			IMovieDataCommand sut = CreateSut(hasSpokenLanguageIdentifier: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.SpokenLanguage, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMovieWhereMediaTypeIsNotNull()
		{
			IMovieDataCommand sut = CreateSut();

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MediaType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMovieWhereMediaTypeIsEqualToMatchingMediaTypeFromMediaLibraryRepository()
		{
			IMediaType mediaType = _fixture.BuildMediaTypeMock().Object;
			IMovieDataCommand sut = CreateSut(mediaType: mediaType);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MediaType, Is.EqualTo(mediaType));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPublishedIsSetOnMovieDataCommand_ReturnsMovieWherePublishedIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasPublished: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Published, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPublishedIsSetOnMovieDataCommand_ReturnsMovieWherePublishedIsEqualToPublishedFromMovieDataCommand()
		{
			short published = _fixture.Create<short>();
			IMovieDataCommand sut = CreateSut(hasPublished: true, published: published);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Published, Is.EqualTo(published));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPublishedIsNotSetOnMovieDataCommand_ReturnsMovieWherePublishedIsNull()
		{
			IMovieDataCommand sut = CreateSut(hasPublished: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Published, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenLengthIsSetOnMovieDataCommand_ReturnsMovieWhereLengthIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasLength: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Length, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenLengthIsSetOnMovieDataCommand_ReturnsMovieWhereLengthIsEqualToLengthFromMovieDataCommand()
		{
			short length = _fixture.Create<short>();
			IMovieDataCommand sut = CreateSut(hasLength: true, length: length);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Length, Is.EqualTo(length));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenLengthIsNotSetOnMovieDataCommand_ReturnsMovieWhereLengthIsNull()
		{
			IMovieDataCommand sut = CreateSut(hasLength: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Length, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenUrlIsSetOnMovieDataCommand_ReturnsMovieWhereUrlIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasUrl: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Url, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenUrlIsSetOnMovieDataCommand_ReturnsMovieWhereUrlIsEqualToUrlFromMovieDataCommand()
		{
			string url = CreateValidUrl();
			IMovieDataCommand sut = CreateSut(hasUrl: true, url: url);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Url.AbsoluteUri, Is.EqualTo(url));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenUrlIsNotSetOnMovieDataCommand_ReturnsMovieWhereUrlIsNull()
		{
			IMovieDataCommand sut = CreateSut(hasUrl: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Url, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetOnMovieDataCommand_ReturnsMovieWhereImageIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasImage: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetOnMovieDataCommand_ReturnsMovieWhereImageIsNotEmpty()
		{
			IMovieDataCommand sut = CreateSut(hasImage: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetOnMovieDataCommand_ReturnsMovieWhereImageIsEqualToImageFromMovieDataCommand()
		{
			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMovieDataCommand sut = CreateSut(hasImage: true, image: image);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.EqualTo(image));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsNotSetOnMovieDataCommand_ReturnsMovieWhereImageIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasImage: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsNotSetOnMovieDataCommand_ReturnsMovieWhereImageIsEmpty()
		{
			IMovieDataCommand sut = CreateSut(hasImage: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDirectorsIsSetOnMovieDataCommand_ReturnsMovieWhereDirectorsIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasDirectors: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Directors, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDirectorsIsSetOnMovieDataCommand_ReturnsMovieWhereDirectorsIsNotEmpty()
		{
			IMovieDataCommand sut = CreateSut(hasDirectors: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Directors, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDirectorsIsSetOnMovieDataCommand_ReturnsMovieWhereDirectorsIsNonEmptyCollectionOfMatchingMediaPersonalitiesInDirectorsFromMovieDataCommand()
		{
			Guid[] mediaPersonalityIdentifiers = _fixture.CreateMany<Guid>(_random.Next(5, 15)).ToArray();
			IMovieDataCommand sut = CreateSut(hasDirectors: true, directors: mediaPersonalityIdentifiers);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(mediaPersonalityIdentifiers.All(mediaPersonalityIdentifier => result.Directors.SingleOrDefault(mediaPersonality => mediaPersonality.MediaPersonalityIdentifier == mediaPersonalityIdentifier) != null), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDirectorsIsNotSetOnMovieDataCommand_ReturnsMovieWhereDirectorsIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasDirectors: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Directors, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDirectorsIsNotSetOnMovieDataCommand_ReturnsMovieWhereDirectorsIsEmpty()
		{
			IMovieDataCommand sut = CreateSut(hasDirectors: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Directors, Is.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenActorsIsSetOnMovieDataCommand_ReturnsMovieWhereActorsIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasActors: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Actors, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenActorsIsSetOnMovieDataCommand_ReturnsMovieWhereActorsIsNotEmpty()
		{
			IMovieDataCommand sut = CreateSut(hasActors: true);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Actors, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenActorsIsSetOnMovieDataCommand_ReturnsMovieWhereActorsIsNonEmptyCollectionOfMatchingMediaPersonalitiesInActorsFromMovieDataCommand()
		{
			Guid[] mediaPersonalityIdentifiers = _fixture.CreateMany<Guid>(_random.Next(5, 15)).ToArray();
			IMovieDataCommand sut = CreateSut(hasActors: true, actors: mediaPersonalityIdentifiers);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(mediaPersonalityIdentifiers.All(mediaPersonalityIdentifier => result.Actors.SingleOrDefault(mediaPersonality => mediaPersonality.MediaPersonalityIdentifier == mediaPersonalityIdentifier) != null), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenActorsIsNotSetOnMovieDataCommand_ReturnsMovieWhereActorsIsNotNull()
		{
			IMovieDataCommand sut = CreateSut(hasActors: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Actors, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenActorsIsNotSetOnMovieDataCommand_ReturnsMovieWhereActorsIsEmpty()
		{
			IMovieDataCommand sut = CreateSut(hasActors: false);

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Actors, Is.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMovieWhereLendingsIsNotNull()
		{
			IMovieDataCommand sut = CreateSut();

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Lendings, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsMovieWhereLendingsIsEmpty()
		{
			IMovieDataCommand sut = CreateSut();

			IMovie result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Lendings, Is.Empty);
		}

		private IMovieDataCommand CreateSut(Guid? mediaIdentifier = null, string title = null, bool hasSubtitle = true, string subtitle = null, bool hasDescription = true, string description = null, bool hasDetails = true, string details = null, int? movieGenreIdentifier = null, IMovieGenre movieGenre = null, bool hasSpokenLanguageIdentifier = true, int? spokenLanguageIdentifier = null, ILanguage spokenLanguage = null, int? mediaTypeIdentifier = null, IMediaType mediaType = null, bool hasPublished = true, short? published = null, bool hasLength = true, short? length = null, bool hasUrl = true, string url = null, bool hasImage = true, byte[] image = null, bool hasDirectors = true, IEnumerable<Guid> directors = null, bool hasActors = true, IEnumerable<Guid> actors = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaPersonalityAsync(It.IsAny<Guid>()))
				.Returns<Guid>(mediaPersonalityIdentifier => Task.FromResult(_fixture.BuildMediaPersonalityMock(mediaPersonalityIdentifier).Object));
			_mediaLibraryRepositoryMock.Setup(m => m.GetMovieGenreAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(movieGenre ?? _fixture.BuildMovieGenreMock().Object));
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaTypeAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(mediaType  ?? _fixture.BuildMediaTypeMock().Object));

			_commonRepositoryMock.Setup(m => m.GetLanguageAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(spokenLanguage ?? _fixture.BuildLanguageMock().Object));

			return new MyMovieDataCommand(mediaIdentifier ?? Guid.NewGuid(), title ?? _fixture.Create<string>().ToUpper(), hasSubtitle ? subtitle ?? _fixture.Create<string>() : null, hasDescription ? description ?? _fixture.Create<string>() : null, hasDetails ? details ?? _fixture.Create<string>() : null, movieGenreIdentifier ?? _fixture.Create<int>(), hasSpokenLanguageIdentifier ? spokenLanguageIdentifier ?? _fixture.Create<int>() : null, mediaTypeIdentifier ?? _fixture.Create<int>(), hasPublished ? published ?? _fixture.Create<short>() : null, hasLength ? length ?? _fixture.Create<short>() : null, hasUrl ? url ?? CreateValidUrl() : null, hasImage ? image ?? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null, hasDirectors ? directors ?? _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray() : null, hasActors ? actors ?? _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray() : null);
		}

		private string CreateValidUrl()
		{
			return $"https://localhost/api/movie/{_fixture.Create<string>()}";
		}

		private class MyMovieDataCommand : BusinessLogic.MediaLibrary.Commands.MovieDataCommandBase
		{
			#region Constructor

			public MyMovieDataCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int movieGenreIdentifier, int? spokenLanguageIdentifier, int mediaTypeIdentifier, short? published, short? length, string url, byte[] image, IEnumerable<Guid> directors, IEnumerable<Guid> actors) 
				: base(mediaIdentifier, title, subtitle, description, details, movieGenreIdentifier, spokenLanguageIdentifier, mediaTypeIdentifier, published, length, url, image, directors, actors)
			{
			}

			#endregion
		}
	}
}