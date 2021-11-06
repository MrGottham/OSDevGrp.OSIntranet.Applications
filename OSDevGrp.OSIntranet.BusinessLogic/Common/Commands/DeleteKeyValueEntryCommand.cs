using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Commands
{
    public class DeleteKeyValueEntryCommand : KeyValueEntryIdentificationCommandBase, IDeleteKeyValueEntryCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, commonRepository)
                .Object.ShouldBeKnownValue(Key, key => Task.Run(async () => await GetKeyValueEntryAsync(commonRepository) != null), GetType(), nameof(Key))
                .Object.ShouldBeDeletable(Key, key => GetKeyValueEntryAsync(commonRepository), GetType(), nameof(Key));
        }

        #endregion
    }
}