using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using System.Threading.Tasks;
using System;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class BudgetAccountGroup : AccountGroupBase, IBudgetAccountGroup
    {
        #region Constructor

        public BudgetAccountGroup(int number, string name, bool deletable = false)
            : base(number, name, deletable)
        {
        }

        #endregion

        #region Methods

        public virtual Task<IBudgetAccountGroupStatus> CalculateAsync(DateTime statusDate, IBudgetAccountCollection budgetAccountCollection)
        {
            NullGuard.NotNull(budgetAccountCollection, nameof(budgetAccountCollection));

            return new BudgetAccountGroupStatus(this, budgetAccountCollection).CalculateAsync(statusDate.Date);
        }

        #endregion
    }
}