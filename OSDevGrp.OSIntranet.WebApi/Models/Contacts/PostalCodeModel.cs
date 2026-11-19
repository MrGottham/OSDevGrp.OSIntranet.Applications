using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Contacts;

public class PostalCodeModel
{
    [Required]
    [JsonRequired]
    public CountryCodeModel Country { get; set; }

    [Required]
    [StringLength(16, MinimumLength = 1)]
    [RegularExpression(ValidationRegexPatterns.PostalCodeRegexPattern)]
    [JsonRequired]
    public string Code { get; set; }

    [Required]
    [StringLength(256, MinimumLength = 1)]
    [JsonRequired]
    public string City { get; set; }

    [StringLength(256)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string State { get; set; }
}