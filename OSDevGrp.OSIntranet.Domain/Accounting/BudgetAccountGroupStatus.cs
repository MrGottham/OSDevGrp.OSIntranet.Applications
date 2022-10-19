using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    internal class BudgetAccountGroupStatus : BudgetAccountGroup, IBudgetAccountGroupStatus
    {
        #region Private variables

        private bool _isCalculating;

        #endregion

        #region Constructor

        public BudgetAccountGroupStatus(IBudgetAccountGroup budgetAccountGroup, IBudgetAccountCollection budgetAccountCollection)
            : base(budgetAccountGroup.Number, budgetAccountGroup.Name, budgetAccountGroup.Deletable)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup))
                .NotNull(budgetAccountCollection, nameof(budgetAccountCollection));

            SetAuditInformation(budgetAccountGroup.CreatedDateTime, budgetAccountGroup.CreatedByIdentifier, budgetAccountGroup.ModifiedDateTime, budgetAccountGroup.ModifiedByIdentifier);

            BudgetAccountCollection = budgetAccountCollection;
        }

        #endregion

        #region Properties

        public IBudgetAccountCollection BudgetAccountCollection { get; private set; }

        public IBudgetInfoValues ValuesForMonthOfStatusDate => BudgetAccountCollection.ValuesForMonthOfStatusDate;

        public IBudgetInfoValues ValuesForLastMonthOfStatusDate => BudgetAccountCollection.ValuesForLastMonthOfStatusDate;

        public IBudgetInfoValues ValuesForYearToDateOfStatusDate => BudgetAccountCollection.ValuesForYearToDateOfStatusDate;

        public IBudgetInfoValues ValuesForLastYearOfStatusDate => BudgetAccountCollection.ValuesForLastYearOfStatusDate;

        public DateTime StatusDate { get; private set; }

        #endregion

        #region Methods

        public async Task<IBudgetAccountGroupStatus> CalculateAsync(DateTime statusDate)
        {
            if (statusDate.Date == StatusDate)
            {
                while (_isCalculating)
                {
                    await Task.Delay(250);
                }

                return this;
            }

            StatusDate = statusDate.Date;

            _isCalculating = true;
            try
            {
                IBudgetAccountCollection calculatedBudgetAccountCollection = await BudgetAccountCollection.CalculateAsync(StatusDate);
                if (calculatedBudgetAccountCollection != null)
                {
                    BudgetAccountCollection = calculatedBudgetAccountCollection;
                }

                return this;
            }
            finally
            {
                _isCalculating = false;
            }
        }

        public override Task<IBudgetAccountGroupStatus> CalculateAsync(DateTime statusDate, IBudgetAccountCollection budgetAccountCollection) => throw new NotSupportedException();

        public override void AddAuditInformation(DateTime createdUtcDateTime, string createdByIdentifier, DateTime modifiedUtcDateTime, string modifiedByIdentifier) => throw new NotSupportedException();

        public override void AllowDeletion() => throw new NotSupportedException();

        public override void DisallowDeletion() => throw new NotSupportedException();

        #endregion
    }
}