using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccountGroupBase : IAuditable
    {
        int Number { get; }

        string Name { get; }
    }
}