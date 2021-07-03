using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;

namespace OSDevGrp.OSIntranet.Repositories.Events
{
    internal abstract class DbContextEventBase : IEvent
    {
        #region Constructor

        protected DbContextEventBase(DbContext dbContext)
        {
            NullGuard.NotNull(dbContext, nameof(dbContext));

            DbContext = dbContext;
        }

        #endregion

        #region Properties

        internal DbContext DbContext { get; }

        #endregion

        #region Methods

        internal bool FromSameDbContext(DbContext dbContext)
        {
            NullGuard.NotNull(dbContext, nameof(dbContext));

            return DbContext.Equals(dbContext);
        }

        #endregion
    }
}