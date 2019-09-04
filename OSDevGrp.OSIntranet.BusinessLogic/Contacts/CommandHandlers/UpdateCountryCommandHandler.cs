using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public class UpdateCountryCommandHandler : CountryIdentificationCommandHandlerBase<IUpdateCountryCommand>
    {
        #region Constructor

        public UpdateCountryCommandHandler(IValidator validator, IContactRepository contactRepository)
            : base(validator, contactRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IUpdateCountryCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            ICountry country = command.ToDomain();

            await ContactRepository.UpdateCountryAsync(country);
        }

        #endregion
    }
}
