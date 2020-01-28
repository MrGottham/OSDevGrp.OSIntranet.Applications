using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers
{
    public class GetMatchingContactCollectionQueryHandler : ContactQueryHandlerBase<IGetMatchingContactCollectionQuery, IEnumerable<IContact>>
    {
        #region Constructor

        public GetMatchingContactCollectionQueryHandler(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository) 
            : base(validator, microsoftGraphRepository, contactRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IEnumerable<IContact>> GetDataAsync(IGetMatchingContactCollectionQuery query, IRefreshableToken token)
        {
            NullGuard.NotNull(query, nameof(query))
                .NotNull(token, nameof(token));

            string searchFor = query.SearchFor;
            SearchOptions searchOptions = query.SearchOptions;

            IEnumerable<IContact> matchingContacts = (await MicrosoftGraphRepository.GetContactsAsync(token))
                .Where(contact => contact.IsMatch(searchFor, searchOptions))
                .ToArray();

            return await ContactRepository.ApplyContactSupplementAsync(matchingContacts);
        }

        #endregion
    }
}