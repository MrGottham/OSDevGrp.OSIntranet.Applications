namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IUserIdentityBuilder
    {
        IUserIdentityBuilder WithIdentifier(int identifier);

        IUserIdentity Build();
    }
}
