using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public class CreateContactCommandHandler : ContactCommandHandlerBase<ICreateContactCommand>
    {
        #region Constructor

        public CreateContactCommandHandler(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository, IAccountingRepository accountingRepository) 
            : base(validator, microsoftGraphRepository, contactRepository, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(ICreateContactCommand command, IRefreshableToken token)
        {
            NullGuard.NotNull(command, nameof(command))
                .NotNull(token, nameof(token));

            IContact contact = command.ToDomain(ContactRepository, AccountingRepository);

            IContact createdContact = await MicrosoftGraphRepository.CreateContactAsync(token, contact);
            if (createdContact == null)
            {
                return;
            }

            contact.ExternalIdentifier = createdContact.ExternalIdentifier;

            await ContactRepository.CreateOrUpdateContactSupplementAsync(contact);
        }

        #endregion
    }
}