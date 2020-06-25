using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class ClaimModel : AuditModelBase
    {
        public virtual int ClaimIdentifier { get; set; }

        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }

        public virtual List<UserIdentityClaimModel> UserIdentityClaims { get; set; }

        public virtual List<ClientSecretIdentityClaimModel> ClientSecretIdentityClaims { get; set; }
    }

    internal static class ClaimModelExtensions
    {
        internal static Claim ToDomain(this ClaimModel claimModel)
        {
            NullGuard.NotNull(claimModel, nameof(claimModel));

            return ClaimHelper.CreateClaim(claimModel.ClaimType, claimModel.ClaimValue);
        }

        internal static void CreateClaimModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<ClaimModel>(entity =>
            {
                entity.HasKey(e => e.ClaimIdentifier);
                entity.Property(e => e.ClaimIdentifier).IsRequired().HasAnnotation("MySQL:AutoIncrement", true);
                entity.Property(e => e.ClaimType).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ClaimValue).IsRequired(false).IsUnicode().HasMaxLength(256);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.HasIndex(e => e.ClaimType).IsUnique();
            });
        }
    }
}