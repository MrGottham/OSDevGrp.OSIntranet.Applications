using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class MediaExistsAsyncTests : MediaLibraryRepositoryTestBase
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
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMovieAndMediaIdentifierIsKnown_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IMovie>(WithExistingMovieIdentifier());

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMovieAndMediaIdentifierIsUnknown_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IMovie>(Guid.NewGuid());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMusicAndMediaIdentifierIsKnown_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IMusic>(WithExistingMusicIdentifier());

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMusicAndMediaIdentifierIsUnknown_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IMusic>(Guid.NewGuid());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsBookAndMediaIdentifierIsKnown_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IBook>(WithExistingBookIdentifier());

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsBookAndMediaIdentifierIsUnknown_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IBook>(Guid.NewGuid());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMediaAndMediaIdentifierIsAnything_ThrowsIntranetRepositoryException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.MediaExistsAsync<IMedia>(Guid.NewGuid()));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMediaAndMediaIdentifierIsAnything_ThrowsNotSupportedExceptionWhereErrorCodeIsEqualToRepositoryError()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.MediaExistsAsync<IMedia>(Guid.NewGuid()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.RepositoryError));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMediaAndMediaIdentifierIsAnything_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.MediaExistsAsync<IMedia>(Guid.NewGuid()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMediaAndMediaIdentifierIsAnything_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotSupportedException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.MediaExistsAsync<IMedia>(Guid.NewGuid()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.TypeOf<NotSupportedException>());
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMovieAndTitleIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaExistsAsync<IMovie>(null, _fixture.Create<string>(), _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("title"));
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMovieAndTitleIsEmpty_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaExistsAsync<IMovie>(string.Empty, _fixture.Create<string>(), _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("title"));
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMovieAndTitleIsWhiteSpace_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaExistsAsync<IMovie>(" ", _fixture.Create<string>(), _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("title"));
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMusicAndTitleIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaExistsAsync<IMusic>(null, _fixture.Create<string>(), _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("title"));
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMusicAndTitleIsEmpty_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaExistsAsync<IMusic>(string.Empty, _fixture.Create<string>(), _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("title"));
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMusicAndTitleIsWhiteSpace_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaExistsAsync<IMusic>(" ", _fixture.Create<string>(), _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("title"));
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsBookAndTitleIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaExistsAsync<IBook>(null, _fixture.Create<string>(), _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("title"));
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsBookAndTitleIsEmpty_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaExistsAsync<IBook>(string.Empty, _fixture.Create<string>(), _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("title"));
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsBookAndTitleIsWhiteSpace_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaExistsAsync<IBook>(" ", _fixture.Create<string>(), _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("title"));
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMediaAndTitleIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaExistsAsync<IMedia>(null, _fixture.Create<string>(), _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("title"));
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMediaAndTitleIsEmpty_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaExistsAsync<IMedia>(string.Empty, _fixture.Create<string>(), _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("title"));
		}

		[Test]
		[Category("UnitTest")]
		public void MediaExistsAsync_WhenGenericMediaIsMediaAndTitleIsWhiteSpace_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.MediaExistsAsync<IMedia>(" ", _fixture.Create<string>(), _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("title"));
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMovieAndTitleAndSubtitleAndMediaTypeIsKnownAsMovie_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMovie movie = await sut.GetMediaAsync<IMovie>(WithExistingMovieIdentifier());

			bool result = await sut.MediaExistsAsync<IMovie>(movie.Title, movie.Subtitle, movie.MediaType.Number);

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMovieAndTitleAndMediaTypeIsKnownAsMovieWithOtherSubtitle_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMovie movie = await sut.GetMediaAsync<IMovie>(WithExistingMovieIdentifier());

			bool result = await sut.MediaExistsAsync<IMovie>(movie.Title, _fixture.Create<string>(), movie.MediaType.Number);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMovieAndTitleAndSubtitleIsKnownAsMovieWithOtherMediaType_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMovie movie = await sut.GetMediaAsync<IMovie>(WithExistingMovieIdentifier());

			bool result = await sut.MediaExistsAsync<IMovie>(movie.Title, movie.Subtitle, movie.MediaType.Number + 100);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMovieAndTitleAndSubtitleAndMediaTypeIsKnownAsMusic_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMusic music = await sut.GetMediaAsync<IMusic>(WithExistingMusicIdentifier());

			bool result = await sut.MediaExistsAsync<IMovie>(music.Title, music.Subtitle, music.MediaType.Number);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMovieAndTitleAndSubtitleAndMediaTypeIsKnownAsBook_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBook book = await sut.GetMediaAsync<IBook>(WithExistingBookIdentifier());

			bool result = await sut.MediaExistsAsync<IMovie>(book.Title, book.Subtitle, book.MediaType.Number);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		[TestCase("Nulla non hendrerit justo")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMovieAndTitleAndSubtitleAndMediaTypeIsUnknown_ReturnsFalse(string subtitle)
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IMovie>(_fixture.Create<string>(), subtitle, _fixture.Create<int>());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMusicAndTitleAndSubtitleAndMediaTypeIsKnownAsMovie_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMovie movie = await sut.GetMediaAsync<IMovie>(WithExistingMovieIdentifier());

			bool result = await sut.MediaExistsAsync<IMusic>(movie.Title, movie.Subtitle, movie.MediaType.Number);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMusicAndTitleAndSubtitleAndMediaTypeIsKnownAsMusic_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMusic music = await sut.GetMediaAsync<IMusic>(WithExistingMusicIdentifier());

			bool result = await sut.MediaExistsAsync<IMusic>(music.Title, music.Subtitle, music.MediaType.Number);

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMusicAndTitleAndMediaTypeIsKnownAsMusicWithOtherSubtitle_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMusic music = await sut.GetMediaAsync<IMusic>(WithExistingMusicIdentifier());

			bool result = await sut.MediaExistsAsync<IMusic>(music.Title, _fixture.Create<string>(), music.MediaType.Number);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMusicAndTitleAndSubtitleIsKnownAsMusicWithOtherMediaType_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMusic music = await sut.GetMediaAsync<IMusic>(WithExistingMusicIdentifier());

			bool result = await sut.MediaExistsAsync<IMusic>(music.Title, music.Subtitle, music.MediaType.Number + 100);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMusicAndTitleAndSubtitleAndMediaTypeIsKnownAsBook_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBook book = await sut.GetMediaAsync<IBook>(WithExistingBookIdentifier());

			bool result = await sut.MediaExistsAsync<IMusic>(book.Title, book.Subtitle, book.MediaType.Number);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		[TestCase("Nulla non hendrerit justo")]
		public async Task MediaExistsAsync_WhenGenericMediaIsMusicAndTitleAndSubtitleAndMediaTypeIsUnknown_ReturnsFalse(string subtitle)
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IMusic>(_fixture.Create<string>(), subtitle, _fixture.Create<int>());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsBookAndTitleAndSubtitleAndMediaTypeIsKnownAsMovie_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMovie movie = await sut.GetMediaAsync<IMovie>(WithExistingMovieIdentifier());

			bool result = await sut.MediaExistsAsync<IBook>(movie.Title, movie.Subtitle, movie.MediaType.Number);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsBookAndTitleAndSubtitleAndMediaTypeIsKnownAsMusic_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMusic music = await sut.GetMediaAsync<IMusic>(WithExistingMusicIdentifier());

			bool result = await sut.MediaExistsAsync<IBook>(music.Title, music.Subtitle, music.MediaType.Number);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsBookAndTitleAndSubtitleAndMediaTypeIsKnownAsBook_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBook book = await sut.GetMediaAsync<IBook>(WithExistingBookIdentifier());

			bool result = await sut.MediaExistsAsync<IBook>(book.Title, book.Subtitle, book.MediaType.Number);

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsBookAndTitleAndMediaTypeIsKnownAsBookWithOtherSubtitle_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBook book = await sut.GetMediaAsync<IBook>(WithExistingBookIdentifier());

			bool result = await sut.MediaExistsAsync<IBook>(book.Title, _fixture.Create<string>(), book.MediaType.Number);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaExistsAsync_WhenGenericMediaIsBookAndTitleAndSubtitleIsKnownAsBookWithOtherMediaType_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBook book = await sut.GetMediaAsync<IBook>(WithExistingBookIdentifier());

			bool result = await sut.MediaExistsAsync<IBook>(book.Title, book.Subtitle, book.MediaType.Number + 100);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("IntegrationTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		[TestCase("Nulla non hendrerit justo")]
		public async Task MediaExistsAsync_WhenGenericMediaIsBookAndTitleAndSubtitleAndMediaTypeIsUnknown_ReturnsFalse(string subtitle)
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaExistsAsync<IBook>(_fixture.Create<string>(), subtitle, _fixture.Create<int>());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		[TestCase("Nulla non hendrerit justo")]
		public void MediaExistsAsync_WhenGenericMediaIsMediaAndTitleIsAnything_ThrowsIntranetRepositoryException(string subtitle)
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.MediaExistsAsync<IMedia>(_fixture.Create<string>(), subtitle, _fixture.Create<int>()));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		[TestCase("Nulla non hendrerit justo")]
		public void MediaExistsAsync_WhenGenericMediaIsMediaAndTitleIsAnything_ThrowsNotSupportedExceptionWhereErrorCodeIsEqualToRepositoryError(string subtitle)
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.MediaExistsAsync<IMedia>(_fixture.Create<string>(), subtitle, _fixture.Create<int>()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.RepositoryError));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		[TestCase("Nulla non hendrerit justo")]
		public void MediaExistsAsync_WhenGenericMediaIsMediaAndTitleIsAnything_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull(string subtitle)
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.MediaExistsAsync<IMedia>(_fixture.Create<string>(), subtitle, _fixture.Create<int>()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		[TestCase("Nulla non hendrerit justo")]
		public void MediaExistsAsync_WhenGenericMediaIsMediaAndTitleIsAnything_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotSupportedException(string subtitle)
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.MediaExistsAsync<IMedia>(_fixture.Create<string>(), subtitle, _fixture.Create<int>()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.TypeOf<NotSupportedException>());
			// ReSharper restore PossibleNullReferenceException
		}
	}
}