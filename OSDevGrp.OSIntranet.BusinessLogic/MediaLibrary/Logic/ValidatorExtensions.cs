using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic
{
	internal static class ValidatorExtensions
	{
		#region Private variables

		private static readonly Regex InternationalStandardBookNumberRegex = new(@"^(?:ISBN(?:-1[03])?:?\s)?(?=[0-9X]{10}$|(?=(?:[0-9]+[-\s]){3})[-\s0-9X]{13}$|97[89][0-9]{10}$|(?=(?:[0-9]+[-\s]){4})[-\s0-9]{17}$)(?:97[89][-\s]?)?[0-9]{1,5}[-\s]?[0-9]+[-\s]?[0-9]+[-\s]?[0-9X]$", RegexOptions.Compiled);

		#endregion

		#region Methods

		internal static IValidator ValidateTitle(this IValidator validator, string value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
				.String.ShouldHaveMinLength(value, 1, validatingType, validatingField)
				.String.ShouldHaveMaxLength(value, 256, validatingType, validatingField);
		}

		internal static IValidator ValidateSubtitle(this IValidator validator, string value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, true)
				.String.ShouldHaveMaxLength(value, 256, validatingType, validatingField, true);
		}

		internal static IValidator ValidateDescription(this IValidator validator, string value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, true)
				.String.ShouldHaveMaxLength(value, 512, validatingType, validatingField, true);
		}

		internal static IValidator ValidateDetails(this IValidator validator, string value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, true)
				.String.ShouldHaveMaxLength(value, 32768, validatingType, validatingField, true);
		}

		internal static IValidator ValidateMovieGenreIdentifier(this IValidator validator, int value, IMediaLibraryRepository mediaLibraryRepository, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.Integer.ShouldBeBetween(value, 1, 99, validatingType, validatingField)
				.Object.ShouldBeKnownValue(value, movieGenreIdentifier => Task.FromResult(mediaLibraryRepository.GetMovieGenreAsync(movieGenreIdentifier).GetAwaiter().GetResult() != null), validatingType, validatingField);
		}

		internal static IValidator ValidateMusicGenreIdentifier(this IValidator validator, int value, IMediaLibraryRepository mediaLibraryRepository, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.Integer.ShouldBeBetween(value, 1, 99, validatingType, validatingField)
				.Object.ShouldBeKnownValue(value, musicGenreIdentifier => Task.FromResult(mediaLibraryRepository.GetMusicGenreAsync(musicGenreIdentifier).GetAwaiter().GetResult() != null), validatingType, validatingField);
		}

		internal static IValidator ValidateBookGenreIdentifier(this IValidator validator, int value, IMediaLibraryRepository mediaLibraryRepository, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.Integer.ShouldBeBetween(value, 1, 99, validatingType, validatingField)
				.Object.ShouldBeKnownValue(value, bookGenreIdentifier => Task.FromResult(mediaLibraryRepository.GetBookGenreAsync(bookGenreIdentifier).GetAwaiter().GetResult() != null), validatingType, validatingField);
		}

		internal static IValidator ValidateMediaTypeIdentifier(this IValidator validator, int value, IMediaLibraryRepository mediaLibraryRepository, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.Integer.ShouldBeBetween(value, 1, 99, validatingType, validatingField)
				.Object.ShouldBeKnownValue(value, mediaTypeIdentifier => Task.FromResult(mediaLibraryRepository.GetMediaTypeAsync(mediaTypeIdentifier).GetAwaiter().GetResult() != null), validatingType, validatingField);
		}

		internal static IValidator ValidateNationalityIdentifier(this IValidator validator, int? value, ICommonRepository commonRepository, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(commonRepository, nameof(commonRepository))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			if (value.HasValue == false)
			{
				return validator;
			}

			return validator.Integer.ShouldBeBetween(value.Value, 1, 99, validatingType, validatingField)
				.Object.ShouldBeKnownValue(value.Value, nationalityIdentifier => Task.FromResult(commonRepository.GetNationalityAsync(nationalityIdentifier).GetAwaiter().GetResult() != null), validatingType, validatingField);
		}

		internal static IValidator ValidateLanguageIdentifier(this IValidator validator, int? value, ICommonRepository commonRepository, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(commonRepository, nameof(commonRepository))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			if (value.HasValue == false)
			{
				return validator;
			}

			return validator.Integer.ShouldBeBetween(value.Value, 1, 99, validatingType, validatingField)
				.Object.ShouldBeKnownValue(value.Value, languageIdentifier => Task.FromResult(commonRepository.GetLanguageAsync(languageIdentifier).GetAwaiter().GetResult() != null), validatingType, validatingField);
		}

		internal static IValidator ValidatePublished(this IValidator validator, short? value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return value.HasValue
				? validator.Integer.ShouldBeBetween(value.Value, 1000, 9999, validatingType, validatingField)
				: validator;
		}

		internal static IValidator ValidateLength(this IValidator validator, short? value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return value.HasValue
				? validator.Integer.ShouldBeBetween(value.Value, 1, 999, validatingType, validatingField)
				: validator;
		}

		internal static IValidator ValidateTracks(this IValidator validator, short? value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return value.HasValue
				? validator.Integer.ShouldBeBetween(value.Value, 1, 999, validatingType, validatingField)
				: validator;
		}

		internal static IValidator ValidateInternationalStandardBookNumber(this IValidator validator, string value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, true)
				.String.ShouldHaveMaxLength(value, 32, validatingType, validatingField, true)
				.String.ShouldMatchPattern(value, InternationalStandardBookNumberRegex, validatingType, validatingField, true);
		}

		internal static IValidator ValidateUrl(this IValidator validator, string value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return UrlHelper.ValidateUrl(validator, value, validatingType, validatingField);
		}

		internal static IValidator ValidateImage(this IValidator validator, byte[] value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.Enumerable.ShouldHaveMinItems(value, 0, validatingType, validatingField, true)
				.Enumerable.ShouldHaveMaxItems(value, 32768, validatingType, validatingField, true);
		}

		internal static IValidator ValidateMediaPersonalityIdentifierCollection(this IValidator validator, IEnumerable<Guid> value, IMediaLibraryRepository mediaLibraryRepository, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			Guid[] mediaPersonalityIdentifiers = value?.ToArray();

			validator.Object.ShouldNotBeNull(mediaPersonalityIdentifiers, validatingType, validatingField);
			foreach (Guid mediaPersonalityIdentifier in mediaPersonalityIdentifiers ?? Array.Empty<Guid>())
			{
				validator.Object.ShouldBeKnownValue(mediaPersonalityIdentifier, mediaLibraryRepository.MediaPersonalityExistsAsync, validatingType, validatingField);
			}

			return validator;
		}

		internal static IValidator ValidateGivenName(this IValidator validator, string value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, true)
				.String.ShouldHaveMaxLength(value, 32, validatingType, validatingField, true);
		}

		internal static IValidator ValidateMiddleName(this IValidator validator, string value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, true)
				.String.ShouldHaveMaxLength(value, 32, validatingType, validatingField, true);
		}

		internal static IValidator ValidateSurname(this IValidator validator, string value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
				.String.ShouldHaveMinLength(value, 1, validatingType, validatingField)
				.String.ShouldHaveMaxLength(value, 32, validatingType, validatingField);
		}

		internal static IValidator ValidateBirthdate(this IValidator validator, DateTime? value, DateTime maxValue, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			if (value.HasValue == false)
			{
				return validator;
			}

			return validator.DateTime.ShouldBePastDateOrToday(value.Value, validatingType, validatingField)
				.DateTime.ShouldBeEarlierThanOffsetDate(value.Value, maxValue, validatingType, validatingField);
		}

		internal static IValidator ValidateDateOfDead(this IValidator validator, DateTime? value, DateTime minValue, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			if (value.HasValue == false)
			{
				return validator;
			}

			return validator.DateTime.ShouldBePastDateOrToday(value.Value, validatingType, validatingField)
				.DateTime.ShouldBeLaterThanOffsetDate(value.Value, minValue, validatingType, validatingField);
		}

		internal static IValidator ValidateFullName(this IValidator validator, string value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
				.String.ShouldHaveMinLength(value, 1, validatingType, validatingField)
				.String.ShouldHaveMaxLength(value, 256, validatingType, validatingField);
		}

		internal static IValidator ValidateMailAddress(this IValidator validator, string value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.ValidateMailAddress(value, validatingType, validatingField, true);
		}

		internal static IValidator ValidatePhoneNumber(this IValidator validator, string value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.ValidatePhoneNumber(value, validatingType, validatingField, true);
		}

		internal static IValidator ValidateLendingLimit(this IValidator validator, int value, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.Integer.ShouldBeBetween(value, 1, 365, validatingType, validatingField);
		}

		internal static IValidator ValidateBorrowerIdentifier(this IValidator validator, Guid value, IMediaLibraryRepository mediaLibraryRepository, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.Object.ShouldBeKnownValue(value, mediaLibraryRepository.BorrowerExistsAsync, validatingType, validatingField);
		}

		internal static IValidator ValidateMediaIdentifier(this IValidator validator, Guid value, IMediaLibraryRepository mediaLibraryRepository, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.Object.ShouldBeKnownValue(value, mediaIdentifier => mediaIdentifier.IsExistingMediaIdentifierAsync(mediaLibraryRepository), validatingType, validatingField);
		}

		internal static IValidator ValidateLendingDate(this IValidator validator, DateTime value, DateTime recallDate, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.DateTime.ShouldBePastDateOrToday(value, validatingType, validatingField)
				.DateTime.ShouldBeEarlierThanOffsetDate(value, recallDate, validatingType, validatingField);
		}

		internal static IValidator ValidateRecallDate(this IValidator validator, DateTime value, DateTime lendingDate, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			return validator.DateTime.ShouldBeLaterThanOffsetDate(value, lendingDate, validatingType, validatingField);
		}

		internal static IValidator ValidateReturnedDate(this IValidator validator, DateTime? value, DateTime lendingDate, Type validatingType, string validatingField)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(validatingType, nameof(validatingType))
				.NotNullOrWhiteSpace(validatingField, nameof(validatingField));

			if (value.HasValue == false)
			{
				return validator;
			}

			return validator.DateTime.ShouldBePastDateOrToday(value.Value, validatingType, validatingField)
				.DateTime.ShouldBeLaterThanOrEqualToOffsetDate(value.Value, lendingDate, validatingType, validatingField);
		}

		internal static IValidator ValidateMediaData<TMedia>(this IValidator validator, IMediaDataCommand<TMedia> mediaData, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) where TMedia : IMedia
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(mediaData, nameof(mediaData))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return validator.ValidateTitle(mediaData.Title, mediaData.GetType(), nameof(mediaData.Title))
				.ValidateSubtitle(mediaData.Subtitle, mediaData.GetType(), nameof(mediaData.Subtitle))
				.ValidateDescription(mediaData.Description, mediaData.GetType(), nameof(mediaData.Description))
				.ValidateDetails(mediaData.Details, mediaData.GetType(), nameof(mediaData.Details))
				.ValidateMediaTypeIdentifier(mediaData.MediaTypeIdentifier, mediaLibraryRepository, mediaData.GetType(), nameof(mediaData.MediaTypeIdentifier))
				.ValidatePublished(mediaData.Published, mediaData.GetType(), nameof(mediaData.Published))
				.ValidateUrl(mediaData.Url, mediaData.GetType(), nameof(mediaData.Url))
				.ValidateImage(mediaData.Image, mediaData.GetType(), nameof(mediaData.Image));
		}

		internal static IValidator ValidateMovieData(this IValidator validator, IMovieDataCommand movieData, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(movieData, nameof(movieData))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return validator.ValidateMediaData(movieData, mediaLibraryRepository, commonRepository)
				.ValidateMovieGenreIdentifier(movieData.MovieGenreIdentifier, mediaLibraryRepository, movieData.GetType(), nameof(movieData.MovieGenreIdentifier))
				.ValidateLanguageIdentifier(movieData.SpokenLanguageIdentifier, commonRepository, movieData.GetType(), nameof(movieData.SpokenLanguageIdentifier))
				.ValidateLength(movieData.Length, movieData.GetType(), nameof(movieData.Length))
				.ValidateMediaPersonalityIdentifierCollection(movieData.Directors, mediaLibraryRepository, movieData.GetType(), nameof(movieData.Directors))
				.ValidateMediaPersonalityIdentifierCollection(movieData.Actors, mediaLibraryRepository, movieData.GetType(), nameof(movieData.Actors));
		}

		internal static IValidator ValidateMusicData(this IValidator validator, IMusicDataCommand musicData, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(musicData, nameof(musicData))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return validator.ValidateMediaData(musicData, mediaLibraryRepository, commonRepository)
				.ValidateMusicGenreIdentifier(musicData.MusicGenreIdentifier, mediaLibraryRepository, musicData.GetType(), nameof(musicData.MusicGenreIdentifier))
				.ValidateTracks(musicData.Tracks, musicData.GetType(), nameof(musicData.Tracks))
				.ValidateMediaPersonalityIdentifierCollection(musicData.Artists, mediaLibraryRepository, musicData.GetType(), nameof(musicData.Artists));
		}

		internal static IValidator ValidateBookData(this IValidator validator, IBookDataCommand bookData, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(bookData, nameof(bookData))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return validator.ValidateMediaData(bookData, mediaLibraryRepository, commonRepository)
				.ValidateBookGenreIdentifier(bookData.BookGenreIdentifier, mediaLibraryRepository, bookData.GetType(), nameof(bookData.BookGenreIdentifier))
				.ValidateLanguageIdentifier(bookData.WrittenLanguageIdentifier, commonRepository, bookData.GetType(), nameof(bookData.WrittenLanguageIdentifier))
				.ValidateInternationalStandardBookNumber(bookData.InternationalStandardBookNumber, bookData.GetType(), nameof(bookData.InternationalStandardBookNumber))
				.ValidateMediaPersonalityIdentifierCollection(bookData.Authors, mediaLibraryRepository, bookData.GetType(), nameof(bookData.Authors));
		}

		internal static IValidator ValidateMediaPersonalityData(this IValidator validator, IMediaPersonalityDataCommand mediaPersonalityData, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(mediaPersonalityData, nameof(mediaPersonalityData))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			DateTime? birthDate = mediaPersonalityData.BirthDate;
			DateTime? dateOfDead = mediaPersonalityData.DateOfDead;

			return validator.ValidateGivenName(mediaPersonalityData.GivenName, mediaPersonalityData.GetType(), nameof(mediaPersonalityData.GivenName))
				.ValidateMiddleName(mediaPersonalityData.MiddleName, mediaPersonalityData.GetType(), nameof(mediaPersonalityData.MiddleName))
				.ValidateSurname(mediaPersonalityData.Surname, mediaPersonalityData.GetType(), nameof(mediaPersonalityData.Surname))
				.ValidateNationalityIdentifier(mediaPersonalityData.NationalityIdentifier, commonRepository, mediaPersonalityData.GetType(), nameof(mediaPersonalityData.NationalityIdentifier))
				.ValidateBirthdate(birthDate, dateOfDead ?? DateTime.Today, mediaPersonalityData.GetType(), nameof(mediaPersonalityData.BirthDate))
				.ValidateDateOfDead(dateOfDead, birthDate ?? DateTime.MinValue.ToUniversalTime(), mediaPersonalityData.GetType(), nameof(mediaPersonalityData.DateOfDead))
				.ValidateUrl(mediaPersonalityData.Url, mediaPersonalityData.GetType(), nameof(mediaPersonalityData.Url))
				.ValidateImage(mediaPersonalityData.Image, mediaPersonalityData.GetType(), nameof(mediaPersonalityData.Image));
		}

		internal static IValidator ValidateBorrowerData(this IValidator validator, IBorrowerDataCommand borrowerData, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(borrowerData, nameof(borrowerData))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return validator.ValidateFullName(borrowerData.FullName, borrowerData.GetType(), nameof(borrowerData.FullName))
				.ValidateMailAddress(borrowerData.MailAddress, borrowerData.GetType(), nameof(borrowerData.MailAddress))
				.ValidatePhoneNumber(borrowerData.PrimaryPhone, borrowerData.GetType(), nameof(borrowerData.PrimaryPhone))
				.ValidatePhoneNumber(borrowerData.SecondaryPhone, borrowerData.GetType(), nameof(borrowerData.SecondaryPhone))
				.ValidateLendingLimit(borrowerData.LendingLimit, borrowerData.GetType(), nameof(borrowerData.LendingLimit));
		}

		internal static IValidator ValidateLendingData(this IValidator validator, ILendingDataCommand lendingData, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(lendingData, nameof(lendingData))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			DateTime lendingDate = lendingData.LendingDate;
			DateTime recallDate = lendingData.RecallDate;

			return validator.ValidateBorrowerIdentifier(lendingData.BorrowerIdentifier, mediaLibraryRepository, lendingData.GetType(), nameof(lendingData.BorrowerIdentifier))
				.ValidateMediaIdentifier(lendingData.MediaIdentifier, mediaLibraryRepository, lendingData.GetType(), nameof(lendingData.MediaIdentifier))
				.ValidateLendingDate(lendingDate, recallDate, lendingData.GetType(), nameof(lendingData.LendingDate))
				.ValidateRecallDate(recallDate, lendingDate, lendingData.GetType(), nameof(lendingData.RecallDate))
				.ValidateReturnedDate(lendingData.ReturnedDate, lendingDate, lendingData.GetType(), nameof(lendingData.ReturnedDate));
		}

		#endregion
	}
}