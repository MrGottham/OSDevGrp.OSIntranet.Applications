using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers
{
    internal class GetUserInfoAsTokenQueryHandler : IQueryHandler<IGetUserInfoAsTokenQuery, IToken>
    {
        #region Private variables

        private readonly IPrincipalResolver _principalResolver;
        private readonly IUserInfoFactory _userInfoFactory;
        private readonly ITokenGenerator _tokenGenerator;

        #endregion

        #region Constructor

        public GetUserInfoAsTokenQueryHandler(IPrincipalResolver principalResolver, IUserInfoFactory userInfoFactory, ITokenGenerator tokenGenerator)
        {
            NullGuard.NotNull(principalResolver, nameof(principalResolver))
                .NotNull(userInfoFactory, nameof(userInfoFactory))
                .NotNull(tokenGenerator, nameof(tokenGenerator));

            _principalResolver = principalResolver;
            _userInfoFactory = userInfoFactory;
            _tokenGenerator = tokenGenerator;
        }

        #endregion

        #region Methods

        public Task<IToken> QueryAsync(IGetUserInfoAsTokenQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return Task.Run(() =>
            {
                ClaimsPrincipal currentPrincipal = _principalResolver.GetCurrentPrincipal() as ClaimsPrincipal;
                if (currentPrincipal == null)
                {
                    return null;
                }

                IUserInfo userInfo = _userInfoFactory.FromPrincipal(currentPrincipal);
                if (userInfo == null)
                {
                    return null;
                }

                return _tokenGenerator.Generate(new ClaimsIdentity(userInfo.ToClaims()), TimeSpan.FromMinutes(5));
            });
        }

        #endregion
    }
}