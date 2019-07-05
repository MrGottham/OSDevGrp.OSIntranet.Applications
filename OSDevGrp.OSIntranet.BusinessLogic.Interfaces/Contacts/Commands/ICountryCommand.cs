using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface ICountryCommand : ICountryIdentificationCommand
    {
        string Name { get; set; }

        string UniversalName { get; set; }

        string PhonePrefix { get; set; }

        ICountry ToDomain();
    }
}
