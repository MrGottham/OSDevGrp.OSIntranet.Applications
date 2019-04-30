using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Contexts
{
    internal class AccountingContext : RepositoryContextBase
    {
        #region Constructors

        public AccountingContext()
        {
        }

        public AccountingContext(IConfiguration configuration, IPrincipalResolver principalResolver)
            : base(configuration, principalResolver)
        {
        }

        #endregion

        #region Properties

        public DbSet<AccountGroupModel> AccountGroups { get; set; }

        public DbSet<BudgetAccountGroupModel> BudgetAccountGroups { get; set; }

        #endregion

        #region Methods

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
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
            });

            modelBuilder.Entity<BudgetAccountGroupModel>(entity =>
            {
                entity.HasKey(e => e.BudgetAccountGroupIdentifier);
                entity.Property(e => e.BudgetAccountGroupIdentifier).IsRequired();
                entity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
            });
        }

        #endregion
    }
}