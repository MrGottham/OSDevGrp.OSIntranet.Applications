using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class ClientSecretIdentityModel
    {
        public virtual int ClientSecretIdentityIdentifier { get; set; }

        public virtual string FriendlyName { get; set; }

        public virtual string ClientId { get; set; }

        public virtual string ClientSecret { get; set; }
    }

    internal static class ClientSecretIdentityModelExtensions
    {
        internal static IClientSecretIdentity ToDomain(this ClientSecretIdentityModel clientSecretIdentityModel)
        {
            return new ClientSecretIdentityBuilder(clientSecretIdentityModel.FriendlyName)
                .WithIdentifier(clientSecretIdentityModel.ClientSecretIdentityIdentifier)
                .WithClientId(clientSecretIdentityModel.ClientId)
                .WithClientSecret(clientSecretIdentityModel.ClientSecret)
                .Build();
        }
    }
}
