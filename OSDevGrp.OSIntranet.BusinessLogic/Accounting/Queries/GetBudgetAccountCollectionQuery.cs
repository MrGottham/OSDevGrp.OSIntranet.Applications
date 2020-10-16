using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries
{
    public class GetBudgetAccountCollectionQuery : AccountingIdentificationQueryBase, IGetBudgetAccountCollectionQuery
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return base.Validate(validator, accountingRepository)
                .Object.ShouldBeKnownValue(AccountingNumber, accountingRepository.AccountingExistsAsync, GetType(), nameof(AccountingNumber));
        }

        #endregion
    }
}