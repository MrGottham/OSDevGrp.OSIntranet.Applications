using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public abstract class CountryCommandBase : CountryIdentificationCommandBase, ICountryCommand
    {
        #region Properties

        public string Name { get; set; }

        public string UniversalName { get; set; }

        public string PhonePrefix { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            return base.Validate(validator, contactRepository)
                .String.ShouldNotBeNullOrWhiteSpace(Name, GetType(), nameof(Name))
                .String.ShouldHaveMinLength(Name, 1, GetType(), nameof(Name))
                .String.ShouldHaveMaxLength(Name, 256, GetType(), nameof(Name))
                .String.ShouldNotBeNullOrWhiteSpace(UniversalName, GetType(), nameof(UniversalName))
                .String.ShouldHaveMinLength(UniversalName, 1, GetType(), nameof(UniversalName))
                .String.ShouldHaveMaxLength(UniversalName, 256, GetType(), nameof(UniversalName))
                .ValidatePhonePrefix(PhonePrefix, GetType(), nameof(PhonePrefix));
        }

        public ICountry ToDomain()
        {
            return new Country(CountryCode, Name, UniversalName, PhonePrefix);
        }

        #endregion
    }
}
