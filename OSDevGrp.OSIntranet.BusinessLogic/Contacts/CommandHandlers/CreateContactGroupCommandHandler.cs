﻿using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public class CreateContactGroupCommandHandler : ContactGroupIdentificationCommandHandlerBase<ICreateContactGroupCommand>
    {
        #region Contstructor

        public CreateContactGroupCommandHandler(IValidator validator, IContactRepository contactRepository) 
            : base(validator, contactRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(ICreateContactGroupCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IContactGroup contactGroup =  command.ToDomain();

            await ContactRepository.CreateContactGroupAsync(contactGroup);
        }

        #endregion
    }
}
