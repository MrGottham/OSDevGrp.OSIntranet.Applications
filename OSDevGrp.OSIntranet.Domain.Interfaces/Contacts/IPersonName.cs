namespace OSDevGrp.OSIntranet.Domain.Interfaces.Contacts
{
    public interface IPersonName : IName
    {
        string GivenName { get; set; }

        string MiddleName { get; set; }

        string Surname { get; set; }
    }
}
