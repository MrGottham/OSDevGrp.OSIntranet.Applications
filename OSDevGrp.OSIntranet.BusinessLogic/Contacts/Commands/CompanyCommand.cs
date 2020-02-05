using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public class CompanyCommand : ICompanyCommand
    {
        #region Properties

        public ICompanyNameCommand Name { get; set; }

        public IAddressCommand Address { get; set; }

        public string PrimaryPhone { get; set; }

        public string SecondaryPhone { get; set; }

        public string HomePage { get; set; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            Name?.Validate(validator);
            Address?.Validate(validator);

            return validator.Object.ShouldNotBeNull(Name, GetType(), nameof(Name))
                .ValidatePhoneNumber(PrimaryPhone, GetType(), nameof(PrimaryPhone))
                .ValidatePhoneNumber(SecondaryPhone, GetType(), nameof(SecondaryPhone))
                .ValidateUrl(HomePage, GetType(), nameof(HomePage));
        }

        public ICompany ToDomain()
        {
            ICompanyName companyName = (ICompanyName) Name?.ToDomain();
            IAddress address = Address != null && Address.IsEmpty() == false ? Address.ToDomain() : null;

            return FillCompany(address == null ? new Company(companyName) : new Company(companyName, address));
        }

        private ICompany FillCompany(ICompany company)
        {
            NullGuard.NotNull(company, nameof(company));

            company.PrimaryPhone = string.IsNullOrWhiteSpace(PrimaryPhone) ? null : PrimaryPhone;
            company.SecondaryPhone = string.IsNullOrWhiteSpace(SecondaryPhone) ? null : SecondaryPhone;
            company.HomePage = string.IsNullOrWhiteSpace(HomePage) ? null : HomePage;

            return company;
        }

        #endregion
    }
}