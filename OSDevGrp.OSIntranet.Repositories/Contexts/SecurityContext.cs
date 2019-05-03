using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Repositories.Models.Security;

namespace OSDevGrp.OSIntranet.Repositories.Contexts
{
    internal class SecurityContext : RepositoryContextBase
    {
        #region Constructors

        public SecurityContext()
        {
        }

        public SecurityContext(IConfiguration configuration, IPrincipalResolver principalResolver)
            : base(configuration, principalResolver)
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

            modelBuilder.CreateUserIdentityModel();
            modelBuilder.CreateUserIdentityClaimModel();
            modelBuilder.CreateClientSecretIdentityModel();
            modelBuilder.CreateClientSecretIdentityClaimModel();
            modelBuilder.CreateClaimModel();
        }

        #endregion
    }
}
