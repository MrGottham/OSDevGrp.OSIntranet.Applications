namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries
{
    public interface IContactIdentificationQuery : IContactQuery
    {
        string ExternalIdentifier { get; set; }
    }
}