using System.Collections.Generic;
using System.Threading.Tasks;
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

        #endregion

        #region Constructor

        public GetCountryCollectionQueryHandler(IContactRepository contactRepository)
        {
            NullGuard.NotNull(contactRepository, nameof(contactRepository));

            _contactRepository = contactRepository;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<ICountry>> QueryAsync(EmptyQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return await _contactRepository.GetCountriesAsync();
        }

        #endregion
    }
}
