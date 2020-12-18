using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface ICreditInfoCommand : IInfoCommand
    {
        decimal Credit { get; set; }

        ICreditInfo ToDomain(IAccount account);
    }
}