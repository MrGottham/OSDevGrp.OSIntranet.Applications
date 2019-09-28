using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers
{
    public class GetCountryQueryHandler : IQueryHandler<IGetCountryQuery, ICountry>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IContactRepository _contactRepository;
        private readonly ICountryHelper _countryHelper;

        #endregion

        #region Constructor

        public GetCountryQueryHandler(IValidator validator, IContactRepository contactRepository, ICountryHelper countryHelper)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository))
                .NotNull(countryHelper, nameof(countryHelper));

            _validator = validator;
            _contactRepository = contactRepository;
            _countryHelper = countryHelper;
        }

        #endregion

        #region Methods

        public async Task<ICountry> QueryAsync(IGetCountryQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(_validator, _contactRepository);

            ICountry country = await _contactRepository.GetCountryAsync(query.CountryCode);

            return _countryHelper.ApplyLogicForPrincipal(country);
        }

        #endregion
    }
}
