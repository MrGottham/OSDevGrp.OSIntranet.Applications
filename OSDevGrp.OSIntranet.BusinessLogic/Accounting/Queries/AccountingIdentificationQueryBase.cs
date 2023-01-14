using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries
{
    public abstract class AccountingIdentificationQueryBase : IAccountingIdentificationQuery
    {
        #region Private variables

        private DateTime _statusDate = DateTime.Today;

        #endregion

        #region Properties

        public int AccountingNumber { get; set; }

        public DateTime StatusDate 
        { 
            get => _statusDate.Date;
            set => _statusDate = value.Date; 
        }

        #endregion

        #region Methods

        public virtual IValidator Validate(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return validator.Permission.HasNecessaryPermission(claimResolver.CanAccessAccounting(AccountingNumber))
                .ValidateAccountingIdentifier(AccountingNumber, GetType(), nameof(AccountingNumber))
                .DateTime.ShouldBePastDateOrToday(StatusDate, GetType(), nameof(StatusDate));
        }

        protected async Task<bool> AccountingExistsAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return await accountingRepository.AccountingExistsAsync(AccountingNumber);
        }

        #endregion
    }
}