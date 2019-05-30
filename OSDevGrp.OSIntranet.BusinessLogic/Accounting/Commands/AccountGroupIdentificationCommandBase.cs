using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public abstract class AccountGroupIdentificationCommandBase : IAccountGroupIdentificationCommand
    {
        #region Private variables

        private IAccountGroup _accountGroup;
        private IBudgetAccountGroup _budgetAccountGroup;

        #endregion

        #region Properties

        public int Number { get; set; }

        #endregion

        #region Methods

        public virtual IValidator Validate(IValidator validator, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return validator.Integer.ShouldBeBetween(Number, 1, 99, GetType(), nameof(Number));
        }

        protected Task<IAccountGroup> GetAccountGroup(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.Run(async () =>  _accountGroup ?? (_accountGroup = await accountingRepository.GetAccountGroupAsync(Number)));
        }

        protected Task<IBudgetAccountGroup> GetBudgetAccountGroup(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.Run(async () =>  _budgetAccountGroup ?? (_budgetAccountGroup = await accountingRepository.GetBudgetAccountGroupAsync(Number)));
        }

        #endregion
    }
}