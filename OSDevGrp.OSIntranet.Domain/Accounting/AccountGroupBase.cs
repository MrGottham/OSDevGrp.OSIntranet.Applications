using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public abstract class AccountGroupBase : IAccountGroupBase
    {
        protected AccountGroupBase(int number, string name)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name));

            Number = number;
            Name = name;
        }

        public int Number { get; }

        public string Name { get; }
    }
}