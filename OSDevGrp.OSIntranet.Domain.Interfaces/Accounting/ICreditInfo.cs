namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface ICreditInfo : IInfo<ICreditInfo>
    {
        IAccount Account { get; }

        decimal Credit { get; set; }

        decimal Balance { get; }
    }
}