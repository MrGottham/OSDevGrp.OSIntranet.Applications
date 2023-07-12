using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.Core.Queries;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	public static class MediaLibraryQueryFactory
    {
        #region Methods

        public static EmptyQuery BuildEmptyQuery()
        {
            return new EmptyQuery();
        }

        public static IGetMediaCollectionQuery BuildGetMediaCollectionQuery(string filter)
        {
	        return new GetMediaCollectionQuery(filter);
        }

        public static IGetMovieCollectionQuery BuildGetMovieCollectionQuery(string filter)
        {
	        return new GetMovieCollectionQuery(filter);
        }

        public static IGetMovieQuery BuildGetMovieQuery(Guid mediaIdentifier)
        {
	        return new GetMovieQuery(mediaIdentifier);
        }

        public static IGetMusicCollectionQuery BuildGetMusicCollectionQuery(string filter)
        {
	        return new GetMusicCollectionQuery(filter);
        }

        public static IGetMusicQuery BuildGetMusicQuery(Guid mediaIdentifier)
        {
	        return new GetMusicQuery(mediaIdentifier);
        }

        public static IGetBookCollectionQuery BuildGetBookCollectionQuery(string filter)
        {
	        return new GetBookCollectionQuery(filter);
        }

        public static IGetBookQuery BuildGetBookQuery(Guid mediaIdentifier)
        {
	        return new GetBookQuery(mediaIdentifier);
        }

        public static IGetMediaPersonalityCollectionQuery BuildGetMediaPersonalityCollectionQuery(string filter)
        {
	        return new GetMediaPersonalityCollectionQuery(filter);
        }

        public static IGetMediaPersonalityQuery BuildGetMediaPersonalityQuery(Guid mediaPersonalityIdentifier)
        {
	        return new GetMediaPersonalityQuery(mediaPersonalityIdentifier);
        }

        public static IGetBorrowerCollectionQuery BuildGetBorrowerCollectionQuery(string filter)
        {
	        return new GetBorrowerCollectionQuery(filter);
        }

        public static IGetBorrowerQuery BuildGetBorrowerQuery(Guid borrowerIdentifier)
        {
	        return new GetBorrowerQuery(borrowerIdentifier);
        }

        public static IGetLendingCollectionQuery BuildGetLendingCollectionQuery(bool includeReturned)
        {
	        return new GetLendingCollectionQuery(includeReturned);
        }

        public static IGetLendingQuery BuildGetLendingQuery(Guid lendingIdentifier)
        {
	        return new GetLendingQuery(lendingIdentifier);
        }

        public static IGetMovieGenreQuery BuildGetMovieGenreQuery(int number)
        {
            return new GetMovieGenreQuery(number);
        }

        public static IGetMusicGenreQuery BuildGetMusicGenreQuery(int number)
        {
            return new GetMusicGenreQuery(number);
        }

        public static IGetBookGenreQuery BuildGetBookGenreQuery(int number)
        {
            return new GetBookGenreQuery(number);
        }

        public static IGetMediaTypeQuery BuildGetMediaTypeQuery(int number)
        {
            return new GetMediaTypeQuery(number);
        }

        #endregion
    }
}