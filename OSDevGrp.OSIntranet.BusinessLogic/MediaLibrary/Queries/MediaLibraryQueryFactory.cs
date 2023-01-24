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