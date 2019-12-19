using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers
{
    public class GetContactGroupQueryHandler : IQueryHandler<IGetContactGroupQuery, IContactGroup>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IContactRepository _contactRepository;

        #endregion

        #region Constructor

        public GetContactGroupQueryHandler(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            _validator = validator;
            _contactRepository = contactRepository;
        }

        #endregion

        #region Methods

        public async Task<IContactGroup> QueryAsync(IGetContactGroupQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(_validator, _contactRepository);

            return await _contactRepository.GetContactGroupAsync(query.Number);
        }

        #endregion
    }
}
