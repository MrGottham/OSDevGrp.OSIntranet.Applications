using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Repositories.Models.MicrosoftGraph
{
    [DataContract]
    internal class ContactModel
    {
        #region Properties

        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        public string Identifier { get; set; }

        [DataMember(Name = "displayName", IsRequired = true)]
        public string DisplayName { get; set; }

        [DataMember(Name = "givenName", IsRequired = false)]
        public string GivenName { get; set; }

        [DataMember(Name = "middleName", IsRequired = false)]
        public string MiddleName { get; set; }

        [DataMember(Name = "surname", IsRequired = false)]
        public string Surname { get; set; }

        [DataMember(Name = "homeAddress", IsRequired = false)]
        public PhysicalAddressModel HomeAddress { get; set; }

        [DataMember(Name = "homePhones", IsRequired = false)]
        public List<string> HomePhones { get; set; }

        [DataMember(Name = "mobilePhone", IsRequired = false)]
        public string MobilePhone { get; set; }

        [DataMember(Name = "birthday", IsRequired = false)]
        public DateTime? Birthday { get; set; }

        [DataMember(Name = "emailAddresses", IsRequired = false)]
        public List<EmailAddressModel> EmailAddresses { get; set; }

        [DataMember(Name = "companyName", IsRequired = false)]
        public string CompanyName { get; set; }

        [DataMember(Name = "businessAddress", IsRequired = false)]
        public PhysicalAddressModel BusinessAddress { get; set; }

        [DataMember(Name = "businessPhones", IsRequired = false)]
        public List<string> BusinessPhones { get; set; }

        [DataMember(Name = "businessHomePage", IsRequired = false)]
        public string BusinessHomePage { get; set; }

        [DataMember(Name = "createdDateTime", IsRequired = true)]
        public DateTime CreatedDateTime { get; set; }

        [DataMember(Name = "lastModifiedDateTime", IsRequired = true)]
        public DateTime LastModifiedDateTime { get; set; }

        #endregion
    }

    internal static class ContactModelExtensions
    {
        internal static IContact ToDomain(this ContactModel contactModel, IConverter microsoftGraphModelConverter)
        {
            NullGuard.NotNull(contactModel, nameof(contactModel))
                .NotNull(microsoftGraphModelConverter, nameof(microsoftGraphModelConverter));

            IName name = microsoftGraphModelConverter.Convert<ContactModel, IName>(contactModel);
            IAddress address = contactModel.HomeAddress == null ? new Address() : microsoftGraphModelConverter.Convert<PhysicalAddressModel, IAddress>(contactModel.HomeAddress);

            string homePhone = null;
            if (contactModel.HomePhones != null && contactModel.HomePhones.Any())
            {
                homePhone = contactModel.HomePhones.First();
            }

            IContact contact = new Contact(name, address)
            {
                ExternalIdentifier = contactModel.Identifier,
                HomePhone = homePhone,
                MobilePhone = contactModel.MobilePhone,
                Birthday = contactModel.Birthday?.ToLocalTime().Date,
                MailAddress = contactModel.EmailAddresses == null ? null : microsoftGraphModelConverter.Convert<IEnumerable<EmailAddressModel>, string>(contactModel.EmailAddresses),
                Company = string.IsNullOrWhiteSpace(contactModel.CompanyName) ? null : microsoftGraphModelConverter.Convert<ContactModel, ICompany>(contactModel)
            };

            contact.AddAuditInformation(contactModel.CreatedDateTime, "Microsoft.Graph", contactModel.LastModifiedDateTime, "Microsoft.Graph");

            return contact;
        }

        internal static ICompany ToCompany(this ContactModel contactModel, IConverter microsoftGraphModelConverter)
        {
            NullGuard.NotNull(contactModel, nameof(contactModel))
                .NotNull(microsoftGraphModelConverter, nameof(microsoftGraphModelConverter));

            ICompanyName name = microsoftGraphModelConverter.Convert<ContactModel, ICompanyName>(contactModel);
            IAddress address = contactModel.BusinessAddress == null ? new Address() : microsoftGraphModelConverter.Convert<PhysicalAddressModel, IAddress>(contactModel.BusinessAddress);

            string primaryPhone = null;
            string secondaryPhone = null;
            if (contactModel.BusinessPhones != null && contactModel.BusinessPhones.Any())
            {
                primaryPhone = contactModel.BusinessPhones.ElementAt(0);
                secondaryPhone = contactModel.BusinessPhones.Count > 1 ? contactModel.BusinessPhones.ElementAt(1) : null;
            }

            return new Company(name, address)
            {
                PrimaryPhone = primaryPhone,
                SecondaryPhone = secondaryPhone,
                HomePage = contactModel.BusinessHomePage
            };
        }

        internal static IName ToName(this ContactModel contactModel)
        {
            NullGuard.NotNull(contactModel, nameof(contactModel));

            if (string.IsNullOrWhiteSpace(contactModel.GivenName) && string.IsNullOrWhiteSpace(contactModel.MiddleName) && string.IsNullOrWhiteSpace(contactModel.Surname))
            {
                return new CompanyName(contactModel.DisplayName);
            }

            if (string.IsNullOrWhiteSpace(contactModel.GivenName) == false && string.IsNullOrWhiteSpace(contactModel.MiddleName) && string.IsNullOrWhiteSpace(contactModel.Surname))
            {
                return new CompanyName(contactModel.GivenName);
            }

            if (string.IsNullOrWhiteSpace(contactModel.GivenName) && string.IsNullOrWhiteSpace(contactModel.MiddleName) && string.IsNullOrWhiteSpace(contactModel.Surname) == false)
            {
                return new CompanyName(contactModel.Surname);
            }

            if (string.IsNullOrWhiteSpace(contactModel.MiddleName) == false)
            {
                return new PersonName(contactModel.GivenName, contactModel.MiddleName, contactModel.Surname);
            }

            return new PersonName(contactModel.GivenName, contactModel.Surname);
        }

        internal static ICompanyName ToCompanyName(this ContactModel contactModel)
        {
            NullGuard.NotNull(contactModel, nameof(contactModel));

            return new CompanyName(contactModel.CompanyName);
        }
    }
}
