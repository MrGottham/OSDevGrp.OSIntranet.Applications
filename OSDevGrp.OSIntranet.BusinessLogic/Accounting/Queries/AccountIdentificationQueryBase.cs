using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries
{
    public abstract class AccountIdentificationQueryBase : AccountingIdentificationQueryBase, IAccountIdentificationQuery
    {
        #region Private variables

        private string _accountNumber;

        #endregion

        #region Properties

        public string AccountNumber
        {
            get => _accountNumber;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _accountNumber = value.Trim().ToUpper();
            }
        }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return base.Validate(validator, claimResolver, accountingRepository)
                .Object.ShouldBeKnownValue(AccountingNumber, accountingNumber => AccountingExistsAsync(accountingRepository), GetType(), nameof(AccountingNumber))
                .ValidateAccountIdentifier(AccountNumber, GetType(), nameof(AccountNumber));
        }

        #endregion
    }
}