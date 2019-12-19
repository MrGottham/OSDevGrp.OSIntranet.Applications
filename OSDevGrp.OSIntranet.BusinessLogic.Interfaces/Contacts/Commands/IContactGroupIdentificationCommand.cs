using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface IContactGroupIdentificationCommand : ICommand
    {
        int Number { get; set; }

        IValidator Validate(IValidator validator, IContactRepository contactRepository);
    }
}
