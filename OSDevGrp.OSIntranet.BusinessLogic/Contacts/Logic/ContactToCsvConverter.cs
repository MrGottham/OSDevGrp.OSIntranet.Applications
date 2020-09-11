using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Converters;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic
{
    public class ContactToCsvConverter : DomainObjectToCsvConverterBase<IContact>, IContactToCsvConverter
    {
        #region Constructor

        public ContactToCsvConverter()
            : base(DefaultFormatProvider.Create())
        {
        }

        #endregion

        #region Methods

        public override Task<IEnumerable<string>> GetColumnNamesAsync()
        {
            string[] columnNames =
            {
                "Fulde navn",
                "Fornavn",
                "Mellemnavn(e)",
                "Efternavn/Firmanavn",
                "Adresse (linje 1)",
                "Adresse (linje 2)",
                "Postnr.",
                "By",
                "Stat",
                "Land",
                "Primær tlf.nr./Mobil",
                "Sekundær tlf.nr./Hjem",
                "Mailadresse",
                "Webside",
                "Fødselsdato",
                "Kontaktgruppe, nr.",
                "Kontaktgruppe, navn",
                "Bekendtskab",
                "Udlånsfrist",
                "Betalingsbetingelse, nr.",
                "Betalingsbetingelse, navn",
                "Firma, navn",
                "Firma, adresse (linje 1)",
                "Firma, adresse (linje 2)",
                "Firma, postnr.",
                "Firma, by",
                "Firma, stat",
                "Firma, land",
                "Firma, primær tlf.nr.",
                "Firma, sekundær tlf.nr.",
                "Firma, webside"
            };

            return Task.FromResult(columnNames.AsEnumerable());
        }

        public override Task<IEnumerable<string>> ConvertAsync(IContact contact)
        {
            NullGuard.NotNull(contact, nameof(contact));

            ResetColumns()
                .AddColumnValue(contact.Name?.DisplayName);
            AddColumns(contact.Name);
            AddColumns(contact.Address);
            AddColumnValue(string.IsNullOrWhiteSpace(contact.PrimaryPhone) ? contact.MobilePhone : contact.PrimaryPhone)
                .AddColumnValue(string.IsNullOrWhiteSpace(contact.SecondaryPhone) ? contact.HomePhone : contact.SecondaryPhone)
                .AddColumnValue(contact.MailAddress)
                .AddColumnValue(contact.PersonalHomePage)
                .AddColumnValue(contact.Birthday, "yyyy-MM-dd");
            AddColumns(contact.ContactGroup);
            AddColumnValue(contact.Acquaintance)
                .AddColumnValue(contact.LendingLimit);
            AddColumns(contact.PaymentTerm);
            AddColumns(contact.Company);

            return Task.FromResult(ColumnValues);
        }

        private void AddColumns(IName name)
        {
            if (name != null && name is IPersonName personName)
            {
                AddColumnValue(personName.GivenName)
                    .AddColumnValue(personName.MiddleName)
                    .AddColumnValue(personName.Surname);
                return;
            }

            if (name != null && name is ICompanyName companyName)
            {
                AddEmptyColumn()
                    .AddEmptyColumn()
                    .AddColumnValue(companyName.FullName);
                return;
            }

            AddEmptyColumn()
                .AddEmptyColumn()
                .AddEmptyColumn();
        }

        private void AddColumns(ICompany company)
        {
            if (company != null)
            {
                AddColumnValue(company.Name?.FullName);
                AddColumns(company.Address);
                AddColumnValue(company.PrimaryPhone)
                    .AddColumnValue(company.SecondaryPhone)
                    .AddColumnValue(company.HomePage);
                return;
            }

            AddEmptyColumn();
            AddColumns((IAddress) null);
            AddEmptyColumn()
                .AddEmptyColumn()
                .AddEmptyColumn();
        }

        private void AddColumns(IAddress address)
        {
            if (address != null)
            {
                AddColumnValue(address.StreetLine1)
                    .AddColumnValue(address.StreetLine2)
                    .AddColumnValue(address.PostalCode)
                    .AddColumnValue(address.City)
                    .AddColumnValue(address.State)
                    .AddColumnValue(address.Country);
                return;
            }

            AddEmptyColumn()
                .AddEmptyColumn()
                .AddEmptyColumn()
                .AddEmptyColumn()
                .AddEmptyColumn()
                .AddEmptyColumn();
        }

        private void AddColumns(IContactGroup contactGroup)
        {
            if (contactGroup != null)
            {
                AddColumnValue(contactGroup.Number)
                    .AddColumnValue(contactGroup.Name);
                return;
            }

            AddEmptyColumn()
                .AddEmptyColumn();
        }

        private void AddColumns(IPaymentTerm paymentTerm)
        {
            if (paymentTerm != null)
            {
                AddColumnValue(paymentTerm.Number)
                    .AddColumnValue(paymentTerm.Name);
                return;
            }

            AddEmptyColumn()
                .AddEmptyColumn();
        }

        #endregion
    }
}