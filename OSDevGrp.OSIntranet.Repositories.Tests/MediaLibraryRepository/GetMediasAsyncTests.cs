using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class GetMediasAsyncTests : MediaLibraryRepositoryTestBase
	{
		#region Private variables

		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenNoGenericMediaWasGivenAndTitleFilterIsNull_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMedia> result = await sut.GetMediasAsync();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenNoGenericMediaWasGivenAndTitleFilterIsNull_ReturnsNonEmptyCollectionOfMedias()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMedia> result = await sut.GetMediasAsync();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenNoGenericMediaWasGivenAndTitleFilterIsEmpty_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMedia> result = await sut.GetMediasAsync(string.Empty);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenNoGenericMediaWasGivenAndTitleFilterIsEmpty_ReturnsNonEmptyCollectionOfMedias()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMedia> result = await sut.GetMediasAsync(string.Empty);

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenNoGenericMediaWasGivenAndTitleFilterIsWhiteSpace_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMedia> result = await sut.GetMediasAsync(" ");

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenNoGenericMediaWasGivenAndTitleFilterIsWhiteSpace_ReturnsNonEmptyCollectionOfMedias()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMedia> result = await sut.GetMediasAsync(" ");

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenNoGenericMediaWasGivenAndTitleFilterMatchesOneOrMoreMediaTitles_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMusic knownMusic = await sut.GetMediaAsync<IMusic>(WithExistingMusicIdentifier());
			IBook knownBook = await sut.GetMediaAsync<IBook>(WithExistingBookIdentifier());
			IMovie knownMovie = await sut.GetMediaAsync<IMovie>(WithExistingMovieIdentifier());

			string titleFilter;
			if (_random.Next(100) > 50)
			{
				titleFilter = knownMovie.Title.Substring(0, _random.Next(1, knownMovie.Title.Length - 1));
			}
			else if (_random.Next(100) > 50)
			{
				titleFilter = knownMusic.Title.Substring(0, _random.Next(1, knownMusic.Title.Length - 1));
			}
			else
			{
				titleFilter = knownBook.Title.Substring(0, _random.Next(1, knownBook.Title.Length - 1));
			}
			IEnumerable<IMedia> result = await sut.GetMediasAsync(titleFilter);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenNoGenericMediaWasGivenAndTitleFilterMatchesOneOrMoreMediaTitles_ReturnsNonEmptyCollectionOfMedias()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMusic knownMusic = await sut.GetMediaAsync<IMusic>(WithExistingMusicIdentifier());
			IBook knownBook = await sut.GetMediaAsync<IBook>(WithExistingBookIdentifier());
			IMovie knownMovie = await sut.GetMediaAsync<IMovie>(WithExistingMovieIdentifier());

			string titleFilter;
			if (_random.Next(100) > 50)
			{
				titleFilter = knownMovie.Title.Substring(0, _random.Next(1, knownMovie.Title.Length - 1));
			}
			else if (_random.Next(100) > 50)
			{
				titleFilter = knownMusic.Title.Substring(0, _random.Next(1, knownMusic.Title.Length - 1));
			}
			else
			{
				titleFilter = knownBook.Title.Substring(0, _random.Next(1, knownBook.Title.Length - 1));
			}
			IEnumerable<IMedia> result = await sut.GetMediasAsync(titleFilter);

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenNoGenericMediaWasGivenAndTitleFilterDoesNotMatchOneOrMoreMediaTitles_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMedia> result = await sut.GetMediasAsync(_fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenNoGenericMediaWasGivenAndTitleFilterDoesNotMatchOneOrMoreMediaTitles_ReturnsEmptyCollection()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMedia> result = await sut.GetMediasAsync(_fixture.Create<string>());

			Assert.That(result, Is.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMovieAndTitleFilterIsNull_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMovie> result = await sut.GetMediasAsync<IMovie>();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMovieAndTitleFilterIsNull_ReturnsNonEmptyCollectionOfMovies()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMovie> result = await sut.GetMediasAsync<IMovie>();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMovieAndTitleFilterIsEmpty_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMovie> result = await sut.GetMediasAsync<IMovie>(string.Empty);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMovieAndTitleFilterIsEmpty_ReturnsNonEmptyCollectionOfMovies()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMovie> result = await sut.GetMediasAsync<IMovie>(string.Empty);

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMovieAndTitleFilterIsWhiteSpace_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMovie> result = await sut.GetMediasAsync<IMovie>(" ");

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMovieAndTitleFilterIsWhiteSpace_ReturnsNonEmptyCollectionOfMovies()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMovie> result = await sut.GetMediasAsync<IMovie>(" ");

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMovieAndTitleFilterMatchesOneOrMoreMovieTitles_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMovie knownMovie = await sut.GetMediaAsync<IMovie>(WithExistingMovieIdentifier());

			string titleFilter = knownMovie.Title.Substring(0, _random.Next(1, knownMovie.Title.Length - 1));
			IEnumerable<IMovie> result = await sut.GetMediasAsync<IMovie>(titleFilter);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMovieAndTitleFilterMatchesOneOrMoreMovieTitles_ReturnsNonEmptyCollectionOfMovies()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMovie knownMovie = await sut.GetMediaAsync<IMovie>(WithExistingMovieIdentifier());

			string titleFilter = knownMovie.Title.Substring(0, _random.Next(1, knownMovie.Title.Length - 1));
			IEnumerable<IMovie> result = await sut.GetMediasAsync<IMovie>(titleFilter);

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMovieAndTitleFilterDoesNotMatchOneOrMoreMovieTitles_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMovie> result = await sut.GetMediasAsync<IMovie>(_fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMovieAndTitleFilterDoesNotMatchOneOrMoreMovieTitles_ReturnsEmptyCollection()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMovie> result = await sut.GetMediasAsync<IMovie>(_fixture.Create<string>());

			Assert.That(result, Is.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMusicAndTitleFilterIsNull_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMusic> result = await sut.GetMediasAsync<IMusic>();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMusicAndTitleFilterIsNull_ReturnsNonEmptyCollectionOfMusic()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMusic> result = await sut.GetMediasAsync<IMusic>();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMusicAndTitleFilterIsEmpty_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMusic> result = await sut.GetMediasAsync<IMusic>(string.Empty);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMusicAndTitleFilterIsEmpty_ReturnsNonEmptyCollectionOfMusic()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMusic> result = await sut.GetMediasAsync<IMusic>(string.Empty);

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMusicAndTitleFilterIsWhiteSpace_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMusic> result = await sut.GetMediasAsync<IMusic>(" ");

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMusicAndTitleFilterIsWhiteSpace_ReturnsNonEmptyCollectionOfMusic()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMusic> result = await sut.GetMediasAsync<IMusic>(" ");

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMusicAndTitleFilterMatchesOneOrMoreMusicTitles_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMusic knownMusic = await sut.GetMediaAsync<IMusic>(WithExistingMusicIdentifier());

			string titleFilter = knownMusic.Title.Substring(0, _random.Next(1, knownMusic.Title.Length - 1));
			IEnumerable<IMusic> result = await sut.GetMediasAsync<IMusic>(titleFilter);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMusicAndTitleFilterMatchesOneOrMoreMusicTitles_ReturnsNonEmptyCollectionOfMusic()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMusic knownMusic = await sut.GetMediaAsync<IMusic>(WithExistingMusicIdentifier());

			string titleFilter = knownMusic.Title.Substring(0, _random.Next(1, knownMusic.Title.Length - 1));
			IEnumerable<IMusic> result = await sut.GetMediasAsync<IMusic>(titleFilter);

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMusicAndTitleFilterDoesNotMatchOneOrMoreMusicTitles_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMusic> result = await sut.GetMediasAsync<IMusic>(_fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsMusicAndTitleFilterDoesNotMatchOneOrMoreMusicTitles_ReturnsEmptyCollection()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMusic> result = await sut.GetMediasAsync<IMusic>(_fixture.Create<string>());

			Assert.That(result, Is.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsBookAndTitleFilterIsNull_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBook> result = await sut.GetMediasAsync<IBook>();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsBookAndTitleFilterIsNull_ReturnsNonEmptyCollectionOfBooks()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBook> result = await sut.GetMediasAsync<IBook>();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsBookAndTitleFilterIsEmpty_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBook> result = await sut.GetMediasAsync<IBook>(string.Empty);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsBookAndTitleFilterIsEmpty_ReturnsNonEmptyCollectionOfBooks()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBook> result = await sut.GetMediasAsync<IBook>(string.Empty);

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsBookAndTitleFilterIsWhiteSpace_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBook> result = await sut.GetMediasAsync<IBook>(" ");

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsBookAndTitleFilterIsWhiteSpace_ReturnsNonEmptyCollectionOfBooks()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBook> result = await sut.GetMediasAsync<IBook>(" ");

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsBookAndTitleFilterMatchesOneOrMoreBookTitles_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBook knownBook = await sut.GetMediaAsync<IBook>(WithExistingBookIdentifier());

			string titleFilter = knownBook.Title.Substring(0, _random.Next(1, knownBook.Title.Length - 1));
			IEnumerable<IBook> result = await sut.GetMediasAsync<IBook>(titleFilter);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsBookAndTitleFilterMatchesOneOrMoreBookTitles_ReturnsNonEmptyCollectionOfBooks()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBook knownBook = await sut.GetMediaAsync<IBook>(WithExistingBookIdentifier());

			string titleFilter = knownBook.Title.Substring(0, _random.Next(1, knownBook.Title.Length - 1));
			IEnumerable<IBook> result = await sut.GetMediasAsync<IBook>(titleFilter);

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsBookAndTitleFilterDoesNotMatchOneOrMoreBookTitles_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBook> result = await sut.GetMediasAsync<IBook>(_fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediasAsync_WhenGenericMediaIsBookAndTitleFilterDoesNotMatchOneOrMoreBookTitles_ReturnsEmptyCollection()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBook> result = await sut.GetMediasAsync<IBook>(_fixture.Create<string>());

			Assert.That(result, Is.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterIsNull_ThrowsIntranetRepositoryException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterIsNull_ThrowsNotSupportedExceptionWhereErrorCodeIsEqualToRepositoryError()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.RepositoryError));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterIsNull_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterIsNull_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotSupportedException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>());

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.TypeOf<NotSupportedException>());
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterIsEmpty_ThrowsIntranetRepositoryException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>(string.Empty));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterIsEmpty_ThrowsNotSupportedExceptionWhereErrorCodeIsEqualToRepositoryError()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>(string.Empty));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.RepositoryError));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterIsEmpty_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>(string.Empty));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterIsEmpty_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotSupportedException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>(string.Empty));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.TypeOf<NotSupportedException>());
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterIsWhiteSpace_ThrowsIntranetRepositoryException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>(" "));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterIsWhiteSpace_ThrowsNotSupportedExceptionWhereErrorCodeIsEqualToRepositoryError()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>(" "));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.RepositoryError));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterIsWhiteSpace_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>(" "));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterIsWhiteSpace_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotSupportedException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>(" "));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.TypeOf<NotSupportedException>());
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterHasValue_ThrowsIntranetRepositoryException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>(_fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterHasValue_ThrowsNotSupportedExceptionWhereErrorCodeIsEqualToRepositoryError()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>(_fixture.Create<string>()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.RepositoryError));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterHasValue_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>(_fixture.Create<string>()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediasAsync_WhenGenericMediaIsMediaAndTitleFilterHasValue_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotSupportedException()
		{
			IMediaLibraryRepository sut = CreateSut();

			IntranetRepositoryException result = Assert.ThrowsAsync<IntranetRepositoryException>(async () => await sut.GetMediasAsync<IMedia>(_fixture.Create<string>()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.InnerException, Is.TypeOf<NotSupportedException>());
			// ReSharper restore PossibleNullReferenceException
		}
	}
}