namespace OSDevGrp.OSIntranet.Domain.Interfaces.Contacts
{
    public interface IAddress
    {
        string StreetLine1 { get; set; }

        string StreetLine2 { get; set; }

        string PostalCode { get; set; }

        string City { get; set; }

        string State { get; set; }

        string Country { get; set; }

        string DisplayAddress { get; }
    }
}
