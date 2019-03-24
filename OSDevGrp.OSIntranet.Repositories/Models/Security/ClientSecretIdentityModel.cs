using System.Collections.Generic;
using System.Security.Claims;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Converters;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class ClientSecretIdentityModel
    {
        public virtual int ClientSecretIdentityIdentifier { get; set; }

        public virtual string FriendlyName { get; set; }

        public virtual string ClientId { get; set; }

        public virtual string ClientSecret { get; set; }

        public virtual List<ClientSecretIdentityClaimModel> ClientSecretIdentityClaims { get; set; }
    }

    internal static class ClientSecretIdentityModelExtensions
    {
        internal static IClientSecretIdentity ToDomain(this ClientSecretIdentityModel clientSecretIdentityModel)
        {
            NullGuard.NotNull(clientSecretIdentityModel, nameof(clientSecretIdentityModel));

            IConverter securityModelConverter = new SecurityModelConverter();
            IEnumerable<Claim> claimCollection = securityModelConverter.Convert<IEnumerable<ClientSecretIdentityClaimModel>, IEnumerable<Claim>>(clientSecretIdentityModel.ClientSecretIdentityClaims);

            return new ClientSecretIdentityBuilder(clientSecretIdentityModel.FriendlyName)
                .WithIdentifier(clientSecretIdentityModel.ClientSecretIdentityIdentifier)
                .WithClientId(clientSecretIdentityModel.ClientId)
                .WithClientSecret(clientSecretIdentityModel.ClientSecret)
                .AddClaims(claimCollection)
                .Build();
        }
    }
}
