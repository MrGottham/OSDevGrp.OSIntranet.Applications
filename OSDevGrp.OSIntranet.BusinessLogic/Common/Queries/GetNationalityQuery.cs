using OSDevGrp.OSIntranet.BusinessLogic.Core.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Queries
{
    internal class GetNationalityQuery : GenericCategoryIdentificationQueryBase, IGetNationalityQuery
    {
        #region Constructor

        public GetNationalityQuery(int number) 
            : base(number)
        {
        }

        #endregion
    }
}