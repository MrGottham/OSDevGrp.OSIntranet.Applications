using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public class DeleteCountryCommand : CountryIdentificationCommandBase, IDeleteCountryCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            return base.Validate(validator, contactRepository)
                .Object.ShouldBeKnownValue(CountryCode,
                    countryCode => Task.Run(async () => await GetCountryAsync(contactRepository) != null), GetType(), nameof(CountryCode))
                .Object.ShouldBeDeletable(CountryCode, countryCode => GetCountryAsync(contactRepository), GetType(), nameof(CountryCode));
        }

        #endregion
    }
}
