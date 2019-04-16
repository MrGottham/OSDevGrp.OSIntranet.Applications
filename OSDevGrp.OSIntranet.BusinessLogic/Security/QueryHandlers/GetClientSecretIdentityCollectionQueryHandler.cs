using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers
{
    public class GetClientSecretIdentityCollectionQueryHandler : IQueryHandler<EmptyQuery, IEnumerable<IClientSecretIdentity>>
    {
        #region Private variables

        private readonly ISecurityRepository _securityRepository;

        #endregion

        #region Constructor

        public GetClientSecretIdentityCollectionQueryHandler(ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(securityRepository, nameof(securityRepository));

            _securityRepository = securityRepository;
        }

        #endregion

        #region Methods

        public Task<IEnumerable<IClientSecretIdentity>> QueryAsync(EmptyQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return _securityRepository.GetClientSecretIdentitiesAsync();
        }

        #endregion
    }
}
