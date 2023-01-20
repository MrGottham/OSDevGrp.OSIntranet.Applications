using OSDevGrp.OSIntranet.BusinessLogic.Core.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Queries
{
    internal class GetLanguageQuery : GenericCategoryIdentificationQueryBase, IGetLanguageQuery
    {
        #region Constructor

        public GetLanguageQuery(int number) 
            : base(number)
        {
        }

        #endregion
    }
}