using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    internal class AccountCollectionValues : IAccountCollectionValues
    {
        #region Constructor

        public AccountCollectionValues(decimal assets, decimal liabilities)
        {
            Assets = assets;
            Liabilities = liabilities;
        }

        #endregion

        #region Properties

        public decimal Assets { get; }

        public decimal Liabilities { get; }

        #endregion
    }
}