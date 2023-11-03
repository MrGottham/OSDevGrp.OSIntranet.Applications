using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	public static class MediaLibraryCommandFactory
    {
        #region Methods

        public static ICreateMovieCommand BuildCreateMovieCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int movieGenreIdentifier, int? spokenLanguageIdentifier, int mediaTypeIdentifier, short? published, short? length, string url, byte[] image, IEnumerable<Guid> directors, IEnumerable<Guid> actors)
        {
	        return new CreateMovieCommand(mediaIdentifier, title, subtitle, description, details, movieGenreIdentifier, spokenLanguageIdentifier, mediaTypeIdentifier, published, length, url, image, directors, actors);
        }

        public static IUpdateMovieCommand BuildUpdateMovieCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int movieGenreIdentifier, int? spokenLanguageIdentifier, int mediaTypeIdentifier, short? published, short? length, string url, byte[] image, IEnumerable<Guid> directors, IEnumerable<Guid> actors)
        {
	        return new UpdateMovieCommand(mediaIdentifier, title, subtitle, description, details, movieGenreIdentifier, spokenLanguageIdentifier, mediaTypeIdentifier, published, length, url, image, directors, actors);
        }

        public static IDeleteMovieCommand BuildDeleteMovieCommand(Guid mediaIdentifier)
        {
	        return new DeleteMovieCommand(mediaIdentifier);
        }

        public static ICreateMusicCommand BuildCreateMusicCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int musicGenreIdentifier, int mediaTypeIdentifier, short? published, short? tracks, string url, byte[] image, IEnumerable<Guid> artists)
        {
	        return new CreateMusicCommand(mediaIdentifier, title, subtitle, description, details, musicGenreIdentifier, mediaTypeIdentifier, published, tracks, url, image, artists);
        }

        public static IUpdateMusicCommand BuildUpdateMusicCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int musicGenreIdentifier, int mediaTypeIdentifier, short? published, short? tracks, string url, byte[] image, IEnumerable<Guid> artists)
        {
	        return new UpdateMusicCommand(mediaIdentifier, title, subtitle, description, details, musicGenreIdentifier, mediaTypeIdentifier, published, tracks, url, image, artists);
        }

        public static IDeleteMusicCommand BuildDeleteMusicCommand(Guid mediaIdentifier)
        {
	        return new DeleteMusicCommand(mediaIdentifier);
        }

        public static ICreateBookCommand BuildCreateBookCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int bookGenreIdentifier, int? writtenLanguageIdentifier, int mediaTypeIdentifier, string internationalStandardBookNumber, short? published, string url, byte[] image, IEnumerable<Guid> authors)
        {
	        return new CreateBookCommand(mediaIdentifier, title, subtitle, description, details, bookGenreIdentifier, writtenLanguageIdentifier, mediaTypeIdentifier, internationalStandardBookNumber, published, url, image, authors);
        }

        public static IUpdateBookCommand BuildUpdateBookCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int bookGenreIdentifier, int? writtenLanguageIdentifier, int mediaTypeIdentifier, string internationalStandardBookNumber, short? published, string url, byte[] image, IEnumerable<Guid> authors)
        {
	        return new UpdateBookCommand(mediaIdentifier, title, subtitle, description, details, bookGenreIdentifier, writtenLanguageIdentifier, mediaTypeIdentifier, internationalStandardBookNumber, published, url, image, authors);
        }

        public static IDeleteBookCommand BuildDeleteBookCommand(Guid mediaIdentifier)
        {
	        return new DeleteBookCommand(mediaIdentifier);
        }

		public static ICreateMediaPersonalityCommand BuildCreateMediaPersonalityCommand(Guid mediaPersonalityIdentifier, string givenName, string middleName, string surname, int nationalityIdentifier, DateTime? birthDate, DateTime? dateOfDead, string url, byte[] image)
        {
	        return new CreateMediaPersonalityCommand(mediaPersonalityIdentifier, givenName, middleName, surname, nationalityIdentifier, birthDate, dateOfDead, url, image);
        }

		public static IUpdateMediaPersonalityCommand BuildUpdateMediaPersonalityCommand(Guid mediaPersonalityIdentifier, string givenName, string middleName, string surname, int nationalityIdentifier, DateTime? birthDate, DateTime? dateOfDead, string url, byte[] image)
		{
			return new UpdateMediaPersonalityCommand(mediaPersonalityIdentifier, givenName, middleName, surname, nationalityIdentifier, birthDate, dateOfDead, url, image);
		}

		public static IDeleteMediaPersonalityCommand BuildDeleteMediaPersonalityCommand(Guid mediaPersonalityIdentifier)
		{
			return new DeleteMediaPersonalityCommand(mediaPersonalityIdentifier);
		}

		public static ICreateBorrowerCommand BuildCreateBorrowerCommand(Guid borrowerIdentifier, string fullName, string mailAddress, string primaryPhone, string secondaryPhone, int lendingLimit)
        {
	        return new CreateBorrowerCommand(borrowerIdentifier, fullName, mailAddress, primaryPhone, secondaryPhone, lendingLimit);
        }

		public static IUpdateBorrowerCommand BuildUpdateBorrowerCommand(Guid borrowerIdentifier, string fullName, string mailAddress, string primaryPhone, string secondaryPhone, int lendingLimit)
		{
			return new UpdateBorrowerCommand(borrowerIdentifier, fullName, mailAddress, primaryPhone, secondaryPhone, lendingLimit);
		}

		public static IDeleteBorrowerCommand BuildDeleteBorrowerCommand(Guid borrowerIdentifier)
		{
			return new DeleteBorrowerCommand(borrowerIdentifier);
		}

		public static ICreateLendingCommand BuildCreateLendingCommand(Guid lendingIdentifier, Guid borrowerIdentifier, Guid mediaIdentifier, DateTime lendingDate, DateTime recallDate, DateTime? returnedDate)
        {
	        return new CreateLendingCommand(lendingIdentifier, borrowerIdentifier, mediaIdentifier, lendingDate, recallDate, returnedDate);
        }

		public static IUpdateLendingCommand BuildUpdateLendingCommand(Guid lendingIdentifier, Guid borrowerIdentifier, Guid mediaIdentifier, DateTime lendingDate, DateTime recallDate, DateTime? returnedDate)
		{
			return new UpdateLendingCommand(lendingIdentifier, borrowerIdentifier, mediaIdentifier, lendingDate, recallDate, returnedDate);
		}

		public static IDeleteLendingCommand BuildDeleteLendingCommand(Guid lendingIdentifier)
		{
			return new DeleteLendingCommand(lendingIdentifier);
		}

		public static ICreateMovieGenreCommand BuildCreateMovieGenreCommand(int number, string name)
        {
            return new CreateMovieGenreCommand(number, name);
        }

        public static IUpdateMovieGenreCommand BuildUpdateMovieGenreCommand(int number, string name)
        {
            return new UpdateMovieGenreCommand(number, name);
        }

        public static IDeleteMovieGenreCommand BuildDeleteMovieGenreCommand(int number)
        {
            return new DeleteMovieGenreCommand(number);
        }

        public static ICreateMusicGenreCommand BuildCreateMusicGenreCommand(int number, string name)
        {
	        return new CreateMusicGenreCommand(number, name);
        }

        public static IUpdateMusicGenreCommand BuildUpdateMusicGenreCommand(int number, string name)
        {
	        return new UpdateMusicGenreCommand(number, name);
        }

        public static IDeleteMusicGenreCommand BuildDeleteMusicGenreCommand(int number)
        {
	        return new DeleteMusicGenreCommand(number);
        }

        public static ICreateBookGenreCommand BuildCreateBookGenreCommand(int number, string name)
        {
	        return new CreateBookGenreCommand(number, name);
        }

        public static IUpdateBookGenreCommand BuildUpdateBookGenreCommand(int number, string name)
        {
	        return new UpdateBookGenreCommand(number, name);
        }

        public static IDeleteBookGenreCommand BuildDeleteBookGenreCommand(int number)
        {
	        return new DeleteBookGenreCommand(number);
        }

        public static ICreateMediaTypeCommand BuildCreateMediaTypeCommand(int number, string name)
        {
	        return new CreateMediaTypeCommand(number, name);
        }

        public static IUpdateMediaTypeCommand BuildUpdateMediaTypeCommand(int number, string name)
        {
	        return new UpdateMediaTypeCommand(number, name);
        }

        public static IDeleteMediaTypeCommand BuildDeleteMediaTypeCommand(int number)
        {
	        return new DeleteMediaTypeCommand(number);
        }

        #endregion
	}
}