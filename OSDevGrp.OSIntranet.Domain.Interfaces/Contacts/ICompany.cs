namespace OSDevGrp.OSIntranet.Domain.Interfaces.Contacts
{
    public interface ICompany
    {
        ICompanyName Name { get; }

        IAddress Address { get; }

        string PrimaryPhone { get; set; }

        string SecondaryPhone { get; set; }

        string HomePage { get; set; }
    }
}
