using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers
{
    public class PullKeyValueEntryQueryHandler : IQueryHandler<IPullKeyValueEntryQuery, IKeyValueEntry>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly ICommonRepository _commonRepository;

        #endregion

        #region Constructor

        public PullKeyValueEntryQueryHandler(IValidator validator, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(commonRepository, nameof(commonRepository));

            _validator = validator;
            _commonRepository = commonRepository;
        }

        #endregion

        #region Methods

        public Task<IKeyValueEntry> QueryAsync(IPullKeyValueEntryQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(_validator);

            return _commonRepository.PullKeyValueEntryAsync(query.Key);
        }

        #endregion
    }
}