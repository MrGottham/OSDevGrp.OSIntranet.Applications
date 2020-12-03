using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    internal class ContactAccountCollectionValues : IContactAccountCollectionValues
    {
        #region Constructor

        public ContactAccountCollectionValues(decimal debtors, decimal creditors)
        {
            Debtors = debtors;
            Creditors = creditors;
        }

        #endregion

        #region Properties

        public decimal Debtors { get; }

        public decimal Creditors { get; }

        #endregion
    }
}