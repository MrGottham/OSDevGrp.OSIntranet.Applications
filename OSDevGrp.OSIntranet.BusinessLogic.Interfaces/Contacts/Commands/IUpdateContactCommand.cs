namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface IUpdateContactCommand : IContactDataCommand
    {
        string ExternalIdentifier { get; set; }
    }
}