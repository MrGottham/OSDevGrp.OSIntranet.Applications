using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public abstract class PostalCodeCommandBase : PostalCodeIdentificationCommandBase, IPostalCodeCommand
    {
        #region Properties

        public string City { get; set; }

        public string State { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            return base.Validate(validator, contactRepository)
                .Object.ShouldBeKnownValue(CountryCode, countryCode => Task.Run(async () => await GetCountryAsync(contactRepository) != null), GetType(), nameof(CountryCode))
                .String.ShouldNotBeNullOrWhiteSpace(City, GetType(), nameof(City))
                .String.ShouldHaveMinLength(City, 1, GetType(), nameof(City))
                .String.ShouldHaveMaxLength(City, 256, GetType(), nameof(City))
                .String.ShouldHaveMinLength(State, 1, GetType(), nameof(State), true)
                .String.ShouldHaveMaxLength(State, 256, GetType(), nameof(State), true);
        }

        public IPostalCode ToDomain(IContactRepository contactRepository)
        {
            NullGuard.NotNull(contactRepository, nameof(contactRepository));

            ICountry country = GetCountryAsync(contactRepository).GetAwaiter().GetResult();

            return new PostalCode(country, PostalCode, City, State);
        }

        #endregion
    }
}
