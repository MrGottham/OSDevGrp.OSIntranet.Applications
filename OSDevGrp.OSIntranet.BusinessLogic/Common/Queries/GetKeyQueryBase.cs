using System.Collections.Generic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Queries
{
    public abstract class GetKeyQueryBase : IGetKeyQuery
    {
        #region Properties

        public IEnumerable<string> KeyElementCollection { get; set; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return validator.Object.ShouldNotBeNull(KeyElementCollection, GetType(), nameof(KeyElementCollection))
                .Enumerable.ShouldContainItems(KeyElementCollection, GetType(), nameof(KeyElementCollection));
        }

        #endregion
    }
}