using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class AccountGroupModelBase : AuditModelBase
    {
        public virtual string Name { get; set; }

        public virtual bool Deletable { get; set; }
    }
}
