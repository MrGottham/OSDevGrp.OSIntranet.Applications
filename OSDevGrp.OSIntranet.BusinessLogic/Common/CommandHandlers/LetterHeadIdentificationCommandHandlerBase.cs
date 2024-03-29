using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers
{
    public abstract class LetterHeadIdentificationCommandHandlerBase<T> : CommandHandlerTransactionalBase, ICommandHandler<T> where T : ILetterHeadIdentificationCommand
    {
        #region Constructor

        protected LetterHeadIdentificationCommandHandlerBase(IValidator validator, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(commonRepository, nameof(commonRepository));

            Validator = validator;
            CommonRepository = commonRepository;
        }

        #endregion

        #region Properties

        protected IValidator Validator { get; }

        protected ICommonRepository CommonRepository { get; }

        #endregion

        #region Methods

        public async Task ExecuteAsync(T command)
        {
            NullGuard.NotNull(command, nameof(command));

            command.Validate(Validator, CommonRepository);

            await ManageRepositoryAsync(command);
        }

        protected abstract Task ManageRepositoryAsync(T command);

        #endregion
    }
}