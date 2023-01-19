using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.QueryHandlers
{
    internal abstract class GetGenericCategoryQueryHandlerBase<TGenericCategoryIdentificationQuery, TGenericCategory> : IQueryHandler<TGenericCategoryIdentificationQuery, TGenericCategory> where TGenericCategoryIdentificationQuery : IGenericCategoryIdentificationQuery where TGenericCategory : IGenericCategory
    {
        #region Constructor

        protected GetGenericCategoryQueryHandlerBase(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            Validator = validator;
        }

        #endregion

        #region Properties

        protected IValidator Validator { get; }

        #endregion

        #region Methods

        public async Task<TGenericCategory> QueryAsync(TGenericCategoryIdentificationQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(Validator);

            return await ReadFromRepository(query);
        }

        protected abstract Task<TGenericCategory> ReadFromRepository(TGenericCategoryIdentificationQuery query);

        #endregion
    }
}