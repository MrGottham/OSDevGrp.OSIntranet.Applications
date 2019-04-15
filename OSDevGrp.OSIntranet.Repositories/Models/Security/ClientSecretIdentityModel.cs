using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class ClientSecretIdentityModel : AuditModelBase
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

            ClientSecretIdentityClaimModel latestClientSecretIdentityClaimModel = clientSecretIdentityModel.ClientSecretIdentityClaims.OrderByDescending(model => model.ModifiedUtcDateTime).FirstOrDefault();

            IClientSecretIdentity clientSecretIdentity = new ClientSecretIdentityBuilder(clientSecretIdentityModel.FriendlyName)
                .WithIdentifier(clientSecretIdentityModel.ClientSecretIdentityIdentifier)
                .WithClientId(clientSecretIdentityModel.ClientId)
                .WithClientSecret(clientSecretIdentityModel.ClientSecret)
                .AddClaims(claimCollection)
                .Build();


            if (latestClientSecretIdentityClaimModel == null || latestClientSecretIdentityClaimModel.ModifiedUtcDateTime > clientSecretIdentityModel.ModifiedUtcDateTime)
            {
                clientSecretIdentity.AddAuditInformations(clientSecretIdentityModel.CreatedUtcDateTime, clientSecretIdentityModel.CreatedByIdentifier, clientSecretIdentityModel.ModifiedUtcDateTime, clientSecretIdentityModel.ModifiedByIdentifier);
            }
            else
            {
                clientSecretIdentity.AddAuditInformations(clientSecretIdentityModel.CreatedUtcDateTime, clientSecretIdentityModel.CreatedByIdentifier, latestClientSecretIdentityClaimModel.ModifiedUtcDateTime, latestClientSecretIdentityClaimModel.ModifiedByIdentifier);
            }

            return clientSecretIdentity;
        }
    }
}
