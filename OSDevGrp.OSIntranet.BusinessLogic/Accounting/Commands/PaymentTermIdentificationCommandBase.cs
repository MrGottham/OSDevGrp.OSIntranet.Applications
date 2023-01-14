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
    public abstract class PaymentTermIdentificationCommandBase : IPaymentTermIdentificationCommand
    {
        #region Private variables

        private IPaymentTerm _paymentTerm;

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
                .ValidatePaymentTermIdentifier(Number, GetType(), nameof(Number));
        }

        protected Task<IPaymentTerm> GetPaymentTermAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.FromResult(Number.GetPaymentTerm(accountingRepository, ref _paymentTerm));
        }

        #endregion
    }
}