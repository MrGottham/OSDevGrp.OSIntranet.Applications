using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers
{
    public abstract class GetKeyQueryHandlerBase<T> : IQueryHandler<T, string> where T : IGetKeyQuery
    {
        #region Properties

        private readonly IValidator _validator;
        private readonly IKeyGenerator _keyGenerator;

        #endregion

        #region Constructor

        protected GetKeyQueryHandlerBase(IValidator validator, IKeyGenerator keyGenerator)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(keyGenerator, nameof(keyGenerator));

            _validator = validator;
            _keyGenerator = keyGenerator;
        }

        #endregion

        #region Methods

        public Task<string> QueryAsync(T query)
        {
            NullGuard.NotNull(query, nameof(query));

            query.Validate(_validator);

            return GenerateKey(_keyGenerator, query.KeyElementCollection);
        }

        protected abstract Task<string> GenerateKey(IKeyGenerator keyGenerator, IEnumerable<string> keyElementCollection);

        #endregion
    }
}