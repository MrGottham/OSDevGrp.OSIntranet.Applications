using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Contacts;

public class NameModel
{
    [Required]
    [StringLength(256, MinimumLength = 1)]
    [JsonRequired]
    public string DisplayName { get; set; }

    [StringLength(256)]
    public string GivenName { get; set; }

    [StringLength(256)]
    public string MiddleName { get; set; }

    [StringLength(256)]
    public string Surname { get; set; }

    [StringLength(256)]
    public string FullName { get; set; }
}