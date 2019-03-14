using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public abstract class AccountGroupBase : IAccountGroupBase
    {
        public virtual int Number { get; set; }

        public virtual string Name { get; set; }
    }
}