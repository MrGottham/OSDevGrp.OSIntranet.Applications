using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries
{
    public class GetPostalCodeQuery : PostalCodeIdentificationQueryBase, IGetPostalCodeQuery
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            return base.Validate(validator, contactRepository)
                .Object.ShouldBeKnownValue(CountryCode, countryCode => Task.Run(async () => await GetCountryAsync(contactRepository) != null), GetType(), nameof(CountryCode));
        }

        #endregion
    }
}
