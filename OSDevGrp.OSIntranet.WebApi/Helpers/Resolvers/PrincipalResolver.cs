using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers
{
    public class PrincipalResolver : IPrincipalResolver
    {
        #region Private variables

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public PrincipalResolver(IHttpContextAccessor httpContextAccessor)
        {
            NullGuard.NotNull(httpContextAccessor, nameof(httpContextAccessor));

            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        public IPrincipal GetCurrentPrincipal()
        {
            return _httpContextAccessor.HttpContext.User;
        }
        
        #endregion
    }
}