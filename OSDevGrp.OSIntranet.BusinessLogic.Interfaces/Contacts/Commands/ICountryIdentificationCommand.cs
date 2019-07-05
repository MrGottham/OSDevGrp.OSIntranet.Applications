using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface ICountryIdentificationCommand : ICommand
    {
        string CountryCode { get; set; }

        IValidator Validate(IValidator validator, IContactRepository contactRepository);
    }
}
