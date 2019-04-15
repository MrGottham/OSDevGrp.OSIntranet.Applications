using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Security;

namespace OSDevGrp.OSIntranet.Repositories.Contexts
{
    internal class SecurityContext : RepositoryContextBase
    {
        #region Constructors

        public SecurityContext()
        {
        }

        public SecurityContext(IConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion

        #region Properties

        public DbSet<UserIdentityModel> UserIdentities { get; set; }

        public DbSet<UserIdentityClaimModel> UserIdentityClaims { get; set; }

        public DbSet<ClientSecretIdentityModel> ClientSecretIdentities { get; set; }

        public DbSet<ClientSecretIdentityClaimModel> ClientSecretIdentityClaims { get; set; }

        public DbSet<ClaimModel> Claims { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserIdentityModel>(entity =>
            {
                entity.HasKey(e => e.UserIdentityIdentifier);
                entity.Property(e => e.UserIdentityIdentifier).IsRequired().HasAnnotation("MySQL:AutoIncrement", true);
                entity.Property(e => e.ExternalUserIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.HasIndex(e => e.ExternalUserIdentifier).IsUnique();
            });

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

        #endregion
    }
}
