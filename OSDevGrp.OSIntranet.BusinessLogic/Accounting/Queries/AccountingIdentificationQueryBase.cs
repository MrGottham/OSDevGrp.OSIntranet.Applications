using System;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

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

        public IValidator Validate(IValidator validator, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return validator.ValidateAccountingIdentifier(AccountingNumber, GetType(), nameof(AccountingNumber))
                .DateTime.ShouldBePastDateOrToday(StatusDate, GetType(), nameof(StatusDate));
        }

        #endregion
    }
}