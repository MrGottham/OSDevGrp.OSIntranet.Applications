using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface IPostalCodeCommand : IPostalCodeIdentificationCommand
    {
        string City { get; set; }

        string State { get; set; }

        IPostalCode ToDomain(IContactRepository contactRepository);
    }
}
