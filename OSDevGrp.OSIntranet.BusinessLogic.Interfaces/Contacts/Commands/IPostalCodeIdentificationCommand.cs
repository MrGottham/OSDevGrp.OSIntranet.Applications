namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface IPostalCodeIdentificationCommand : ICountryIdentificationCommand
    {
        string PostalCode { get; set; }
    }
}
