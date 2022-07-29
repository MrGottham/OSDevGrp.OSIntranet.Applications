using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Repositories
{
    internal abstract class DatabaseRepositoryBase<TDbContext> : RepositoryBase where TDbContext : DbContext
    {
        #region Constructor

        protected DatabaseRepositoryBase(TDbContext dbContext, IConfiguration configuration, ILoggerFactory loggerFactory) 
            : base(configuration, loggerFactory)
        {
            NullGuard.NotNull(dbContext, nameof(dbContext));

            DbContext = dbContext;
        }

        #endregion

        #region Properties

        protected TDbContext DbContext { get; }

        #endregion
    }
}