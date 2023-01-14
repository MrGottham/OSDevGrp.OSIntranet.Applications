using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

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

        public virtual IValidator Validate(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return validator.Permission.HasNecessaryPermission(claimResolver.IsAccountingAdministrator())
                .ValidateAccountGroupIdentifier(Number, GetType(), nameof(Number));
        }

        protected Task<IAccountGroup> GetAccountGroupAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.FromResult(Number.GetAccountGroup(accountingRepository, ref _accountGroup));
        }

        protected Task<IBudgetAccountGroup> GetBudgetAccountGroupAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.FromResult(Number.GetBudgetAccountGroup(accountingRepository, ref _budgetAccountGroup));
        }

        #endregion
    }
}