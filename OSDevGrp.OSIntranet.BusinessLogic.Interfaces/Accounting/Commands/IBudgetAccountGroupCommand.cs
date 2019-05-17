using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IBudgetAccountGroupCommand : IAccountGroupIdentificationCommand
    {
        string Name { get; set; }

        IBudgetAccountGroup ToDomain();
    }
}