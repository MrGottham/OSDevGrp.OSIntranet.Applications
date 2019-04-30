using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.Resolvers;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Contexts
{
    internal abstract class RepositoryContextBase : DbContext
    {
        #region Constructors

        protected RepositoryContextBase()
        {
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets<AccountingContext>()
                .Build();
            PrincipalResolver = new ThreadPrincipalResolver();
        }

        protected RepositoryContextBase(IConfiguration configuration, IPrincipalResolver principalResolver)
        {
            NullGuard.NotNull(configuration, nameof(configuration))
                .NotNull(principalResolver, nameof(principalResolver));

            Configuration = configuration;
            PrincipalResolver = principalResolver;
        }

        #endregion

        #region Properties

        protected IConfiguration Configuration { get; }

        protected IPrincipalResolver PrincipalResolver { get; }

        #endregion

        #region Methods

        public override int SaveChanges()
        {
            string identityIdentifier = GetIdentityIdentifier(PrincipalResolver.GetCurrentPrincipal());

            AddAuditInformations(identityIdentifier);

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            string identityIdentifier = GetIdentityIdentifier(PrincipalResolver.GetCurrentPrincipal());

            AddAuditInformations(identityIdentifier);

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            string identityIdentifier = GetIdentityIdentifier(PrincipalResolver.GetCurrentPrincipal());

            AddAuditInformations(identityIdentifier);

            return await base.SaveChangesAsync(cancellationToken);
        }

        public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            string identityIdentifier = GetIdentityIdentifier(PrincipalResolver.GetCurrentPrincipal());

            AddAuditInformations(identityIdentifier);

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            NullGuard.NotNull(optionsBuilder, nameof(optionsBuilder));

            optionsBuilder.UseMySQL(Configuration.GetConnectionString(ConnectionStringNames.IntranetName));
        }

        private string GetIdentityIdentifier(IPrincipal currentPricipal)
        {
            NullGuard.NotNull(currentPricipal, nameof(currentPricipal));

            return GetIdentityIdentifier(currentPricipal.Identity);
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
            if (identityIdentifierClaim != null)
            {
                return identityIdentifierClaim.Value;
            }

            return null;
        }

        private void AddAuditInformations(string identityIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(identityIdentifier, nameof(identityIdentifier));

            foreach (EntityEntry entityEntry in ChangeTracker.Entries())
            {
                AuditModelBase auditModel = entityEntry.Entity as AuditModelBase;
                if (auditModel == null)
                {
                    continue;
                }

                if (entityEntry.State == EntityState.Added)
                {
                    auditModel.CreatedUtcDateTime = DateTime.UtcNow;
                    auditModel.CreatedByIdentifier = identityIdentifier;
                }

                auditModel.ModifiedUtcDateTime = DateTime.UtcNow;
                auditModel.ModifiedByIdentifier = identityIdentifier;
            }
        }

        #endregion
    }
}