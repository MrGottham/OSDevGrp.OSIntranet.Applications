namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IBudgetAccount : IAccountBase<IBudgetAccount>
    {
        IBudgetAccountGroup BudgetAccountGroup { get; set; }
    }
}