using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class UserIdentityClaimModel : IdentityClaimModelBase
    {
        public virtual int UserIdentityClaimIdentifier { get; set; }

        public virtual int UserIdentityIdentifier { get; set; }

        public virtual UserIdentityModel UserIdentity { get; set; }
    }

    internal static class UserIdentityClaimModelExtensions
    {
        internal static void CreateUserIdentityClaimModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<UserIdentityClaimModel>(entity =>
            {
                entity.HasKey(e => e.UserIdentityClaimIdentifier);
                entity.Property(e => e.UserIdentityClaimIdentifier).IsRequired().HasAnnotation("MySQL:AutoIncrement", true);
                entity.Property(e => e.UserIdentityIdentifier).IsRequired();
                entity.Property(e => e.ClaimIdentifier).IsRequired();
                entity.Property(e => e.ClaimValue).IsRequired(false).IsUnicode().HasMaxLength(256);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.HasIndex(e => new {e.UserIdentityIdentifier, e.ClaimIdentifier}).IsUnique();
                entity.HasOne(e => e.UserIdentity).WithMany(e => e.UserIdentityClaims).HasForeignKey(e => e.UserIdentityIdentifier).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Claim);
            });
        }
    }
}
