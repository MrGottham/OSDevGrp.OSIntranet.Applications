using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers
{
    public class GetCountryCollectionQueryHandler : IQueryHandler<EmptyQuery, IEnumerable<ICountry>>
    {
        #region Private variables

        private readonly IContactRepository _contactRepository;
        private readonly ICountryHelper _countryHelper;

        #endregion

        #region Constructor

        public GetCountryCollectionQueryHandler(IContactRepository contactRepository, ICountryHelper countryHelper)
        {
            NullGuard.NotNull(contactRepository, nameof(contactRepository))
                .NotNull(countryHelper, nameof(countryHelper));

            _contactRepository = contactRepository;
            _countryHelper = countryHelper;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<ICountry>> QueryAsync(EmptyQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            IEnumerable<ICountry> countries = await _contactRepository.GetCountriesAsync();

            return _countryHelper.ApplyLogicForPrincipal(countries);
        }

        #endregion
    }
}
