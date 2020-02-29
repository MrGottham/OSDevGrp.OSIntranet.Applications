using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

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

        #region Constructor

        public ContactAccount(IAccounting accounting, string accountNumber, string accountName, IPaymentTerm paymentTerm)
            : base(accounting, accountNumber, accountName)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm));

            PaymentTerm = paymentTerm;
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

        #endregion

        #region Methods

        protected override IContactAccount Calculate(DateTime statusDate)
        {
            return this;
        }

        #endregion
    }
}