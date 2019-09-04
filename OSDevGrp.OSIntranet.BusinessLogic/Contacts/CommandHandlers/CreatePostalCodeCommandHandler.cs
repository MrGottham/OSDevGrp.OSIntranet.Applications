using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public class CreatePostalCodeCommandHandler : PostalCodeIdentificationCommandBase<ICreatePostalCodeCommand>
    {
        #region Constructor

        public CreatePostalCodeCommandHandler(IValidator validator, IContactRepository contactRepository) 
            : base(validator, contactRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(ICreatePostalCodeCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IPostalCode postalCode = command.ToDomain(ContactRepository);

            await ContactRepository.CreatePostalCodeAsync(postalCode);
        }

        #endregion
    }
}
