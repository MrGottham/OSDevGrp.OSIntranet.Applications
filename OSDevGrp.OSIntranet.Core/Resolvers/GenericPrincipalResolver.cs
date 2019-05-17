using System.Security.Principal;
using System.Threading;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;

namespace OSDevGrp.OSIntranet.Core.Resolvers
{
    public class GenericPrincipalResolver : IPrincipalResolver
    {
        #region Private variables

        private IPrincipal _currentPrincipal;

        #endregion

        #region Constructors

        public GenericPrincipalResolver()
            : this(Thread.CurrentPrincipal)
        {
        }

        public GenericPrincipalResolver(IPrincipal currentPrincipal)
        {
            _currentPrincipal = currentPrincipal;
        }

        #endregion

        #region Methods

        public IPrincipal GetCurrentPrincipal()
        {
            return _currentPrincipal;
        }
        
        #endregion
    }
}