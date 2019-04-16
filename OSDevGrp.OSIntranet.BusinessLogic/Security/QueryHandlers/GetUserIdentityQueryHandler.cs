using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers
{
    public class GetUserIdentityQueryHandler : IQueryHandler<IGetUserIdentityQuery, IUserIdentity>
    {
        #region Private variables

        private readonly ISecurityRepository _securityRepository;

        #endregion

        #region Constructor

        public GetUserIdentityQueryHandler(ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(securityRepository, nameof(securityRepository));

            _securityRepository = securityRepository;
        }

        #endregion

        #region Methods

        public Task<IUserIdentity> QueryAsync(IGetUserIdentityQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return _securityRepository.GetUserIdentityAsync(query.IdentityIdentifier);
        }

        #endregion
    }
}
