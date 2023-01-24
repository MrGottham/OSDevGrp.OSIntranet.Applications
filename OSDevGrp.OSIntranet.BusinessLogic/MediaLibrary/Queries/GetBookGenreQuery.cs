using OSDevGrp.OSIntranet.BusinessLogic.Core.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
    internal class GetBookGenreQuery : GenericCategoryIdentificationQueryBase, IGetBookGenreQuery
    {
        #region Constructor

        public GetBookGenreQuery(int number) 
            : base(number)
        {
        }

        #endregion
    }
}