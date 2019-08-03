using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries
{
    public abstract class AccountingIdentificationQueryBase : IAccountingIdentificationQuery
    {
        #region Private variables

        private IAccounting _accounting;

        #endregion

        #region Properties

        public int AccountingNumber { get; set; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return validator.ValidateAccountingIdentifier(AccountingNumber, GetType(), nameof(AccountingNumber));
        }

        protected Task<IAccounting> GetAccounting(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.Run(() => AccountingNumber.GetAccounting(accountingRepository, ref _accounting));
        }

        #endregion
    }
}