using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.WebApi.Models.Contacts;

public class AddressModel
{
    [StringLength(256)]
    public string StreetLine1 { get; set; }

    [StringLength(256)]
    public string StreetLine2 { get; set; }

    [StringLength(16)]
    [RegularExpression(ValidationRegexPatterns.PostalCodeRegexPattern)]
    public string PostalCode { get; set; }

    [StringLength(256)]
    public string City { get; set; }

    [StringLength(256)]
    public string State { get; set; }

    [StringLength(256)]
    public string Country { get; set; }

    public string DisplayAddress { get; set; }
}