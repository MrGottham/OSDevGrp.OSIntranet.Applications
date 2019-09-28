using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Contacts
{
    public interface ICountry : IAuditable, IDeletable
    {
        string Code { get; set; }

        string Name { get; set; }

        string UniversalName { get; set; }

        string PhonePrefix { get; set; }

        bool DefaultForPrincipal { get; }

        void ApplyDefaultForPrincipal(string defaultCountryCode);
    }
}
