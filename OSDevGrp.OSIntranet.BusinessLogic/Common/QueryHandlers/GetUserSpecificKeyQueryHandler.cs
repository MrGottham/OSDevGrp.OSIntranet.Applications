using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers
{
    public class GetUserSpecificKeyQueryHandler : GetKeyQueryHandlerBase<IGetUserSpecificKeyQuery>
    {
        #region Constructor

        public GetUserSpecificKeyQueryHandler(IValidator validator, IKeyGenerator keyGenerator) 
            : base(validator, keyGenerator)
        {
        }

        #endregion

        #region Methods

        protected override Task<string> GenerateKey(IKeyGenerator keyGenerator, IEnumerable<string> keyElementCollection)
        {
            NullGuard.NotNull(keyGenerator, nameof(keyGenerator))
                .NotNull(keyElementCollection, nameof(keyElementCollection));

            return keyGenerator.GenerateUserSpecificKeyAsync(keyElementCollection);
        }

        #endregion
    }
}