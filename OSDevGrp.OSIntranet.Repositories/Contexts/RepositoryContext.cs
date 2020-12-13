using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.Resolvers;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;
using OSDevGrp.OSIntranet.Repositories.Models.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Contacts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Security;

namespace OSDevGrp.OSIntranet.Repositories.Contexts
{
    internal class RepositoryContext : DbContext
    {
        #region Constructors

        public RepositoryContext()
        {
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets<RepositoryContext>()
                .Build();
            PrincipalResolver = new GenericPrincipalResolver();
            LoggerFactory = NullLoggerFactory.Instance;
        }

        public RepositoryContext(IConfiguration configuration, IPrincipalResolver principalResolver, ILoggerFactory loggerFactory)
        {
            NullGuard.NotNull(configuration, nameof(configuration))
                .NotNull(principalResolver, nameof(principalResolver))
                .NotNull(loggerFactory, nameof(loggerFactory));

            Configuration = configuration;
            PrincipalResolver = principalResolver;
            LoggerFactory = loggerFactory;
        }

        #endregion

        #region DbSets for Common data

        public DbSet<LetterHeadModel> LetterHeads { get; set; }

        #endregion

        #region DbSets for Contact data

        public DbSet<ContactSupplementModel> ContactSupplements { get; set; }

        public DbSet<ContactSupplementBindingModel> ContactSupplementBindings { get; set; }

        public DbSet<ContactGroupModel> ContactGroups { get; set; }

        public DbSet<CountryModel> Countries { get; set; }

        public DbSet<PostalCodeModel> PostalCodes { get; set; }

        #endregion

        #region DbSets for Accounting data

        public DbSet<AccountingModel> Accountings { get; set; }

        public DbSet<AccountModel> Accounts { get; set; }

        public DbSet<BudgetAccountModel> BudgetAccounts { get; set; }

        public DbSet<ContactAccountModel> ContactAccounts { get; set; }

        public DbSet<BasicAccountModel> BasicAccounts { get; set; }

        public DbSet<CreditInfoModel> CreditInfos { get; set; }

        public DbSet<BudgetInfoModel> BudgetInfos { get; set; }

        public DbSet<YearMonthModel> YearMonths { get; set; }

        public DbSet<AccountGroupModel> AccountGroups { get; set; }

        public DbSet<BudgetAccountGroupModel> BudgetAccountGroups { get; set; }

        public DbSet<PaymentTermModel> PaymentTerms { get; set; }

        #endregion

        #region DbSets for Security data

        public DbSet<UserIdentityModel> UserIdentities { get; set; }

        public DbSet<UserIdentityClaimModel> UserIdentityClaims { get; set; }

        public DbSet<ClientSecretIdentityModel> ClientSecretIdentities { get; set; }

        public DbSet<ClientSecretIdentityClaimModel> ClientSecretIdentityClaims { get; set; }

        public DbSet<ClaimModel> Claims { get; set; }

        #endregion

        #region Properties

        private IConfiguration Configuration { get; }

        private IPrincipalResolver PrincipalResolver { get; }

        private ILoggerFactory LoggerFactory { get; }

        #endregion

        #region Methods

        public override int SaveChanges()
        {
            string identityIdentifier = GetIdentityIdentifier(PrincipalResolver.GetCurrentPrincipal());

            AddAuditInformation(identityIdentifier);

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            string identityIdentifier = GetIdentityIdentifier(PrincipalResolver.GetCurrentPrincipal());

            AddAuditInformation(identityIdentifier);

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            string identityIdentifier = GetIdentityIdentifier(PrincipalResolver.GetCurrentPrincipal());

            AddAuditInformation(identityIdentifier);

            return await base.SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            string identityIdentifier = GetIdentityIdentifier(PrincipalResolver.GetCurrentPrincipal());

            AddAuditInformation(identityIdentifier);

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            NullGuard.NotNull(optionsBuilder, nameof(optionsBuilder));

            optionsBuilder.UseLoggerFactory(LoggerFactory)
                .UseMySQL(Configuration.GetConnectionString(ConnectionStringNames.IntranetName));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            CreateModelsForCommonData(modelBuilder);
            CreateModelsForContactData(modelBuilder);
            CreateModelsForAccountingData(modelBuilder);
            CreateModelsForSecurityData(modelBuilder);
        }

