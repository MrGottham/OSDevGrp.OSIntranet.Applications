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

            IContact contact = command.ToDomain(ContactRepository, AccountingRepository);

            IContact updatedContact = await MicrosoftGraphRepository.UpdateContactAsync(token, contact);
            if (updatedContact == null)
            {
                return;
            }

            await ContactRepository.CreateOrUpdateContactSupplementAsync(updatedContact);
        }

        #endregion
    }
}