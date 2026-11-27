using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OSDevGrp.OSIntranet.WebApi.Models.Shared;

namespace OSDevGrp.OSIntranet.WebApi.Models.Contacts;

public class ContactModel
{
    [Required]
    [StringLength(256, MinimumLength = 1)]
    [JsonRequired]
    public string ExternalIdentifier { get; set; }

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

    [StringLength(32)]
    [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern)]
    public string HomePhone { get; set; }

    [StringLength(32)]
    [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern)]
    public string MobilePhone { get; set; }

    public DateTime? Birthday { get; set; }

    public ushort? Age { get; set; }

    [StringLength(256)]
    [RegularExpression(ValidationRegexPatterns.MailAddressRegexPattern)]
    public string MailAddress { get; set; }

    public CompanyModel Company { get; set; }

    [Required]
    [JsonRequired]
    public ContactGroupModel ContactGroup { get; set; }

    [StringLength(4096)]
    public string Acquaintance { get; set; }

    [StringLength(256)]
    [RegularExpression(ValidationRegexPatterns.UrlRegexPattern)]
    public string PersonalHomePage { get; set; }

    [Required]
    [Range(1, 365)]
    [JsonRequired]
    public int LendingLimit { get; set; }

    [Required]
    [JsonRequired]
    public PaymentTermModel PaymentTerm { get; set; }
}