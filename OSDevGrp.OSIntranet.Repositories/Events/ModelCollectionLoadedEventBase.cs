using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Repositories.Events
{
    internal abstract class ModelCollectionLoadedEventBase<TModel> : DbContextEventBase where TModel : class
    {
        #region Constructor

        protected ModelCollectionLoadedEventBase(DbContext dbContext, IReadOnlyCollection<TModel> modelCollection) 
            : base(dbContext)
        {
            NullGuard.NotNull(modelCollection, nameof(modelCollection));

            ModelCollection = modelCollection;
        }

        #endregion

        #region Properties

        internal IReadOnlyCollection<TModel> ModelCollection { get; }

        #endregion
    }
}