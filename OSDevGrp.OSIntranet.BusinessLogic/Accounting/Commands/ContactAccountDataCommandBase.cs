using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public abstract class ContactAccountDataCommandBase : AccountCoreDataCommandBase<IContactAccount>, IContactAccountDataCommand
    {
        #region Private variables

        private IPaymentTerm _paymentTerm;

        #endregion

        #region Properties

        public string MailAddress { get; set; }

        public string PrimaryPhone { get; set; }

        public string SecondaryPhone { get; set; }

        public int PaymentTermNumber { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, accountingRepository, commonRepository)
                .ValidateMailAddress(MailAddress, GetType(), nameof(MailAddress), true)
                .ValidatePhoneNumber(PrimaryPhone, GetType(), nameof(PrimaryPhone), true)
                .ValidatePhoneNumber(SecondaryPhone, GetType(), nameof(SecondaryPhone), true)
                .ValidatePaymentTermIdentifier(PaymentTermNumber, GetType(), nameof(PaymentTermNumber))
                .Object.ShouldBeKnownValue(PaymentTermNumber, paymentTermNumber => Task.Run(async () => await GetPaymentTermAsync(accountingRepository) != null), GetType(), nameof(PaymentTermNumber));
        }

        public override IContactAccount ToDomain(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            IAccounting accounting = GetAccountingAsync(accountingRepository).GetAwaiter().GetResult();
            IPaymentTerm paymentTerm = GetPaymentTermAsync(accountingRepository).GetAwaiter().GetResult();

            return new ContactAccount(accounting, AccountNumber, AccountName, paymentTerm)
            {
                Description = Description,
                Note = Note,
                MailAddress = MailAddress,
                PrimaryPhone = PrimaryPhone,
                SecondaryPhone = SecondaryPhone
            };
        }

        protected Task<IPaymentTerm> GetPaymentTermAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.FromResult(PaymentTermNumber.GetPaymentTerm(accountingRepository, ref _paymentTerm));
        }

        #endregion
    }
}