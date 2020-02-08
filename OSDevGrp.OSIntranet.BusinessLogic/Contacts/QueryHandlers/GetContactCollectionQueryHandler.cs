using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers
{
    public class GetContactCollectionQueryHandler : ContactQueryHandlerBase<IGetContactCollectionQuery, IEnumerable<IContact>>
    {
        #region Constructor

        public GetContactCollectionQueryHandler(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository) 
            : base(validator, microsoftGraphRepository, contactRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IEnumerable<IContact>> GetDataAsync(IGetContactCollectionQuery query, IRefreshableToken token)
        {
            NullGuard.NotNull(query, nameof(query))
                .NotNull(token, nameof(token));

            IEnumerable<IContact> contacts = await MicrosoftGraphRepository.GetContactsAsync(token);
            if (contacts == null)
            {
                return new List<IContact>(0);
            }

            return await ContactRepository.ApplyContactSupplementAsync(contacts);
        }

        #endregion
    }
}