using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
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

            return validator.ValidateAccountGroupIdentifier(Number, GetType(), nameof(Number));
        }

        protected Task<IAccountGroup> GetAccountGroup(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.Run(() => Number.GetAccountGroup(accountingRepository, ref _accountGroup));
        }

        protected Task<IBudgetAccountGroup> GetBudgetAccountGroup(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.Run(() => Number.GetBudgetAccountGroup(accountingRepository, ref _budgetAccountGroup));
        }

        #endregion
    }
}