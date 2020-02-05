using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface IAddressCommand : ICommand
    {
        string StreetLine1 { get; set; }

        string StreetLine2 { get; set; }

        string PostalCode { get; set; }

        string City { get; set; }

        string State { get; set; }

        string Country { get; set; }

        IValidator Validate(IValidator validator);

        bool IsEmpty();

        IAddress ToDomain();
    }
}