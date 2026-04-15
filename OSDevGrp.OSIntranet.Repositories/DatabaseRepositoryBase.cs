using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories
{
    internal abstract class DatabaseRepositoryBase<TDbContext> : RepositoryBase where TDbContext : DbContext
    {
        #region Constructor

        protected DatabaseRepositoryBase(TDbContext dbContext, IConverterFactory converterFactory, ILoggerFactory loggerFactory) 
            : base(converterFactory, loggerFactory)
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