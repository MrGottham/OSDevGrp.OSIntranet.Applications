using OSDevGrp.OSIntranet.BusinessLogic.Core.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.Queries
{
    internal abstract class GenericCategoryIdentificationQueryBase : IGenericCategoryIdentificationQuery
    {
        #region Constructor

        protected GenericCategoryIdentificationQueryBase(int number)
        {
            Number = number;
        }

        #endregion

        #region Properties

        public int Number { get; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return validator.ValidateGenericCategoryIdentifier(Number, GetType(), nameof(Number));
        }

        #endregion
    }
}