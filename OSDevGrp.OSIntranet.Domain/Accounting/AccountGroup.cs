using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class AccountGroup : AccountGroupBase, IAccountGroup
    {
        public AccountGroup(int number, string name, AccountGroupType accountGroupType)
            : base(number, name)
        {
            AccountGroupType = accountGroupType;
        }

        public AccountGroupType AccountGroupType { get; }
    }
}