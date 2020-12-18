using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IBudgetInfoCommand : IInfoCommand
    {
        decimal Income { get; set; }
        
        decimal Expenses { get; set; }

        IBudgetInfo ToDomain(IBudgetAccount budgetAccount);
    }
}