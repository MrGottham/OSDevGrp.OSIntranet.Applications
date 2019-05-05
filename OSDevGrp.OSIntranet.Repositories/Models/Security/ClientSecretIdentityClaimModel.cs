using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class ClientSecretIdentityClaimModel : IdentityClaimModelBase
    {
        public virtual int ClientSecretIdentityClaimIdentifier { get; set; }

        public virtual int ClientSecretIdentityIdentifier { get; set; }

        public virtual ClientSecretIdentityModel ClientSecretIdentity { get; set; }
    }

    internal static class ClientSecretIdentityClaimModelExtensions
    {
        internal static ClientSecretIdentityClaimModel With(this ClientSecretIdentityClaimModel clientSecretIdentityClaimModel, ClientSecretIdentityModel clientSecretIdentity)
        {
            NullGuard.NotNull(clientSecretIdentityClaimModel, nameof(clientSecretIdentityClaimModel))
                .NotNull(clientSecretIdentity, nameof(clientSecretIdentity));

            clientSecretIdentityClaimModel.ClientSecretIdentityIdentifier = clientSecretIdentity.ClientSecretIdentityIdentifier;
            clientSecretIdentityClaimModel.ClientSecretIdentity = clientSecretIdentity;

            return clientSecretIdentityClaimModel;
        }

        internal static void CreateClientSecretIdentityClaimModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<ClientSecretIdentityClaimModel>(entity =>
            {
                entity.HasKey(e => e.ClientSecretIdentityClaimIdentifier);
                entity.Property(e => e.ClientSecretIdentityClaimIdentifier).IsRequired().HasAnnotation("MySQL:AutoIncrement", true);
                entity.Property(e => e.ClientSecretIdentityIdentifier).IsRequired();
                entity.Property(e => e.ClaimIdentifier).IsRequired();
                entity.Property(e => e.ClaimValue).IsRequired(false).IsUnicode().HasMaxLength(256);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.HasIndex(e => new {e.ClientSecretIdentityIdentifier, e.ClaimIdentifier}).IsUnique();
                entity.HasOne(e => e.ClientSecretIdentity).WithMany(e => e.ClientSecretIdentityClaims).HasForeignKey(e => e.ClientSecretIdentityClaimIdentifier).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Claim);
            });
        }
    }
}
