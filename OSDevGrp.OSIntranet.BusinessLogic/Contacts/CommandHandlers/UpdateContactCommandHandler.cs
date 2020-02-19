using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public class UpdateContactCommandHandler : ContactCommandHandlerBase<IUpdateContactCommand>
    {
        #region Constructor

        public UpdateContactCommandHandler(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository, IAccountingRepository accountingRepository) 
            : base(validator, microsoftGraphRepository, contactRepository, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IUpdateContactCommand command, IRefreshableToken token)
        {
            NullGuard.NotNull(command, nameof(command))
                .NotNull(token, nameof(token));

            if (string.IsNullOrWhiteSpace(command.ExternalIdentifier))
            {
                return;
            }

            IContact contact = command.ToDomain(ContactRepository, AccountingRepository);
            
            IContact existingContact = await command.GetExistingContactAsync(MicrosoftGraphRepository, ContactRepository);
            if (existingContact == null)
            {
                return;
            }

            string existingInternalIdentifier = existingContact.InternalIdentifier;
            if (string.IsNullOrWhiteSpace(existingInternalIdentifier) == false)
            {
                contact.InternalIdentifier = existingInternalIdentifier;
            }

            IContact updatedContact = await MicrosoftGraphRepository.UpdateContactAsync(token, contact);
            if (updatedContact == null)
            {
                return;
            }

            string updatedExternalIdentifier = updatedContact.ExternalIdentifier;
            if (string.IsNullOrWhiteSpace(updatedExternalIdentifier))
            {
                return;
            }

            contact.ExternalIdentifier = updatedExternalIdentifier;

            await ContactRepository.CreateOrUpdateContactSupplementAsync(contact, existingContact.ExternalIdentifier);
        }

        #endregion
    }
}