using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Contacts
{
    public interface IContactGroup : IAuditable, IDeletable
    {
        int Number { get; set; }

        string Name { get; set; }
    }
}
