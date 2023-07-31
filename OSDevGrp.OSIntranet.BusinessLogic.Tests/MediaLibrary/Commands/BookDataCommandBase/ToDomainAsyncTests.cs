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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.BookDataCommandBase
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
			IBookDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToDomainAsync(null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ToDomainAsync_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IBookDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_AssertGetBookGenreAsyncWasCalledOnMediaLibraryRepositoryWithBookGenreIdentifierFromBookDataCommand()
		{
			int bookGenreIdentifier = _fixture.Create<int>();
			IBookDataCommand sut = CreateSut(bookGenreIdentifier: bookGenreIdentifier);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetBookGenreAsync(It.Is<int>(value => value == bookGenreIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenWrittenLanguageIdentifierIsSetOnBookDataCommand_AssertGetLanguageAsyncWasCalledOnCommonRepositoryWithWrittenLanguageIdentifierFromBookDataCommand()
		{
			int writtenLanguageIdentifier = _fixture.Create<int>();
			IBookDataCommand sut = CreateSut(hasWrittenLanguageIdentifier: true, writtenLanguageIdentifier: writtenLanguageIdentifier);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_commonRepositoryMock.Verify(m => m.GetLanguageAsync(It.Is<int>(value => value == writtenLanguageIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenWrittenLanguageIdentifierIsNotSetOnBookDataCommand_AssertGetLanguageAsyncWasNotCalledOnCommonRepository()
		{
			IBookDataCommand sut = CreateSut(hasWrittenLanguageIdentifier: false);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_commonRepositoryMock.Verify(m => m.GetLanguageAsync(It.IsAny<int>()), Times.Never());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_AssertGetMediaTypeAsyncWasCalledOnMediaLibraryRepositoryWithMediaTypeIdentifierFromBookDataCommand()
		{
			int mediaTypeIdentifier = _fixture.Create<int>();
			IBookDataCommand sut = CreateSut(mediaTypeIdentifier: mediaTypeIdentifier);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaTypeAsync(It.Is<int>(value => value == mediaTypeIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenAuthorsIsSetOnBookDataCommand_AssertGetMediaPersonalityAsyncWasCalledOnMediaLibraryRepositoryWithEachMediaPersonalityIdentifierInAuthorsFromBookDataCommand()
		{
			Guid[] mediaPersonalityIdentifiers = _fixture.CreateMany<Guid>(_random.Next(5, 15)).ToArray();
			IBookDataCommand sut = CreateSut(hasAuthors: true, authors: mediaPersonalityIdentifiers);

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
			IBookDataCommand sut = CreateSut();

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBook()
		{
			IBookDataCommand sut = CreateSut();

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.TypeOf<Book>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBookWhereMediaIdentifierIsEqualToMediaIdentifierFromBookDataCommand()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			IBookDataCommand sut = CreateSut(mediaIdentifier);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MediaIdentifier, Is.EqualTo(mediaIdentifier));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBookWhereTitleIsNotNull()
		{
			IBookDataCommand sut = CreateSut();

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Title, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBookWhereTitleIsNotEmpty()
		{
			IBookDataCommand sut = CreateSut();

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Title, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBookWhereTitleIsEqualToTitleFromBookDataCommand()
		{
			string title = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(title: title);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Title, Is.EqualTo(title.ToUpper()));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSubtitleIsSetOnBookDataCommand_ReturnsBookWhereSubtitleIsNotNull()
		{
			IBookDataCommand sut = CreateSut(hasSubtitle: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Subtitle, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSubtitleIsSetOnBookDataCommand_ReturnsBookWhereSubtitleIsNotEmpty()
		{
			IBookDataCommand sut = CreateSut(hasSubtitle: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Subtitle, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSubtitleIsSetOnBookDataCommand_ReturnsBookWhereSubtitleIsEqualToSubtitleFromBookDataCommand()
		{
			string subtitle = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(hasSubtitle: true, subtitle: subtitle);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Subtitle, Is.EqualTo(subtitle.ToUpper()));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSubtitleIsNotSetOnBookDataCommand_ReturnsBookWhereSubtitleIsNull()
		{
			IBookDataCommand sut = CreateSut(hasSubtitle: false);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Subtitle, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDescriptionIsSetOnBookDataCommand_ReturnsBookWhereDescriptionIsNotNull()
		{
			IBookDataCommand sut = CreateSut(hasDescription: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Description, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDescriptionIsSetOnBookDataCommand_ReturnsBookWhereDescriptionIsNotEmpty()
		{
			IBookDataCommand sut = CreateSut(hasDescription: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Description, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDescriptionIsSetOnBookDataCommand_ReturnsBookWhereDescriptionIsEqualToDescriptionFromBookDataCommand()
		{
			string description = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(hasDescription: true, description: description);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Description, Is.EqualTo(description));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDescriptionIsNotSetOnBookDataCommand_ReturnsBookWhereDescriptionIsNull()
		{
			IBookDataCommand sut = CreateSut(hasDescription: false);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Description, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDetailsIsSetOnBookDataCommand_ReturnsBookWhereDetailsIsNotNull()
		{
			IBookDataCommand sut = CreateSut(hasDetails: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Details, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDetailsIsSetOnBookDataCommand_ReturnsBookWhereDetailsIsNotEmpty()
		{
			IBookDataCommand sut = CreateSut(hasDetails: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Details, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDetailsIsSetOnBookDataCommand_ReturnsBookWhereDetailsIsEqualToDetailsFromBookDataCommand()
		{
			string details = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(hasDetails: true, details: details);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Details, Is.EqualTo(details));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenDetailsIsNotSetOnBookDataCommand_ReturnsBookWhereDetailsIsNull()
		{
			IBookDataCommand sut = CreateSut(hasDetails: false);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Details, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBookWhereBookGenreIsNotNull()
		{
			IBookDataCommand sut = CreateSut();

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.BookGenre, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBookWhereBookGenreIsEqualToMatchingBookGenreFromMediaLibraryRepository()
		{
			IBookGenre bookGenre = _fixture.BuildBookGenreMock().Object;
			IBookDataCommand sut = CreateSut(bookGenre: bookGenre);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.BookGenre, Is.EqualTo(bookGenre));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenWrittenLanguageIdentifierIsSetOnBookDataCommand_ReturnsBookWhereWrittenLanguageIsNotNull()
		{
			IBookDataCommand sut = CreateSut(hasWrittenLanguageIdentifier: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.WrittenLanguage, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenWrittenLanguageIdentifierIsSetOnBookDataCommand_ReturnsBookWhereWrittenLanguageIsEqualToMatchingLanguageFromCommonRepository()
		{
			ILanguage writtenLanguage = _fixture.BuildLanguageMock().Object;
			IBookDataCommand sut = CreateSut(hasWrittenLanguageIdentifier: true, writtenLanguage: writtenLanguage);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.WrittenLanguage, Is.EqualTo(writtenLanguage));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenWrittenLanguageIdentifierIsNotSetOnBookDataCommand_ReturnsBookWhereWrittenLanguageIsNull()
		{
			IBookDataCommand sut = CreateSut(hasWrittenLanguageIdentifier: false);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.WrittenLanguage, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBookWhereMediaTypeIsNotNull()
		{
			IBookDataCommand sut = CreateSut();

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MediaType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBookWhereMediaTypeIsEqualToMatchingMediaTypeFromMediaLibraryRepository()
		{
			IMediaType mediaType = _fixture.BuildMediaTypeMock().Object;
			IBookDataCommand sut = CreateSut(mediaType: mediaType);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MediaType, Is.EqualTo(mediaType));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenInternationalStandardBookNumberIsSetOnBookDataCommand_ReturnsBookWhereInternationalStandardBookNumberIsNotNull()
		{
			IBookDataCommand sut = CreateSut(hasInternationalStandardBookNumber: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.InternationalStandardBookNumber, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenInternationalStandardBookNumberIsSetOnBookDataCommand_ReturnsBookWhereInternationalStandardBookNumberIsNotEmpty()
		{
			IBookDataCommand sut = CreateSut(hasInternationalStandardBookNumber: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.InternationalStandardBookNumber, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenInternationalStandardBookNumberIsSetOnBookDataCommand_ReturnsBookWhereInternationalStandardBookNumberIsEqualToInternationalStandardBookNumberFromBookDataCommand()
		{
			string internationalStandardBookNumber = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(hasInternationalStandardBookNumber: true, internationalStandardBookNumber: internationalStandardBookNumber);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.InternationalStandardBookNumber, Is.EqualTo(internationalStandardBookNumber));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenInternationalStandardBookNumberIsNotSetOnBookDataCommand_ReturnsBookWhereInternationalStandardBookNumberIsNull()
		{
			IBookDataCommand sut = CreateSut(hasInternationalStandardBookNumber: false);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.InternationalStandardBookNumber, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPublishedIsSetOnBookDataCommand_ReturnsBookWherePublishedIsNotNull()
		{
			IBookDataCommand sut = CreateSut(hasPublished: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Published, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPublishedIsSetOnBookDataCommand_ReturnsBookWherePublishedIsEqualToPublishedFromBookDataCommand()
		{
			short published = _fixture.Create<short>();
			IBookDataCommand sut = CreateSut(hasPublished: true, published: published);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Published, Is.EqualTo(published));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPublishedIsNotSetOnBookDataCommand_ReturnsBookWherePublishedIsNull()
		{
			IBookDataCommand sut = CreateSut(hasPublished: false);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Published, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenUrlIsSetOnBookDataCommand_ReturnsBookWhereUrlIsNotNull()
		{
			IBookDataCommand sut = CreateSut(hasUrl: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Url, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenUrlIsSetOnBookDataCommand_ReturnsBookWhereUrlIsEqualToUrlFromBookDataCommand()
		{
			string url = CreateValidUrl();
			IBookDataCommand sut = CreateSut(hasUrl: true, url: url);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Url.AbsoluteUri, Is.EqualTo(url));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenUrlIsNotSetOnBookDataCommand_ReturnsBookWhereUrlIsNull()
		{
			IBookDataCommand sut = CreateSut(hasUrl: false);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Url, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetOnBookDataCommand_ReturnsBookWhereImageIsNotNull()
		{
			IBookDataCommand sut = CreateSut(hasImage: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetOnBookDataCommand_ReturnsBookWhereImageIsNotEmpty()
		{
			IBookDataCommand sut = CreateSut(hasImage: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsSetOnBookDataCommand_ReturnsBookWhereImageIsEqualToImageFromBookDataCommand()
		{
			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IBookDataCommand sut = CreateSut(hasImage: true, image: image);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.EqualTo(image));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsNotSetOnBookDataCommand_ReturnsBookWhereImageIsNotNull()
		{
			IBookDataCommand sut = CreateSut(hasImage: false);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenImageIsNotSetOnBookDataCommand_ReturnsBookWhereImageIsEmpty()
		{
			IBookDataCommand sut = CreateSut(hasImage: false);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Image, Is.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenAuthorsIsSetOnBookDataCommand_ReturnsBookWhereAuthorsIsNotNull()
		{
			IBookDataCommand sut = CreateSut(hasAuthors: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Authors, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenAuthorsIsSetOnBookDataCommand_ReturnsBookWhereAuthorsIsNotEmpty()
		{
			IBookDataCommand sut = CreateSut(hasAuthors: true);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Authors, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenAuthorsIsSetOnBookDataCommand_ReturnsBookWhereAuthorsIsNonEmptyCollectionOfMatchingMediaPersonalitiesInAuthorsFromBookDataCommand()
		{
			Guid[] mediaPersonalityIdentifiers = _fixture.CreateMany<Guid>(_random.Next(5, 15)).ToArray();
			IBookDataCommand sut = CreateSut(hasAuthors: true, authors: mediaPersonalityIdentifiers);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(mediaPersonalityIdentifiers.All(mediaPersonalityIdentifier => result.Authors.SingleOrDefault(mediaPersonality => mediaPersonality.MediaPersonalityIdentifier == mediaPersonalityIdentifier) != null), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenAuthorsIsNotSetOnBookDataCommand_ReturnsBookWhereAuthorsIsNotNull()
		{
			IBookDataCommand sut = CreateSut(hasAuthors: false);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Authors, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenAuthorsIsNotSetOnBookDataCommand_ReturnsBookWhereAuthorsIsEmpty()
		{
			IBookDataCommand sut = CreateSut(hasAuthors: false);

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Authors, Is.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBookWhereLendingsIsNotNull()
		{
			IBookDataCommand sut = CreateSut();

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Lendings, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBookWhereLendingsIsEmpty()
		{
			IBookDataCommand sut = CreateSut();

			IBook result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Lendings, Is.Empty);
		}

		private IBookDataCommand CreateSut(Guid? mediaIdentifier = null, string title = null, bool hasSubtitle = true, string subtitle = null, bool hasDescription = true, string description = null, bool hasDetails = true, string details = null, int? bookGenreIdentifier = null, IBookGenre bookGenre = null, bool hasWrittenLanguageIdentifier = true, int? writtenLanguageIdentifier = null, ILanguage writtenLanguage = null, int? mediaTypeIdentifier = null, IMediaType mediaType = null, bool hasInternationalStandardBookNumber = true, string internationalStandardBookNumber = null, bool hasPublished = true, short? published = null, bool hasUrl = true, string url = null, bool hasImage = true, byte[] image = null, bool hasAuthors = true, IEnumerable<Guid> authors = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaPersonalityAsync(It.IsAny<Guid>()))
				.Returns<Guid>(mediaPersonalityIdentifier => Task.FromResult(_fixture.BuildMediaPersonalityMock(mediaPersonalityIdentifier).Object));
			_mediaLibraryRepositoryMock.Setup(m => m.GetBookGenreAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(bookGenre ?? _fixture.BuildBookGenreMock().Object));
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaTypeAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(mediaType ?? _fixture.BuildMediaTypeMock().Object));

			_commonRepositoryMock.Setup(m => m.GetLanguageAsync(It.IsAny<int>()))
				.Returns(Task.FromResult(writtenLanguage ?? _fixture.BuildLanguageMock().Object));

			return new MyBookDataCommand(mediaIdentifier ?? Guid.NewGuid(), title ?? _fixture.Create<string>(), hasSubtitle ? subtitle ?? _fixture.Create<string>() : null, hasDescription ? description ?? _fixture.Create<string>() : null, hasDetails ? details ?? _fixture.Create<string>() : null, bookGenreIdentifier ?? _fixture.Create<int>(), hasWrittenLanguageIdentifier ? writtenLanguageIdentifier ?? _fixture.Create<int>() : null, mediaTypeIdentifier ?? _fixture.Create<int>(), hasInternationalStandardBookNumber ? internationalStandardBookNumber ?? _fixture.Create<string>() : null, hasPublished ? published ?? _fixture.Create<short>() : null, hasUrl ? url ?? CreateValidUrl() : null, hasImage ? image ?? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null, hasAuthors ? authors ?? _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray() : null);
		}

		private string CreateValidUrl()
		{
			return $"https://localhost/api/book/{_fixture.Create<string>()}";
		}

		private class MyBookDataCommand : BusinessLogic.MediaLibrary.Commands.BookDataCommandBase
		{
			#region Constructor

			public MyBookDataCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int bookGenreIdentifier, int? writtenLanguageIdentifier, int mediaTypeIdentifier, string internationalStandardBookNumber, short? published, string url, byte[] image, IEnumerable<Guid> authors) 
				: base(mediaIdentifier, title, subtitle, description, details, bookGenreIdentifier, writtenLanguageIdentifier, mediaTypeIdentifier, internationalStandardBookNumber, published, url, image, authors)
			{
			}

			#endregion
		}
	}
}