using System;
using System.Linq;
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

        #endregion

        #region Methods

        protected override IBudgetInfo Calculate(DateTime statusDate)
        {
            DateTime calculationFromDate = FromDate;
            DateTime calculationToDate = ResolveCalculationToDate(statusDate);

            Posted = BudgetAccount.PostingLineCollection.AsParallel()
                .Where(postingLine =>
                {
                    DateTime postingDate = postingLine.PostingDate.Date;

                    return postingDate >= calculationFromDate && postingDate <= calculationToDate;
                })
                .Sum(postingLine => postingLine.PostingValue);

            return this;
        }

        protected override IBudgetInfo AlreadyCalculated() => this;

        #endregion
    }
}