using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.Core.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Queries
{
    public static class CommonQueryFactory
    {
        #region Methods

        public static EmptyQuery BuildEmptyQuery()
        {
            return new EmptyQuery();
        }

        public static IGetNationalityQuery BuildGetNationalityQuery(int number)
        {
            return new GetNationalityQuery(number);
        }

        public static IGetLanguageQuery BuildGetLanguageQuery(int number)
        {
            return new GetLanguageQuery(number);
        }

        #endregion
    }
}