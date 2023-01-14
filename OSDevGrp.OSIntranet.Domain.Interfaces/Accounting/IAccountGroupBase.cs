using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccountGroupBase : IAuditable, IProtectable
    {
        int Number { get; }

        string Name { get; }
    }
}