        private string GetIdentityIdentifier(IPrincipal currentPrincipal)
        {
            NullGuard.NotNull(currentPrincipal, nameof(currentPrincipal));

            return GetIdentityIdentifier(currentPrincipal.Identity);
        }

        private string GetIdentityIdentifier(IIdentity currentIdentity)
        {
            NullGuard.NotNull(currentIdentity, nameof(currentIdentity));

            return GetIdentityIdentifier(currentIdentity as ClaimsIdentity);
        }

        private string GetIdentityIdentifier(ClaimsIdentity currentClaimsIdentity)
        {
            NullGuard.NotNull(currentClaimsIdentity, nameof(currentClaimsIdentity));

            Claim identityIdentifierClaim = currentClaimsIdentity.FindFirst(ClaimHelper.ExternalUserIdentifierClaimType);
            if (identityIdentifierClaim != null)
            {
                return identityIdentifierClaim.Value;
            }

            identityIdentifierClaim = currentClaimsIdentity.FindFirst(ClaimHelper.FriendlyNameClaimType);
            if (identityIdentifierClaim != null)
            {
                return identityIdentifierClaim.Value;
            }

            identityIdentifierClaim = currentClaimsIdentity.FindFirst(ClaimTypes.Email);
            if (identityIdentifierClaim != null)
            {
                return identityIdentifierClaim.Value;
            }

            identityIdentifierClaim = currentClaimsIdentity.FindFirst(ClaimTypes.Name);
            return identityIdentifierClaim?.Value;
        }

        private void AddAuditInformation(string identityIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(identityIdentifier, nameof(identityIdentifier));

            foreach (EntityEntry entityEntry in ChangeTracker.Entries())
            {
                AuditModelBase auditModel = entityEntry.Entity as AuditModelBase;
                if (auditModel == null)
                {
                    continue;
                }

                DateTime utcNow = DateTime.UtcNow;
                if (entityEntry.State == EntityState.Added)
                {
                    auditModel.CreatedUtcDateTime = utcNow;
                    auditModel.CreatedByIdentifier = identityIdentifier;
                    auditModel.ModifiedUtcDateTime = utcNow;
                    auditModel.ModifiedByIdentifier = identityIdentifier;
                    continue;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    auditModel.ModifiedUtcDateTime = utcNow;
                    auditModel.ModifiedByIdentifier = identityIdentifier;
                }
            }
        }

        private void CreateModelsForCommonData(ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.CreateLetterHeadModel();
        }

        private void CreateModelsForContactData(ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.CreateContactSupplementModel();
            modelBuilder.CreateContactSupplementBindingModel();
            modelBuilder.CreateContactGroupModel();
            modelBuilder.CreateCountryModel();
            modelBuilder.CreatePostalCodeModel();
        }

        private void CreateModelsForAccountingData(ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.CreateAccountingModel();
            modelBuilder.CreateAccountModel();
            modelBuilder.CreateBudgetAccountModel();
            modelBuilder.CreateContactAccountModel();
            modelBuilder.CreateBasicAccountModel();
            modelBuilder.CreateCreditInfoModel();
            modelBuilder.CreateBudgetInfoModel();
            modelBuilder.CreateYearMonthModel();
            modelBuilder.CreateAccountGroupModel();
            modelBuilder.CreateBudgetAccountGroupModel();
            modelBuilder.CreatePaymentTermModel();
        }

        private void CreateModelsForSecurityData(ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.CreateUserIdentityModel();
            modelBuilder.CreateUserIdentityClaimModel();
            modelBuilder.CreateClientSecretIdentityModel();
            modelBuilder.CreateClientSecretIdentityClaimModel();
            modelBuilder.CreateClaimModel();
        }

        #endregion
    }
}