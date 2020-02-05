using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface ICompanyCommand : ICommand
    {
        ICompanyNameCommand Name { get; set; }

        IAddressCommand Address { get; set; }

        string PrimaryPhone { get; set; }

        string SecondaryPhone { get; set; }

        string HomePage { get; set; }

        IValidator Validate(IValidator validator);

        ICompany ToDomain();
    }
}