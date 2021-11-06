using OSDevGrp.OSIntranet.BusinessLogic.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Queries
{
    public abstract class KeyValueEntryIdentificationQueryBase : IKeyValueEntryIdentificationQuery
    {
        #region Properties

        public string Key { get; set; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return validator.ValidateKeyValueEntryKey(Key, GetType(), nameof(Key));
        }

        #endregion
    }
}