using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class BudgetInfo : InfoBase<IBudgetInfo>, IBudgetInfo
    {
        #region Private variables

        private decimal _income;
        private decimal _expenses;

        #endregion

        #region Constructor

        public BudgetInfo(IBudgetAccount budgetAccount, short year, short month, decimal income, decimal expenses)
            : base(year, month)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            if (income < 0M)
            {
                throw new ArgumentException("The income value cannot be below 0.", nameof(income));
            }

            if (expenses < 0M)
            {
                throw new ArgumentException("The expenses value cannot be below 0.", nameof(expenses));
            }

            BudgetAccount = budgetAccount;
            Income = income;
            Expenses = expenses;
        }

        #endregion

        #region Properties

        public IBudgetAccount BudgetAccount { get; }

        public decimal Income
        {
            get => _income;
            set
            {
                if (value < 0M)
                {
                    throw new ArgumentException("The income value cannot be below 0.", nameof(value));
                }

                _income = value;
            }
        }

        public decimal Expenses
        {
            get => _expenses;
            set
            {
                if (value < 0M)
                {
                    throw new ArgumentException("The expenses value cannot be below 0.", nameof(value));
                }

                _expenses = value;
            }
        }

        public decimal Budget => Income - Expenses;

        public decimal Posted { get; private set; }

        public decimal Available => CalculateAvailable(Budget, Posted);

        #endregion

        #region Methods

        protected override IBudgetInfo Calculate(DateTime statusDate)
        {
            DateTime calculationFromDate = FromDate;
            DateTime calculationToDate = ResolveCalculationToDate(statusDate);

            Posted = BudgetAccount.PostingLineCollection.CalculatePostingValue(calculationFromDate, calculationToDate);

            return this;
        }

        protected override IBudgetInfo AlreadyCalculated() => this;

        internal static decimal CalculateAvailable(decimal budget, decimal posted)
        {
            if (budget < 0M || posted < 0M)
            {
                return Math.Abs(budget) - Math.Abs(posted);
            }

            return 0M;
        }

        #endregion
    }
}