using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries
{
    public class GetDebtorAccountCollectionQuery : AccountingIdentificationQueryBase, IGetDebtorAccountCollectionQuery
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return base.Validate(validator, claimResolver, accountingRepository)
                .Object.ShouldBeKnownValue(AccountingNumber, accountingRepository.AccountingExistsAsync, GetType(), nameof(AccountingNumber));
        }

        #endregion
    }
}