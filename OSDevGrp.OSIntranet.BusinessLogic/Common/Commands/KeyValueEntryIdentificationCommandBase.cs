using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    public abstract class KeyValueEntryIdentificationCommandBase : IKeyValueEntryIdentificationCommand
    {
        #region Private variables

        private IKeyValueEntry _keyValueEntry;

        #endregion

        #region Properties

        public string Key { get; set; }

        #endregion

        #region Methods

        public virtual IValidator Validate(IValidator validator, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(commonRepository, nameof(commonRepository));

            return validator.ValidateKeyValueEntryKey(Key, GetType(), nameof(Key));
        }

        protected Task<IKeyValueEntry> GetKeyValueEntryAsync(ICommonRepository commonRepository)
        {
            NullGuard.NotNull(commonRepository, nameof(commonRepository));

            return Task.FromResult(Key.GetKeyValueEntry(commonRepository, ref _keyValueEntry));
        }

        #endregion
    }
}