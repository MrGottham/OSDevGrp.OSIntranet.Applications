using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers
{
    public class GetAuthorizeUriForMicrosoftGraphQueryHandler : IQueryHandler<IGetAuthorizeUriForMicrosoftGraphQuery, Uri>
    {
        #region Private variables

        private readonly IMicrosoftGraphRepository _microsoftGraphRepository;

        #endregion

        #region Constructor

        public GetAuthorizeUriForMicrosoftGraphQueryHandler(IMicrosoftGraphRepository microsoftGraphRepository)
        {
            NullGuard.NotNull(microsoftGraphRepository, nameof(microsoftGraphRepository));

            _microsoftGraphRepository = microsoftGraphRepository;
        }

        #endregion

        #region Methods

        public async Task<Uri> QueryAsync(IGetAuthorizeUriForMicrosoftGraphQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return await _microsoftGraphRepository.GetAuthorizeUriAsync(query.RedirectUri, query.StateIdentifier);
        }

        #endregion
    }
}
