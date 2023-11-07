using OSDevGrp.OSIntranet.BusinessLogic.Core.QueryHandlers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers
{
    internal class GetNationalityCollectionQueryHandler : GetGenericCategoryCollectionQueryHandlerBase<INationality>
    {
        #region Private variables

        private readonly ICommonRepository _commonRepository;

        #endregion

        #region Constructor

        public GetNationalityCollectionQueryHandler(ICommonRepository commonRepository)
        {
            NullGuard.NotNull(commonRepository, nameof(commonRepository));

            _commonRepository = commonRepository;
        }

        #endregion

        #region Methods

        protected override Task<IEnumerable<INationality>> ReadFromRepository() => _commonRepository.GetNationalitiesAsync();

        #endregion
    }
}