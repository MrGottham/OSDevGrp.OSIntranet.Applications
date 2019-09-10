using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers
{
    public class GetLetterHeadCollectionQueryHandler : IQueryHandler<EmptyQuery, IEnumerable<ILetterHead>>
    {
        #region Private variables

        private readonly ICommonRepository _commonRepository;

        #endregion

        #region Constructor

        public GetLetterHeadCollectionQueryHandler(ICommonRepository commonRepository)
        {
            NullGuard.NotNull(commonRepository, nameof(commonRepository));

            _commonRepository = commonRepository;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<ILetterHead>> QueryAsync(EmptyQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return await _commonRepository.GetLetterHeadsAsync();
        }

        #endregion
    }
}