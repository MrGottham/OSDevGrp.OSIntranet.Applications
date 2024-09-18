namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IUserAddress
    {
        string MailingAddress { get; }

        string StreetAddress { get; }

        string City { get; }

        string Region { get; }

        string PostalCode { get; }

        string Country { get; }

        bool IsEmpty();

        string ToJson();
    }
}