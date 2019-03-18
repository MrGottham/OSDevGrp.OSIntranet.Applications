using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class BudgetAccountGroup : AccountGroupBase, IBudgetAccountGroup
    {
        public BudgetAccountGroup(int number, string name)
            : base(number, name)
        {
        }
    }
}