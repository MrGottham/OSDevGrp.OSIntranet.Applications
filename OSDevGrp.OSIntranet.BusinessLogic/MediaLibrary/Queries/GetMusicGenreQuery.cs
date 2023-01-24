using OSDevGrp.OSIntranet.BusinessLogic.Core.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
    internal class GetMusicGenreQuery : GenericCategoryIdentificationQueryBase, IGetMusicGenreQuery
    {
        #region Constructor

        public GetMusicGenreQuery(int number) 
            : base(number)
        {
        }

        #endregion
    }
}