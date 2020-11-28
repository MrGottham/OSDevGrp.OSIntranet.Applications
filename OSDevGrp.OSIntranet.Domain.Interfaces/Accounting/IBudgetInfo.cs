namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IBudgetInfo : IInfo<IBudgetInfo>, IBudgetInfoValues
    {
        IBudgetAccount BudgetAccount { get; }

        decimal Income { get; set; }

        decimal Expenses { get; set; }
    }
}