using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
	public static class MediaLibraryMockBuilder
    {
	    public static Mock<IMovie> BuildMovieMock(this Fixture fixture, Guid? mediaIdentifier = null, string title = null, string subtitle = null, string description = null, string details = null, IMovieGenre movieGenre = null, ILanguage spokenLanguage = null, IMediaType mediaType = null, short? published = null, short? length = null, Uri url = null, byte[] image = null, IEnumerable<IMediaPersonality> directors = null, IEnumerable<IMediaPersonality> actors = null, bool? deletable = false, string toString = null, bool? equals = null, int? hashCode = null, IEnumerable<IMediaBinding> mediaBindings = null)
	    {
		    NullGuard.NotNull(fixture, nameof(fixture));

		    Random random = new Random(fixture.Create<int>());

		    Mock<IMovie> movieMock = fixture.BuildMediaMock<IMovie>(random, mediaIdentifier, title, subtitle, description, details, mediaType, published, url, image, deletable, toString, equals, hashCode, mediaBindings);
		    movieMock.Setup(m => m.MovieGenre)
			    .Returns(movieGenre ?? fixture.BuildMovieGenreMock().Object);
		    movieMock.Setup(m => m.SpokenLanguage)
			    .Returns(spokenLanguage ?? (random.Next(100) > 50 ? fixture.BuildLanguageMock().Object : null));
		    movieMock.Setup(m => m.Length)
			    .Returns(length ?? (random.Next(100) > 50 ? (short)random.Next(60, 180) : (short?)null));
		    movieMock.Setup(m => m.Directors)
			    .Returns(directors ?? new[]
			    {
				    fixture.BuildMediaPersonalityMock(roles: new[] { MediaRole.Director }).Object,
				    fixture.BuildMediaPersonalityMock(roles: new[] { MediaRole.Director }).Object
			    });
		    movieMock.Setup(m => m.Actors)
			    .Returns(actors ?? new[]
			    {
				    fixture.BuildMediaPersonalityMock(roles: new[] { MediaRole.Actor }).Object,
				    fixture.BuildMediaPersonalityMock(roles: new[] { MediaRole.Actor }).Object,
				    fixture.BuildMediaPersonalityMock(roles: new[] { MediaRole.Actor }).Object
			    });
		    return movieMock;
	    }

	    public static Mock<IMusic> BuildMusicMock(this Fixture fixture, Guid? mediaIdentifier = null, string title = null, string subtitle = null, string description = null, string details = null, IMusicGenre musicGenre = null, IMediaType mediaType = null, short? published = null, short? tracks = null, Uri url = null, byte[] image = null, IEnumerable<IMediaPersonality> artists = null, bool? deletable = false, string toString = null, bool? equals = null, int? hashCode = null, IEnumerable<IMediaBinding> mediaBindings = null)
	    {
		    NullGuard.NotNull(fixture, nameof(fixture));

		    Random random = new Random(fixture.Create<int>());

		    Mock<IMusic> musicMock = fixture.BuildMediaMock<IMusic>(random, mediaIdentifier, title, subtitle, description, details, mediaType, published, url, image, deletable, toString, equals, hashCode, mediaBindings);
		    musicMock.Setup(m => m.MusicGenre)
			    .Returns(musicGenre ?? fixture.BuildMusicGenreMock().Object);
		    musicMock.Setup(m => m.Tracks)
			    .Returns(tracks ?? (random.Next(100) > 50 ? (short)random.Next(1, 15) : (short?)null));
		    musicMock.Setup(m => m.Artists)
			    .Returns(artists ?? new[]
			    {
				    fixture.BuildMediaPersonalityMock(roles: new[] { MediaRole.Artist }).Object,
				    fixture.BuildMediaPersonalityMock(roles: new[] { MediaRole.Artist }).Object,
				    fixture.BuildMediaPersonalityMock(roles: new[] { MediaRole.Artist }).Object
			    });
		    return musicMock;
	    }

	    public static Mock<IBook> BuildBookMock(this Fixture fixture, Guid? mediaIdentifier = null, string title = null, string subtitle = null, string description = null, string details = null, IBookGenre bookGenre = null, ILanguage writtenLanguage = null, IMediaType mediaType = null, string internationalStandardBookNumber = null, short? published = null, Uri url = null, byte[] image = null, IEnumerable<IMediaPersonality> authors = null, bool? deletable = false, string toString = null, bool? equals = null, int? hashCode = null, IEnumerable<IMediaBinding> mediaBindings = null)
	    {
		    NullGuard.NotNull(fixture, nameof(fixture));

		    Random random = new Random(fixture.Create<int>());

		    Mock<IBook> bookMock = fixture.BuildMediaMock<IBook>(random, mediaIdentifier, title, subtitle, description, details, mediaType, published, url, image, deletable, toString, equals, hashCode, mediaBindings);
		    bookMock.Setup(m => m.BookGenre)
			    .Returns(bookGenre ?? fixture.BuildBookGenreMock().Object);
		    bookMock.Setup(m => m.WrittenLanguage)
			    .Returns(writtenLanguage ?? (random.Next(100) > 50 ? fixture.BuildLanguageMock().Object : null));
		    bookMock.Setup(m => m.InternationalStandardBookNumber)
			    .Returns(internationalStandardBookNumber ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
		    bookMock.Setup(m => m.Authors)
			    .Returns(authors ?? new[]
			    {
				    fixture.BuildMediaPersonalityMock(roles: new[] { MediaRole.Author }).Object,
				    fixture.BuildMediaPersonalityMock(roles: new[] { MediaRole.Author }).Object,
				    fixture.BuildMediaPersonalityMock(roles: new[] { MediaRole.Author }).Object
			    });
		    return bookMock;
	    }

	    public static Mock<IMedia> BuildMediaMock(this Fixture fixture, Guid? mediaIdentifier = null, string title = null, string subtitle = null, string description = null, string details = null, IMediaType mediaType = null, short? published = null, Uri url = null, byte[] image = null, bool? deletable = false, string toString = null, bool? equals = null, int? hashCode = null, IEnumerable<IMediaBinding> mediaBindings = null)
	    {
		    NullGuard.NotNull(fixture, nameof(fixture));

			return fixture.BuildMediaMock<IMedia>(new Random(fixture.Create<int>()), mediaIdentifier, title, subtitle, description, details, mediaType, published, url, image, deletable, toString, equals, hashCode, mediaBindings);
	    }

		public static Mock<IMediaPersonality> BuildMediaPersonalityMock(this Fixture fixture, Guid? mediaPersonalityIdentifier = null, string givenName = null, string middleName = null, string surname = null, INationality nationality = null, IEnumerable<MediaRole> roles = null, DateTime? birthDate = null, DateTime? dateOfDead = null, Uri url = null, byte[] image = null, bool? deletable = false, string toString = null, bool? equals = null, int? hashCode = null)
	    {
		    NullGuard.NotNull(fixture, nameof(fixture));

		    Random random = new Random(fixture.Create<int>());

		    Mock<IMediaPersonality> mediaPersonalityMock = new Mock<IMediaPersonality>();
		    mediaPersonalityMock.Setup(m => m.MediaPersonalityIdentifier)
			    .Returns(mediaPersonalityIdentifier ?? Guid.NewGuid());
		    mediaPersonalityMock.Setup(m => m.GivenName)
			    .Returns(givenName ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
		    mediaPersonalityMock.Setup(m => m.MiddleName)
			    .Returns(middleName ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
		    mediaPersonalityMock.Setup(m => m.Surname)
			    .Returns(surname ?? fixture.Create<string>());
		    mediaPersonalityMock.Setup(m => m.Nationality)
			    .Returns(nationality ?? fixture.BuildNationalityMock().Object);
		    mediaPersonalityMock.Setup(m => m.Roles)
			    .Returns(roles ?? fixture.CreateMany<MediaRole>(1).ToArray());
		    mediaPersonalityMock.Setup(m => m.BirthDate)
			    .Returns(birthDate ?? (random.Next(100) > 50 ? fixture.Create<DateTime>() : (DateTime?)null));
		    mediaPersonalityMock.Setup(m => m.DateOfDead)
			    .Returns(dateOfDead ?? (random.Next(100) > 50 ? fixture.Create<DateTime>() : (DateTime?)null));
		    mediaPersonalityMock.Setup(m => m.Url)
			    .Returns(url ?? (random.Next(100) > 50 ? new Uri($"https://localhost/api/mediapersonalities/{Guid.NewGuid():D}") : null));
		    mediaPersonalityMock.Setup(m => m.Image)
			    .Returns(image ?? (random.Next(100) > 50 ? fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray() : Array.Empty<byte>()));
			mediaPersonalityMock.Setup(m => m.Deletable)
			    .Returns(deletable ?? fixture.Create<bool>());
		    mediaPersonalityMock.Setup(m => m.CreatedDateTime)
			    .Returns(fixture.Create<DateTime>());
		    mediaPersonalityMock.Setup(m => m.CreatedByIdentifier)
			    .Returns(fixture.Create<string>());
		    mediaPersonalityMock.Setup(m => m.ModifiedDateTime)
			    .Returns(fixture.Create<DateTime>());
		    mediaPersonalityMock.Setup(m => m.ModifiedByIdentifier)
			    .Returns(fixture.Create<string>());
		    mediaPersonalityMock.Setup(m => m.ToString())
			    .Returns(toString ?? fixture.Create<string>());
		    mediaPersonalityMock.Setup(m => m.Equals(It.IsAny<object>()))
			    .Returns(equals ?? fixture.Create<bool>());
		    mediaPersonalityMock.Setup(m => m.GetHashCode())
			    .Returns(hashCode ?? fixture.Create<int>());
		    return mediaPersonalityMock;
	    }

		public static Mock<IMediaBinding> BuildMediaBindingMock(this Fixture fixture, IMedia media = null, MediaRole? role = null, IMediaPersonality mediaPersonality = null, bool? deletable = false, string toString = null, bool? equals = null, int? hashCode = null)
		{
			NullGuard.NotNull(fixture, nameof(fixture));

			Mock<IMediaBinding> mediaBindingMock = new Mock<IMediaBinding>();
			mediaBindingMock.Setup(m => m.Media)
				.Returns(media ?? fixture.BuildMediaMock().Object);
			mediaBindingMock.Setup(m => m.Role)
				.Returns(role ?? fixture.Create<MediaRole>());
			mediaBindingMock.Setup(m => m.MediaPersonality)
				.Returns(mediaPersonality ?? fixture.BuildMediaPersonalityMock().Object);
			mediaBindingMock.Setup(m => m.Deletable)
				.Returns(deletable ?? fixture.Create<bool>());
			mediaBindingMock.Setup(m => m.CreatedDateTime)
				.Returns(fixture.Create<DateTime>());
			mediaBindingMock.Setup(m => m.CreatedByIdentifier)
				.Returns(fixture.Create<string>());
			mediaBindingMock.Setup(m => m.ModifiedDateTime)
				.Returns(fixture.Create<DateTime>());
			mediaBindingMock.Setup(m => m.ModifiedByIdentifier)
				.Returns(fixture.Create<string>());
			mediaBindingMock.Setup(m => m.ToString())
				.Returns(toString ?? fixture.Create<string>());
			mediaBindingMock.Setup(m => m.Equals(It.IsAny<object>()))
				.Returns(equals ?? fixture.Create<bool>());
			mediaBindingMock.Setup(m => m.GetHashCode())
				.Returns(hashCode ?? fixture.Create<int>());
			return mediaBindingMock;
		}

		public static Mock<IMovieGenre> BuildMovieGenreMock(this Fixture fixture, int? number = null, string name = null, bool? deletable = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return fixture.BuildGenericCategoryMock<IMovieGenre>(number, name, deletable);
        }

        public static Mock<IMusicGenre> BuildMusicGenreMock(this Fixture fixture, int? number = null, string name = null, bool? deletable = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return fixture.BuildGenericCategoryMock<IMusicGenre>(number, name, deletable);
        }

        public static Mock<IBookGenre> BuildBookGenreMock(this Fixture fixture, int? number = null, string name = null, bool? deletable = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return fixture.BuildGenericCategoryMock<IBookGenre>(number, name, deletable);
        }

        public static Mock<IMediaType> BuildMediaTypeMock(this Fixture fixture, int? number = null, string name = null, bool? deletable = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return fixture.BuildGenericCategoryMock<IMediaType>(number, name, deletable);
        }

		private static Mock<TMedia> BuildMediaMock<TMedia>(this Fixture fixture, Random random, Guid? mediaIdentifier = null, string title = null, string subtitle = null, string description = null, string details = null, IMediaType mediaType = null, short? published = null, Uri url = null, byte[] image = null, bool? deletable = false, string toString = null, bool? equals = null, int? hashCode = null, IEnumerable<IMediaBinding> mediaBindings = null) where TMedia : class, IMedia
		{
			NullGuard.NotNull(fixture, nameof(fixture))
				.NotNull(random, nameof(random));

			Mock<TMedia> mediaMock = new Mock<TMedia>();
			mediaMock.Setup(m => m.MediaIdentifier)
				.Returns(mediaIdentifier ?? Guid.NewGuid());
			mediaMock.Setup(m => m.Title)
				.Returns(title ?? fixture.Create<string>());
			mediaMock.Setup(m => m.Subtitle)
				.Returns(subtitle ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
			mediaMock.Setup(m => m.Description)
				.Returns(description ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
			mediaMock.Setup(m => m.Details)
				.Returns(details ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
			mediaMock.Setup(m => m.MediaType)
				.Returns(mediaType ?? fixture.BuildMediaTypeMock().Object);
			mediaMock.Setup(m => m.Published)
				.Returns(published ?? (random.Next(100) > 50 ? fixture.Create<short>() : (short?)null));
			mediaMock.Setup(m => m.Url)
				.Returns(url ?? (random.Next(100) > 50 ? new Uri($"https://localhost/api/mediapersonalities/{Guid.NewGuid():D}") : null));
			mediaMock.Setup(m => m.Image)
				.Returns(image ?? (random.Next(100) > 50 ? fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray() : Array.Empty<byte>()));
			mediaMock.Setup(m => m.Deletable)
				.Returns(deletable ?? fixture.Create<bool>());
			mediaMock.Setup(m => m.CreatedDateTime)
				.Returns(fixture.Create<DateTime>());
			mediaMock.Setup(m => m.CreatedByIdentifier)
				.Returns(fixture.Create<string>());
			mediaMock.Setup(m => m.ModifiedDateTime)
				.Returns(fixture.Create<DateTime>());
			mediaMock.Setup(m => m.ModifiedByIdentifier)
				.Returns(fixture.Create<string>());
			mediaMock.Setup(m => m.ToString())
				.Returns(toString ?? fixture.Create<string>());
			mediaMock.Setup(m => m.Equals(It.IsAny<object>()))
				.Returns(equals ?? fixture.Create<bool>());
			mediaMock.Setup(m => m.GetHashCode())
				.Returns(hashCode ?? fixture.Create<int>());
			mediaMock.Setup(m => m.GetMediaBindings())
				.Returns(mediaBindings ?? new[] { fixture.BuildMediaBindingMock(media: mediaMock.Object).Object, fixture.BuildMediaBindingMock(media: mediaMock.Object).Object, fixture.BuildMediaBindingMock(media: mediaMock.Object).Object });
			return mediaMock;
		}
	}
}