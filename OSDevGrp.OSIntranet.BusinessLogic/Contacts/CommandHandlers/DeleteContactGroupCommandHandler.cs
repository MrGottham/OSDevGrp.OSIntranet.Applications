using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public class DeleteContactGroupCommandHandler : ContactGroupIdentificationCommandHandlerBase<IDeleteContactGroupCommand>
    {
        #region Constructor

        public DeleteContactGroupCommandHandler(IValidator validator, IContactRepository contactRepository) 
            : base(validator, contactRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IDeleteContactGroupCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            await ContactRepository.DeleteContactGroupAsync(command.Number);
        }

        #endregion
    }
}
