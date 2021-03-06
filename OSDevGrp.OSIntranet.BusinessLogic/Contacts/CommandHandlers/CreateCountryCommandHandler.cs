﻿using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public class CreateCountryCommandHandler : CountryIdentificationCommandHandlerBase<ICreateCountryCommand>
    {
        #region Constructor

        public CreateCountryCommandHandler(IValidator validator, IContactRepository contactRepository)
            : base(validator, contactRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(ICreateCountryCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            ICountry country = command.ToDomain();

            await ContactRepository.CreateCountryAsync(country);
        }

        #endregion
    }
}
