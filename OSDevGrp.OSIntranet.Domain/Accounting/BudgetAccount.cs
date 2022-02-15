using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class BudgetAccount : AccountBase<IBudgetAccount>, IBudgetAccount
    {
        #region Private variables

        private IBudgetAccountGroup _budgetAccountGroup;

        #endregion

        #region Constructors

        public BudgetAccount(IAccounting accounting, string accountNumber, string accountName, IBudgetAccountGroup budgetAccountGroup)
            : this(accounting, accountNumber, accountName, budgetAccountGroup, new BudgetInfoCollection(), new PostingLineCollection())
        {
        }

        public BudgetAccount(IAccounting accounting, string accountNumber, string accountName, IBudgetAccountGroup budgetAccountGroup, IBudgetInfoCollection budgetInfoCollection, IPostingLineCollection postingLineCollection)
            : base(accounting, accountNumber, accountName, postingLineCollection)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup))
                .NotNull(budgetInfoCollection, nameof(budgetInfoCollection));

            BudgetAccountGroup = budgetAccountGroup;
            BudgetInfoCollection = budgetInfoCollection;
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

        public IBudgetInfoValues ValuesForMonthOfStatusDate => BudgetInfoCollection.ValuesForMonthOfStatusDate;

        public IBudgetInfoValues ValuesForLastMonthOfStatusDate => BudgetInfoCollection.ValuesForLastMonthOfStatusDate;

        public IBudgetInfoValues ValuesForYearToDateOfStatusDate => BudgetInfoCollection.ValuesForYearToDateOfStatusDate;

        public IBudgetInfoValues ValuesForLastYearOfStatusDate => BudgetInfoCollection.ValuesForLastYearOfStatusDate;

        public IBudgetInfoCollection BudgetInfoCollection { get; private set; }

        #endregion

        #region Methods

        protected override Task[] GetCalculationTasks(DateTime statusDate)
        {
            return new[]
            {
                CalculateBudgetInfoCollectionAsync(statusDate),
                CalculatePostingLineCollectionAsync(statusDate)
            };
        }

        protected override async Task<IBudgetAccount> GetCalculationResultAsync()
        {
            PostingLineCollection = await PostingLineCollection.ApplyCalculationAsync(this);

            return this;
        }

        protected override IBudgetAccount AlreadyCalculated() => this;

        private async Task CalculateBudgetInfoCollectionAsync(DateTime statusDate)
        {
            BudgetInfoCollection = await BudgetInfoCollection.CalculateAsync(statusDate);
        }

        #endregion
    }
}