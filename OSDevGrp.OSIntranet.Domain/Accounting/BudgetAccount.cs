using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class BudgetAccount : AccountBase<IBudgetAccount>, IBudgetAccount
    {
        #region Private variables

        private IBudgetAccountGroup _budgetAccountGroup;

        #endregion

        #region Constructor

        public BudgetAccount(IAccounting accounting, string accountNumber, string accountName, IBudgetAccountGroup budgetAccountGroup)
            : base(accounting, accountNumber, accountName)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup));

            BudgetAccountGroup = budgetAccountGroup;
        }

        #endregion

        #region Properties

        public IBudgetAccountGroup BudgetAccountGroup
        { 
            get => _budgetAccountGroup;
            set
            {
                NullGuard.NotNull(value, nameof(value));

                _budgetAccountGroup = value;
            } 
        }

        #endregion

        #region Methods

        protected override IBudgetAccount Calculate(DateTime statusDate)
        {
            return this;
        }

        #endregion
    }
}