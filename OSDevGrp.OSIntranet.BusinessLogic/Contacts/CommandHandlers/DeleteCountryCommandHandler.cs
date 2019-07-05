using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public class DeleteCountryCommandHandler : CountryIdentificationCommandHandlerBase<IDeleteCountryCommand>
    {
        #region Constructor

        public DeleteCountryCommandHandler(IValidator validator, IContactRepository contactRepository)
            : base(validator, contactRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task ManageRepositoryAsync(IDeleteCountryCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            return ContactRepository.DeleteCountryAsync(command.CountryCode);
        }

        #endregion
    }
}
