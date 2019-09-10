﻿using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers
{
    public class GetUserIdentityQueryHandler : IQueryHandler<IGetUserIdentityQuery, IUserIdentity>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly ISecurityRepository _securityRepository;

        #endregion

        #region Constructor

        public GetUserIdentityQueryHandler(IValidator validator, ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(securityRepository, nameof(securityRepository));

            _validator = validator;
            _securityRepository = securityRepository;
        }

        #endregion

        #region Methods

        public async Task<IUserIdentity> QueryAsync(IGetUserIdentityQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(_validator, _securityRepository);

            return await _securityRepository.GetUserIdentityAsync(query.Identifier);
        }

        #endregion
    }
}
