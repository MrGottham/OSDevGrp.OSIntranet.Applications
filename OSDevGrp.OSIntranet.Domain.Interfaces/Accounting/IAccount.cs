namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccount : IAccountBase<IAccount>
    {
        IAccountGroup AccountGroup { get; set; }
    }
}