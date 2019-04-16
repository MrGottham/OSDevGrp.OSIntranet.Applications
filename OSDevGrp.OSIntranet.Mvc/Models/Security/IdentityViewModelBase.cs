using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Security
{
    public abstract class IdentityViewModelBase : AuditableViewModelBase
    {
        #region Properties

        public int Identifier { get; set; }

        #endregion
    }
}
