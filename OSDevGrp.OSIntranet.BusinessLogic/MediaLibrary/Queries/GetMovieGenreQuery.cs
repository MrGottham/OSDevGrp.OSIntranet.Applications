using OSDevGrp.OSIntranet.BusinessLogic.Core.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
    internal class GetMovieGenreQuery : GenericCategoryIdentificationQueryBase, IGetMovieGenreQuery
    {
        #region Constructor

        public GetMovieGenreQuery(int number) 
            : base(number)
        {
        }

        #endregion
    }
}