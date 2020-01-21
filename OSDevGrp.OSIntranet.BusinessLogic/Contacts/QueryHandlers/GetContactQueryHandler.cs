using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers
{
    public class GetContactQueryHandler : ContactQueryHandlerBase<IGetContactQuery, IContact>
    {
        #region Constructor

        public GetContactQueryHandler(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository) 
            : base(validator, microsoftGraphRepository, contactRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IContact> GetDataAsync(IGetContactQuery query, IRefreshableToken token)
        {
            NullGuard.NotNull(query, nameof(query))
                .NotNull(token, nameof(token));

            IContact contact = await MicrosoftGraphRepository.GetContactAsync(token, query.ExternalIdentifier);

            return await ContactRepository.ApplyContactSupplementAsync(contact);
        }
        
        #endregion
    }
}