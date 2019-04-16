using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers
{
    public class GetClaimCollectionQueryHandler : IQueryHandler<EmptyQuery, IEnumerable<Claim>>
    {
        #region Private variables

        private readonly ISecurityRepository _securityRepository;

        #endregion

        #region Constructor

        public GetClaimCollectionQueryHandler(ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(securityRepository, nameof(securityRepository));

            _securityRepository = securityRepository;
        }

        #endregion

        #region Methods

        public Task<IEnumerable<Claim>> QueryAsync(EmptyQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return _securityRepository.GetClaimsAsync();
        }

        #endregion
    }
}
