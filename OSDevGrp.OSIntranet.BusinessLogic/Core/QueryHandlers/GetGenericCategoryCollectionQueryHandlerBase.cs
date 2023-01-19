using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.QueryHandlers
{
    internal abstract class GetGenericCategoryCollectionQueryHandlerBase<TGenericCategory> : IQueryHandler<EmptyQuery, IEnumerable<TGenericCategory>> where TGenericCategory : IGenericCategory
    {
        #region Methods

        public async Task<IEnumerable<TGenericCategory>> QueryAsync(EmptyQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            IEnumerable<TGenericCategory> genericCategories = await ReadFromRepository();

            return genericCategories ?? Array.Empty<TGenericCategory>();
        }

        protected abstract Task<IEnumerable<TGenericCategory>> ReadFromRepository();

        #endregion
    }
}