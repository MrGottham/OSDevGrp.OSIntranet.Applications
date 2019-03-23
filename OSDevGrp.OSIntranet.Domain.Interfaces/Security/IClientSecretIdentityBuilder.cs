namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IClientSecretIdentityBuilder
    {
        IClientSecretIdentityBuilder WithIdentifier(int identifier);

        IClientSecretIdentityBuilder WithClientId(string clientId);

        IClientSecretIdentityBuilder WithClientSecret(string clientSecret);

        IClientSecretIdentity Build();
    }
}
