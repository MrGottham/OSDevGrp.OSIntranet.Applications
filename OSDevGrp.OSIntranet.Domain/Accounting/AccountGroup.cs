using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class AccountGroup : AccountGroupBase, IAccountGroup
    {
        public virtual AccountGroupType AccountGroupType { get; set; }
    }
}