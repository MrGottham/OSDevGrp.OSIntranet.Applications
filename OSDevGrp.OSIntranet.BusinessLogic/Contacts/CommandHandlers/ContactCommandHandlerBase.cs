using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public abstract class ContactCommandHandlerBase<TCommand> : CommandHandlerTransactionalBase, ICommandHandler<TCommand> where TCommand : IContactCommand
    {
        #region Constructor

        protected ContactCommandHandlerBase(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository, IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(microsoftGraphRepository, nameof(microsoftGraphRepository))
                .NotNull(contactRepository, nameof(contactRepository))
                .NotNull(accountingRepository, nameof(accountingRepository));

            Validator = validator;
            MicrosoftGraphRepository = microsoftGraphRepository;
            ContactRepository = contactRepository;
            AccountingRepository = accountingRepository;
        }

        #endregion

        #region Properties

        protected IValidator Validator { get; }

        protected IMicrosoftGraphRepository MicrosoftGraphRepository { get; }

        protected IContactRepository ContactRepository { get; }

        protected IAccountingRepository AccountingRepository { get; }

        #endregion

        #region Methods

        public async Task ExecuteAsync(TCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            command.Validate(Validator, MicrosoftGraphRepository, ContactRepository, AccountingRepository);

            await ManageRepositoryAsync(command, command.ToToken());
        }

        protected abstract Task ManageRepositoryAsync(TCommand command, IRefreshableToken token);

        #endregion
    }
}