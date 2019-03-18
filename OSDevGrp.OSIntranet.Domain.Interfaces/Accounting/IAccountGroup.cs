using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccountGroup : IAccountGroupBase
    {
        AccountGroupType AccountGroupType { get; }
    }
}