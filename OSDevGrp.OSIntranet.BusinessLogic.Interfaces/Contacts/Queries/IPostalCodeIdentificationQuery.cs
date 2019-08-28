namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries
{
    public interface IPostalCodeIdentificationQuery : ICountryIdentificationQuery
    {
        string PostalCode { get; set; }
    }
}
