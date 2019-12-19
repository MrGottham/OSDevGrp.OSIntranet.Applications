using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public abstract class ContactGroupIdentificationCommandHandlerBase<T> : CommandHandlerTransactionalBase, ICommandHandler<T> where T : IContactGroupIdentificationCommand
    {
        #region Constructor

        protected ContactGroupIdentificationCommandHandlerBase(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            Validator = validator;
            ContactRepository = contactRepository;
        }

        #endregion

        #region Properties

        protected IValidator Validator { get; }

        protected IContactRepository ContactRepository { get; }

        #endregion

        #region Methods

        public async Task ExecuteAsync(T command)
        {
            NullGuard.NotNull(command, nameof(command));

            command.Validate(Validator, ContactRepository);

            await ManageRepositoryAsync(command);
        }

        protected abstract Task ManageRepositoryAsync(T command);

        #endregion
    }
}
