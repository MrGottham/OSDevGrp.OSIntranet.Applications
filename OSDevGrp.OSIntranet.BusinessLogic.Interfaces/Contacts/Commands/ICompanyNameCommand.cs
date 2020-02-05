namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface ICompanyNameCommand : INameCommand
    {
        string FullName { get; set; }
    }
}