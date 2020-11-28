namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccountCollectionValues
    {
        decimal Assets { get; }

        decimal Liabilities { get; }
    }
}