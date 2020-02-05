using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public class DeleteContactCommandHandler : ContactCommandHandlerBase<IDeleteContactCommand>
    {
        #region Constructor

        public DeleteContactCommandHandler(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository, IAccountingRepository accountingRepository) 
            : base(validator, microsoftGraphRepository, contactRepository, accountingRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IDeleteContactCommand command, IRefreshableToken token)
        {
            NullGuard.NotNull(command, nameof(command))
                .NotNull(token, nameof(token));

            string externalIdentifier = command.ExternalIdentifier;
            if (string.IsNullOrWhiteSpace(externalIdentifier))
            {
                return;
            }

            await ContactRepository.DeleteContactSupplementAsync(externalIdentifier);

            await MicrosoftGraphRepository.DeleteContactAsync(token, externalIdentifier);
        }

        #endregion
    }
}