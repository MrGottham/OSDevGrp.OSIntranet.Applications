using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public abstract class AccountingIdentificationCommandBase : IAccountingIdentificationCommand
    {
        #region Private variables

        private IAccounting _accounting;

        #endregion

        #region Properties

        public int AccountingNumber { get; set; }

        #endregion

        #region Methods

        public virtual IValidator Validate(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
	        NullGuard.NotNull(validator, nameof(validator))
		        .NotNull(claimResolver, nameof(claimResolver))
		        .NotNull(accountingRepository, nameof(accountingRepository))
		        .NotNull(commonRepository, nameof(commonRepository));

            return validator.Permission.HasNecessaryPermission(EvaluateNecessaryPermission(claimResolver))
	            .ValidateAccountingIdentifier(AccountingNumber, GetType(), nameof(AccountingNumber));
        }

        protected virtual bool EvaluateNecessaryPermission(IClaimResolver claimResolver) => true;

        protected async Task<bool> AccountingExistsAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return await accountingRepository.AccountingExistsAsync(AccountingNumber);
        }

        protected Task<IAccounting> GetAccountingAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.FromResult(AccountingNumber.GetAccounting(DateTime.Today, accountingRepository, ref _accounting));
        }

        #endregion
    }
}