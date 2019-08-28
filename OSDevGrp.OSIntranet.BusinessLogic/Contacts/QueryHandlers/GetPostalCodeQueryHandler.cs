using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers
{
    public class GetPostalCodeQueryHandler : IQueryHandler<IGetPostalCodeQuery, IPostalCode>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IContactRepository _contactRepository;

        #endregion

        #region Constructor

        public GetPostalCodeQueryHandler(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            _validator = validator;
            _contactRepository = contactRepository;
        }

        #endregion

        #region Methods

        public Task<IPostalCode> QueryAsync(IGetPostalCodeQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(_validator, _contactRepository);

            return _contactRepository.GetPostalCodeAsync(query.CountryCode, query.PostalCode);
        }

        #endregion
    }
}
