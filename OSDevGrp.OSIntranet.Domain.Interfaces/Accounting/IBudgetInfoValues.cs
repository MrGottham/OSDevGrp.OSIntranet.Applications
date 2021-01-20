namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IBudgetInfoValues
    {
        decimal Budget { get; }

        decimal Posted { get; }

        decimal Available { get; }
    }
}