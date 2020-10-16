using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IAccountDataCommand : IAccountCoreDataCommand<IAccount>
    {
        int AccountGroupNumber { get; set; }
    }
}