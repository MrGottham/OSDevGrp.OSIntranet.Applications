using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Contacts;

public class CountryCodeModel
{
    [Required]
    [StringLength(4, MinimumLength = 1)]
    [RegularExpression(ValidationRegexPatterns.CountryCodeRegexPattern)]
    [JsonRequired]
    public string Code { get; set; }

    [Required]
    [StringLength(256, MinimumLength = 1)]
    [JsonRequired]
    public string Name { get; set; }

    [Required]
    [StringLength(256, MinimumLength = 1)]
    [JsonRequired]
    public string UniversalName { get; set; }

    [Required]
    [StringLength(4, MinimumLength = 1)]
    [RegularExpression(ValidationRegexPatterns.PhonePrefixRegexPattern)]
    [JsonRequired]
    public string PhonePrefix { get; set; }
}