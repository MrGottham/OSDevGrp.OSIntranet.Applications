using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries
{
    public abstract class AccountGroupIdentificationQueryBase : IAccountGroupIdentificationQuery
    {
        #region Properties

        public int Number { get; set; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return validator.ValidateAccountGroupIdentifier(Number, GetType(), nameof(Number));
        }

        #endregion
    }
}