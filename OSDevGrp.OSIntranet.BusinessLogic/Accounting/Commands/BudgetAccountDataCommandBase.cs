using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public abstract class BudgetAccountDataCommandBase : AccountCoreDataCommandBase<IBudgetAccount>, IBudgetAccountDataCommand
    {
        #region Private variables

        private IBudgetAccountGroup _budgetAccountGroup;

        #endregion

        #region Properties

        public int BudgetAccountGroupNumber { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, accountingRepository, commonRepository)
                .ValidateAccountGroupIdentifier(BudgetAccountGroupNumber, GetType(), nameof(BudgetAccountGroupNumber))
                .Object.ShouldBeKnownValue(BudgetAccountGroupNumber, budgetAccountGroupNumber => Task.Run(async () => await GetBudgetAccountGroupAsync(accountingRepository) != null), GetType(), nameof(BudgetAccountGroupNumber));
        }

        public override IBudgetAccount ToDomain(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            IAccounting accounting = GetAccountingAsync(accountingRepository).GetAwaiter().GetResult();
            IBudgetAccountGroup budgetAccountGroup = GetBudgetAccountGroupAsync(accountingRepository).GetAwaiter().GetResult();

            return new BudgetAccount(accounting, AccountNumber, AccountName, budgetAccountGroup)
            {
                Description = Description,
                Note = Note
            };
        }

        protected Task<IBudgetAccountGroup> GetBudgetAccountGroupAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.FromResult(BudgetAccountGroupNumber.GetBudgetAccountGroup(accountingRepository, ref _budgetAccountGroup));
        }

        #endregion
    }
}