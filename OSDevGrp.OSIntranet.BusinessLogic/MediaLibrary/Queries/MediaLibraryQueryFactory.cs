using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.Core.Queries;

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

        public static IGetMusicCollectionQuery BuildGetMusicCollectionQuery(string filter)
        {
	        return new GetMusicCollectionQuery(filter);
        }

        public static IGetBookCollectionQuery BuildGetBookCollectionQuery(string filter)
        {
	        return new GetBookCollectionQuery(filter);
        }

        public static IGetMediaPersonalityCollectionQuery BuildGetMediaPersonalityCollectionQuery(string filter)
        {
	        return new GetMediaPersonalityCollectionQuery(filter);
        }

        public static IGetBorrowerCollectionQuery BuildGetBorrowerCollectionQuery(string filter)
        {
	        return new GetBorrowerCollectionQuery(filter);
        }

        public static IGetLendingCollectionQuery BuildGetLendingCollectionQuery(bool includeReturned)
        {
	        return new GetLendingCollectionQuery(includeReturned);
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