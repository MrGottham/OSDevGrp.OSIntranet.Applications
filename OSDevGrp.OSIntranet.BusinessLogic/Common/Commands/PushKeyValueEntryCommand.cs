using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    public class PushKeyValueEntryCommand : KeyValueEntryIdentificationCommandBase, IPushKeyValueEntryCommand
    {
        #region Properties

        public object Value { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, commonRepository)
                .Object.ShouldNotBeNull(Value, GetType(), nameof(Value));
        }

        public IKeyValueEntry ToDomain()
        {
            return KeyValueEntry.Create(Key, Value);
        }

        #endregion
    }
}