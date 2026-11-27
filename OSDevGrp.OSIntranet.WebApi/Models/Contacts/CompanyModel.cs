using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Contacts;

public class CompanyModel
{
    [Required]
    [JsonRequired]
    public NameModel Name { get; set; }

    [Required]
    [JsonRequired]
    public AddressModel Address { get; set; }

    [StringLength(32)]
    [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern)]
    public string PrimaryPhone { get; set; }

    [StringLength(32)]
    [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern)]
    public string SecondaryPhone { get; set; }

    [StringLength(256)]
    [RegularExpression(ValidationRegexPatterns.UrlRegexPattern)]
    public string HomePage { get; set; }
}