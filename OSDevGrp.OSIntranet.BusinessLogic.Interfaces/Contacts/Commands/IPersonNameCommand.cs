namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface IPersonNameCommand : INameCommand
    {
        string GivenName { get; set; }

        string MiddleName { get; set; }

        string Surname { get; set; }
    }
}