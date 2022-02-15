using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class ContactInfoValues : IContactInfoValues
    {
        #region Constructor

        public ContactInfoValues(decimal balance)
        {
            Balance = balance;
        }

        #endregion

        #region Properties

        public decimal Balance { get; }

        #endregion
    }
}