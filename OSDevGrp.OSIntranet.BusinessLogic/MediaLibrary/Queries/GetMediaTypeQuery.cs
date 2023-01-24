using OSDevGrp.OSIntranet.BusinessLogic.Core.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
    internal class GetMediaTypeQuery : GenericCategoryIdentificationQueryBase, IGetMediaTypeQuery
    {
        #region Constructor

        public GetMediaTypeQuery(int number) 
            : base(number)
        {
        }

        #endregion
    }
}