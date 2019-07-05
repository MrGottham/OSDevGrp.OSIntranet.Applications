using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        public AccountingContext(IConfiguration configuration, IPrincipalResolver principalResolver, ILoggerFactory loggerFactory)
            : base(configuration, principalResolver, loggerFactory)
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

            modelBuilder.CreateAccountGroupModel();
            modelBuilder.CreateBudgetAccountGroupModel();
        }

        #endregion
    }
}