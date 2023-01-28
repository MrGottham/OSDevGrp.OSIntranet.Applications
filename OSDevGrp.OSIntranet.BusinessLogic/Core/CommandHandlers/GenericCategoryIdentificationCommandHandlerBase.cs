using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers
{
    internal abstract class GenericCategoryIdentificationCommandHandlerBase<TGenericCategoryIdentificationCommand, TGenericCategory> : CommandHandlerTransactionalBase, ICommandHandler<TGenericCategoryIdentificationCommand> where TGenericCategoryIdentificationCommand : IGenericCategoryIdentificationCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
        #region Constructor

        protected GenericCategoryIdentificationCommandHandlerBase(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            Validator = validator;
        }

        #endregion

        #region Properties

        protected IValidator Validator { get; }

        #endregion

        #region Methods

        public async Task ExecuteAsync(TGenericCategoryIdentificationCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            command.Validate(Validator, GetGenericCategoryAsync);

            await ManageRepositoryAsync(command);
        }

        protected abstract Task<TGenericCategory> GetGenericCategoryAsync(int number);

        protected abstract Task ManageRepositoryAsync(TGenericCategoryIdentificationCommand command);

        #endregion
    }
}