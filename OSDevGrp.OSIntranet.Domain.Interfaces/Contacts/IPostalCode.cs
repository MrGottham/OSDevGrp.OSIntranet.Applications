using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Contacts
{
    public interface IPostalCode : IAuditable, IDeletable
    {
        ICountry Country { get; }

        string Code { get; set; }

        string City { get; set; }

        string State { get; set; }
    }
}
