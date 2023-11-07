using OSDevGrp.OSIntranet.BusinessLogic.Core.QueryHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers
{
    internal class GetLanguageQueryHandler : GetGenericCategoryQueryHandlerBase<IGetLanguageQuery, ILanguage>
    {
        #region Private variables

        private readonly ICommonRepository _commonRepository;

        #endregion

        #region Constructor

        public GetLanguageQueryHandler(IValidator validator, ICommonRepository commonRepository) 
            : base(validator)
        {
            NullGuard.NotNull(commonRepository, nameof(commonRepository));

            _commonRepository = commonRepository;
        }

        #endregion

        #region Methods

        protected override async Task<ILanguage> ReadFromRepository(IGetLanguageQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return await _commonRepository.GetLanguageAsync(query.Number);
        }

        #endregion
    }
}