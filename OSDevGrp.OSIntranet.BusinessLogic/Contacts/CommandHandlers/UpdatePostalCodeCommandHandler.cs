using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public class UpdatePostalCodeCommandHandler : PostalCodeIdentificationCommandBase<IUpdatePostalCodeCommand>
    {
        #region Constructor

        public UpdatePostalCodeCommandHandler(IValidator validator, IContactRepository contactRepository) 
            : base(validator, contactRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IUpdatePostalCodeCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IPostalCode postalCode = command.ToDomain(ContactRepository);

            await ContactRepository.UpdatePostalCodeAsync(postalCode);
        }

        #endregion
    }
}
