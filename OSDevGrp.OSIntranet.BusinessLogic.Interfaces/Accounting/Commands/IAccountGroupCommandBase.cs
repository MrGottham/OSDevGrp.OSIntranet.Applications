using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IAccountGroupCommandBase : IAccountGroupIdentificationCommandBase
    {
        string Name { get; set; }

        AccountGroupType AccountGroupType { get; set; }

        IAccountGroup ToDomain();
    }
}