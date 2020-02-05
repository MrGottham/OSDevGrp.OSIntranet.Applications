using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public abstract class ContactDataCommandBase : ContactCommandBase, IContactDataCommand
    {
        #region Private variables

        private IContactGroup _contactGroup;
        private IPaymentTerm _paymentTerm;

        #endregion

        #region Properties

        public INameCommand Name { get; set; }

        public IAddressCommand Address { get; set; }

        public string HomePhone { get; set; }

        public string MobilePhone { get; set; }

        public DateTime? Birthday { get; set; }

        public string MailAddress { get; set; }

        public ICompanyCommand Company { get; set; }

        public int ContactGroupIdentifier { get; set; }

        public string Acquaintance { get; set; }

        public string PersonalHomePage { get; set; }

        public int LendingLimit { get; set; }

        public int PaymentTermIdentifier { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(microsoftGraphRepository, nameof(microsoftGraphRepository))
                .NotNull(contactRepository, nameof(contactRepository))
                .NotNull(accountingRepository, nameof(accountingRepository));

            Name?.Validate(validator);
            Address?.Validate(validator);
            Company?.Validate(validator);

            if (Birthday.HasValue)
            {
                validator.DateTime.ShouldBePastDateOrToday(Birthday.Value, GetType(), nameof(Birthday));
            }

            return base.Validate(validator, microsoftGraphRepository, contactRepository, accountingRepository)
                .Object.ShouldNotBeNull(Name, GetType(), nameof(Name))
                .ValidatePhoneNumber(HomePhone, GetType(), nameof(HomePhone))
                .ValidatePhoneNumber(MobilePhone, GetType(), nameof(MobilePhone))
                .ValidateMailAddress(MailAddress, GetType(), nameof(MailAddress))
                .ValidateContactGroupIdentifier(ContactGroupIdentifier, GetType(), nameof(ContactGroupIdentifier))
                .Object.ShouldBeKnownValue(ContactGroupIdentifier, contactGroupIdentifier => Task.Run(async () => await GetContactGroupAsync(contactRepository) != null), GetType(), nameof(ContactGroupIdentifier))
                .String.ShouldHaveMinLength(Acquaintance, 1, GetType(), nameof(Acquaintance),  true)
                .String.ShouldHaveMaxLength(Acquaintance, 4096, GetType(), nameof(Acquaintance), true)
                .ValidatePaymentTermIdentifier(PaymentTermIdentifier, GetType(), nameof(PaymentTermIdentifier))
                .ValidateUrl(PersonalHomePage, GetType(), nameof(PersonalHomePage))
                .Integer.ShouldBeBetween(LendingLimit, 1, 365, GetType(), nameof(LendingLimit))
                .Object.ShouldBeKnownValue(PaymentTermIdentifier, paymentTermIdentifier => Task.Run(async () => await GetPaymentTermAsync(accountingRepository) != null), GetType(), nameof(PaymentTermIdentifier));
        }

        public virtual IContact ToDomain(IContactRepository contactRepository, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(contactRepository, nameof(contactRepository))
                .NotNull(accountingRepository, nameof(accountingRepository));

            IName name = Name?.ToDomain();
            IAddress address = Address != null && Address.IsEmpty() == false ? Address.ToDomain() : null;
            IContactGroup contactGroup = GetContactGroupAsync(contactRepository).GetAwaiter().GetResult();
            IPaymentTerm paymentTerm = GetPaymentTermAsync(accountingRepository).GetAwaiter().GetResult();

            return FillContact(address == null ? new Contact(name) : new Contact(name, address), contactGroup, paymentTerm);
        }

        protected Task<IContactGroup> GetContactGroupAsync(IContactRepository contactRepository)
        {
            NullGuard.NotNull(contactRepository, nameof(contactRepository));

            return Task.Run(() => ContactGroupIdentifier.GetContactGroup(contactRepository, ref _contactGroup));
        }

        protected Task<IPaymentTerm> GetPaymentTermAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.Run(() => PaymentTermIdentifier.GetPaymentTerm(accountingRepository, ref _paymentTerm));
        }

        private IContact FillContact(IContact contact, IContactGroup contactGroup, IPaymentTerm paymentTerm)
        {
            NullGuard.NotNull(contact, nameof(contact))
                .NotNull(contactGroup, nameof(contactGroup))
                .NotNull(paymentTerm, nameof(paymentTerm));

            contact.HomePhone = string.IsNullOrWhiteSpace(HomePhone) ? null : HomePhone;
            contact.MobilePhone = string.IsNullOrWhiteSpace(MobilePhone) ? null : MobilePhone;
            contact.Birthday = Birthday?.Date;
            contact.MailAddress = string.IsNullOrWhiteSpace(MailAddress) ? null : MailAddress;
            contact.Company = Company?.ToDomain();
            contact.ContactGroup = contactGroup;
            contact.Acquaintance = string.IsNullOrWhiteSpace(Acquaintance) ? null : Acquaintance;
            contact.PersonalHomePage = string.IsNullOrWhiteSpace(PersonalHomePage) ? null : PersonalHomePage;
            contact.LendingLimit = LendingLimit;
            contact.PaymentTerm = paymentTerm;

            return contact;
        }

        #endregion
    }
}