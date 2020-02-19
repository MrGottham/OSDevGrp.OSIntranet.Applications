using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;
using OSDevGrp.OSIntranet.Repositories.Models.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Contacts;

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

        public DbSet<AccountingModel> Accountings { get; set; }

        public DbSet<LetterHeadModel> LetterHeads { get; set; }

        public DbSet<AccountGroupModel> AccountGroups { get; set; }

        public DbSet<BudgetAccountGroupModel> BudgetAccountGroups { get; set; }

        public DbSet<PaymentTermModel> PaymentTerms { get; set; }

        public DbSet<ContactSupplementModel> ContactSupplements { get; set; }

        public DbSet<ContactSupplementBindingModel> ContactSupplementBindings { get; set; }

        public DbSet<ContactGroupModel> ContactGroups { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            modelBuilder.CreateAccountingModel();
            modelBuilder.CreateLetterHeadModel();
            modelBuilder.CreateAccountGroupModel();
            modelBuilder.CreateBudgetAccountGroupModel();
            modelBuilder.CreatePaymentTermModel();
            modelBuilder.CreateContactSupplementModel();
            modelBuilder.CreateContactSupplementBindingModel();
            modelBuilder.CreateContactGroupModel();
        }

        #endregion
    }
}