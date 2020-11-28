namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IContactInfo : IInfo<IContactInfo>
    {
        IContactAccount ContactAccount { get; }

        decimal Balance { get; }
    }
}