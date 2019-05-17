using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries
{
    public abstract class AccountGroupIdentificationQueryBase : IAccountGroupIdentificationQuery
    {
        #region Properties

        public int Number { get; set; }

        #endregion
    }
}