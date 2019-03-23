using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Security;

namespace OSDevGrp.OSIntranet.Repositories.Contexts
{
    internal class SecurityContext : DbContext
    {
        #region Private variables

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructors

        public SecurityContext()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<AccountingContext>()
                .Build();
        }

        public SecurityContext(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            _configuration = configuration;
        }

        #endregion

        #region Properties

        public DbSet<UserIdentityModel> UserIdentities { get; set; }

        public DbSet<ClientSecretIdentityModel> ClientSecretIdentities { get; set; }

        #endregion

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_configuration.GetConnectionString(ConnectionStringNames.IntranetName));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserIdentityModel>(entity =>
            {
                entity.HasKey(e => e.UserIdentityIdentifier);
                entity.Property(e => e.UserIdentityIdentifier).IsRequired().HasAnnotation("MySQL:AutoIncrement", true);
                entity.Property(e => e.ExternalUserIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.HasIndex(e => e.ExternalUserIdentifier).IsUnique();
            });

            modelBuilder.Entity<ClientSecretIdentityModel>(entity =>
            {
                entity.HasKey(e => e.ClientSecretIdentityIdentifier);
                entity.Property(e => e.ClientSecretIdentityIdentifier).IsRequired().HasAnnotation("MySQL:AutoIncrement", true);
                entity.Property(e => e.FriendlyName).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ClientId).IsRequired().IsUnicode().HasMaxLength(32);
                entity.Property(e => e.ClientSecret).IsRequired().IsUnicode().HasMaxLength(32);
                entity.HasIndex(e => e.FriendlyName).IsUnique();
                entity.HasIndex(e => e.ClientId).IsUnique();
            });
        }

        #endregion
    }
}
