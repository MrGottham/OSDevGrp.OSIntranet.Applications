namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface IDeleteContactCommand : IContactCommand
    {
        string ExternalIdentifier { get; set; }
    }
}