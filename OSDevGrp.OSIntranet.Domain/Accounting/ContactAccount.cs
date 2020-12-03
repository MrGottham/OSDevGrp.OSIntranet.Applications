using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class ContactAccount : AccountBase<IContactAccount>, IContactAccount
    {
        #region Private variables

        private string _mailAddress;
        private string _primaryPhone;
        private string _secondaryPhone;
        private IPaymentTerm _paymentTerm;

        #endregion

        #region Constructors

        public ContactAccount(IAccounting accounting, string accountNumber, string accountName, IPaymentTerm paymentTerm)
            : this(accounting, accountNumber, accountName, paymentTerm, new ContactInfoCollection(), new PostingLineCollection())
        {
        }

        public ContactAccount(IAccounting accounting, string accountNumber, string accountName, IPaymentTerm paymentTerm, IContactInfoCollection contactInfoCollection, IPostingLineCollection postingLineCollection)
            : base(accounting, accountNumber, accountName, postingLineCollection)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm))
                .NotNull(contactInfoCollection, nameof(contactInfoCollection));

            PaymentTerm = paymentTerm;
            ContactInfoCollection = contactInfoCollection;
        }

        #endregion

        #region Properties

        public string MailAddress
        {
            get => _mailAddress;
            set => _mailAddress = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string PrimaryPhone
        {
            get => _primaryPhone;
            set => _primaryPhone = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string SecondaryPhone
        {
            get => _secondaryPhone;
            set => _secondaryPhone = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public IPaymentTerm PaymentTerm
        { 
            get => _paymentTerm;
            set
            {
                NullGuard.NotNull(value, nameof(value));

                _paymentTerm = value;
            } 
        }

        public ContactAccountType ContactAccountType
        {
            get
            {
                decimal balance = ValuesAtStatusDate.Balance;

                switch (Accounting.BalanceBelowZero)
                {
                    case BalanceBelowZeroType.Debtors:
                        if (balance < 0M)
                        {
                            return ContactAccountType.Debtor;
                        }

                        if (balance > 0M)
                        {
                            return ContactAccountType.Creditor;
                        }

                        break;

                    case BalanceBelowZeroType.Creditors:
                        if (balance < 0M)
                        {
                            return ContactAccountType.Creditor;
                        }

                        if (balance > 0M)
                        {
                            return ContactAccountType.Debtor;
                        }

                        break;
                }

                return ContactAccountType.None;
            }
        }

        public IContactInfoValues ValuesAtStatusDate => ContactInfoCollection.ValuesAtStatusDate;

        public IContactInfoValues ValuesAtEndOfLastMonthFromStatusDate => ContactInfoCollection.ValuesAtEndOfLastMonthFromStatusDate;

        public IContactInfoValues ValuesAtEndOfLastYearFromStatusDate => ContactInfoCollection.ValuesAtEndOfLastYearFromStatusDate;

        public IContactInfoCollection ContactInfoCollection { get; private set; }

        #endregion

        #region Methods

        protected override Task[] GetCalculationTasks(DateTime statusDate)
        {
            return new[]
            {
                CalculateContactInfoCollectionAsync(statusDate),
                CalculatePostingLineCollectionAsync(statusDate)
            };
        }

        protected override IContactAccount GetCalculationResult() => this;

        protected override IContactAccount AlreadyCalculated() => this;

        private async Task CalculateContactInfoCollectionAsync(DateTime statusDate)
        {
            ContactInfoCollection = await ContactInfoCollection.CalculateAsync(statusDate);
        }

        #endregion
    }
}