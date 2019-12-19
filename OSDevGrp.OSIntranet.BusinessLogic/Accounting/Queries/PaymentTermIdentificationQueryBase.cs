using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries
{
    public abstract class PaymentTermIdentificationQueryBase : IPaymentTermIdentificationQuery
    {
        #region Properties

        public int Number { get; set; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return validator.ValidatePaymentTermIdentifier(Number, GetType(), nameof(Number));
        }

        #endregion
    }
}
