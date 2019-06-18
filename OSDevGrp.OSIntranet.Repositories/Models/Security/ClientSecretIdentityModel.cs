using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Contexts;
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
        internal static IClientSecretIdentity ToDomain(this ClientSecretIdentityModel clientSecretIdentityModel, IConverter securityModelConverter)
        {
            NullGuard.NotNull(clientSecretIdentityModel, nameof(clientSecretIdentityModel))
                .NotNull(securityModelConverter, nameof(securityModelConverter));

            IEnumerable<Claim> claimCollection = securityModelConverter.Convert<IEnumerable<ClientSecretIdentityClaimModel>, IEnumerable<Claim>>(clientSecretIdentityModel.ClientSecretIdentityClaims);

            ClientSecretIdentityClaimModel latestClientSecretIdentityClaimModel = clientSecretIdentityModel.ClientSecretIdentityClaims.OrderByDescending(model => model.ModifiedUtcDateTime).FirstOrDefault();

            IClientSecretIdentity clientSecretIdentity = new ClientSecretIdentityBuilder(clientSecretIdentityModel.FriendlyName)
                .WithIdentifier(clientSecretIdentityModel.ClientSecretIdentityIdentifier)
                .WithClientId(clientSecretIdentityModel.ClientId)
                .WithClientSecret(clientSecretIdentityModel.ClientSecret)
                .AddClaims(claimCollection)
                .Build();


            if (latestClientSecretIdentityClaimModel == null || latestClientSecretIdentityClaimModel.ModifiedUtcDateTime < clientSecretIdentityModel.ModifiedUtcDateTime)
            {
                clientSecretIdentity.AddAuditInformation(clientSecretIdentityModel.CreatedUtcDateTime, clientSecretIdentityModel.CreatedByIdentifier, clientSecretIdentityModel.ModifiedUtcDateTime, clientSecretIdentityModel.ModifiedByIdentifier);
            }
            else
            {
                clientSecretIdentity.AddAuditInformation(clientSecretIdentityModel.CreatedUtcDateTime, clientSecretIdentityModel.CreatedByIdentifier, latestClientSecretIdentityClaimModel.ModifiedUtcDateTime, latestClientSecretIdentityClaimModel.ModifiedByIdentifier);
            }

            return clientSecretIdentity;
        }

        internal static ClientSecretIdentityModel WithDefaultIdentifier(this ClientSecretIdentityModel clientSecretIdentityModel)
        {
            NullGuard.NotNull(clientSecretIdentityModel, nameof(clientSecretIdentityModel));

            clientSecretIdentityModel.ClientSecretIdentityIdentifier = default(int);
            
            return clientSecretIdentityModel;
        }

        internal static ClientSecretIdentityModel With(this ClientSecretIdentityModel clientSecretIdentityModel, IEnumerable<Claim> claimCollection, SecurityContext context, IConverter securityModelConverter)
        {
            NullGuard.NotNull(clientSecretIdentityModel, nameof(clientSecretIdentityModel))
                .NotNull(claimCollection, nameof(claimCollection))
                .NotNull(context, nameof(context))
                .NotNull(securityModelConverter, nameof(securityModelConverter));

            IList<ClaimModel> claimModelCollection = context.Claims.ToList();

            clientSecretIdentityModel.ClientSecretIdentityClaims = claimCollection.AsParallel()
                .Where(claim => claimModelCollection.Any(c => c.ClaimType == claim.Type))
                .Select(claim => securityModelConverter.Convert<Claim, ClientSecretIdentityClaimModel>(claim).With(clientSecretIdentityModel).With(claimModelCollection.Single(c => c.ClaimType == claim.Type)))
                .ToList();

            return clientSecretIdentityModel;
        }

        internal static void CreateClientSecretIdentityModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<ClientSecretIdentityModel>(entity =>
            {
                entity.HasKey(e => e.ClientSecretIdentityIdentifier);
                entity.Property(e => e.ClientSecretIdentityIdentifier).IsRequired().HasAnnotation("MySQL:AutoIncrement", true);
                entity.Property(e => e.FriendlyName).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ClientId).IsRequired().IsUnicode().HasMaxLength(32);
                entity.Property(e => e.ClientSecret).IsRequired().IsUnicode().HasMaxLength(32);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.HasIndex(e => e.FriendlyName).IsUnique();
                entity.HasIndex(e => e.ClientId).IsUnique();
            });
        }
    }
}
