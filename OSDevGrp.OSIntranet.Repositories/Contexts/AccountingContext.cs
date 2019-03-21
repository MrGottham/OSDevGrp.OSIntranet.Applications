using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Contexts
{
    internal class AccountingContext : DbContext
    {
        #region Private variables

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructors

        public AccountingContext()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<AccountingContext>()
                .Build();
        }

        public AccountingContext(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            _configuration = configuration;
        }

        #endregion

        #region Properties

        public DbSet<AccountGroupModel> AccountGroups { get; set; }

        public DbSet<BudgetAccountGroupModel> BudgetAccountGroups { get; set; }

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

            modelBuilder.Entity<AccountGroupModel>(entity =>
            {
                entity.HasKey(e => e.AccountGroupIdentifier);
                entity.Property(e => e.AccountGroupIdentifier).IsRequired();
                entity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.AccountGroupType).IsRequired();
            });

            modelBuilder.Entity<BudgetAccountGroupModel>(entity =>
            {
                entity.HasKey(e => e.BudgetAccountGroupIdentifier);
                entity.Property(e => e.BudgetAccountGroupIdentifier).IsRequired();
                entity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(256);
            });
        }

        #endregion
    }
}