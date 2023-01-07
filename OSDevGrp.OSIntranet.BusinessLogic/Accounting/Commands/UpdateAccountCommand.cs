using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public class UpdateAccountCommand : AccountDataCommandBase, IUpdateAccountCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
	            .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, claimResolver, accountingRepository, commonRepository)
                .Object.ShouldBeKnownValue(AccountNumber, accountNumber => AccountExistsAsync(accountingRepository), GetType(), nameof(AccountNumber));
        }

        #endregion
    }
}