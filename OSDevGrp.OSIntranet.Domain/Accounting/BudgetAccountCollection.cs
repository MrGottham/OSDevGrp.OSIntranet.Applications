using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class BudgetAccountCollection : AccountCollectionBase<IBudgetAccount, IBudgetAccountCollection>, IBudgetAccountCollection
    {
        #region Methods

        protected override IBudgetAccountCollection Calculate(DateTime statusDate)
        {
            return this;
        }

        #endregion
    } 
}