using System.Security.Principal;
using System.Threading;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;

namespace OSDevGrp.OSIntranet.Core.Resolvers
{
    public class ThreadPrincipalResolver : IPrincipalResolver
    {
        #region Methods

        public IPrincipal GetCurrentPrincipal()
        {
            return Thread.CurrentPrincipal;
        }
        
        #endregion
    }
}