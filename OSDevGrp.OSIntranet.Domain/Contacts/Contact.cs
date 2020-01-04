using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.Contacts
{
    public class Contact : AuditableBase, IContact
    {
        #region Private variables

        private string _internalIdentifier;
        private string _externalIdentifier;
        private string _primaryPhone;
        private string _secondaryPhone;
        private string _mailAddress;
        private IContactGroup _contactGroup;
        private string _acquaintance;
        private string _personalHomePage;
        private IPaymentTerm _paymentTerm;

        #endregion

        #region Constructors

        public Contact(IName name)
            : this(name, new Address())
        {
        }

        public Contact(IName name, IAddress address)
        {
            NullGuard.NotNull(name, nameof(name))
                .NotNull(address, nameof(address));

            Name = name;
            Address = address;
        }

        #endregion

        #region Properties

        public string InternalIdentifier
        {
            get => _internalIdentifier;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _internalIdentifier = value.Trim();
            }
        }

        public string ExternalIdentifier
        {
            get => _externalIdentifier;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _externalIdentifier = value.Trim();
            }
        }

        public IName Name { get; }

        public IAddress Address { get; }

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

        public string HomePhone
        {
            get => SecondaryPhone;
            set => SecondaryPhone = value;
        }

        public string MobilePhone
        {
            get => PrimaryPhone;
            set => PrimaryPhone = value;
        }

        public DateTime? Birthday { get; set; }

        public string MailAddress
        {
            get => _mailAddress;
            set => _mailAddress = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public ICompany Company { get; set; }

        public IContactGroup ContactGroup
        {
            get => _contactGroup;
            set
            {
                NullGuard.NotNull(value, nameof(value));

                _contactGroup = value;
            }
        }

        public string Acquaintance
        {
            get => _acquaintance;
            set => _acquaintance = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string PersonalHomePage
        {
            get => _personalHomePage;
            set => _personalHomePage = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public int LendingLimit { get; set; }

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
    }
}
