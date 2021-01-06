using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    internal class CreditInfoValues : ICreditInfoValues
    {
        #region Constructor

        public CreditInfoValues(decimal credit, decimal balance)
        {
            Credit = credit;
            Balance = balance;
        }

        #endregion

        #region Properties

        public decimal Credit { get; }

        public decimal Balance { get; }

        public decimal Available => Credit + Balance;

        #endregion
    }
}