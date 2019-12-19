using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface IContactGroupCommand : IContactGroupIdentificationCommand
    {
        string Name { get; set; }

        IContactGroup ToDomain();
    }
}
