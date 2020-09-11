using OSDevGrp.OSIntranet.Core.Interfaces.Converters;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic
{
    public interface IContactToCsvConverter : IDomainObjectToCsvConverter<IContact>
    {
    }
}