using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Repositories.Models.Common;

namespace OSDevGrp.OSIntranet.Repositories.Contexts
{
    internal class CommonContext : RepositoryContextBase
    {
        #region Constructors

        public CommonContext()
        {
        }

        public CommonContext(IConfiguration configuration, IPrincipalResolver principalResolver)
            : base(configuration, principalResolver)
        {
        }

        #endregion

        #region Properties

        public DbSet<LetterHeadModel> LetterHeads { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            modelBuilder.CreateLetterHeadModel();
        }

        #endregion
    }
}