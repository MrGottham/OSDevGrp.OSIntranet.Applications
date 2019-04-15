using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public abstract class AccountGroupViewModelBase : AuditableViewModelBase
    {
        #region Properties

        public int Number { get; set; }

        public string Name { get; set; }

        #endregion
    }
}