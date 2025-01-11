using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class ContactAccountModel : AccountCoreDataModel
    {
        [StringLength(256, MinimumLength = 1)]
        [RegularExpression(ValidationRegexPatterns.MailAddressRegexPattern)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string MailAddress { get; set; }

        [StringLength(32, MinimumLength = 1)]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PrimaryPhone { get; set; }

        [StringLength(32, MinimumLength = 1)]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SecondaryPhone { get; set; }

        [Required]
        [JsonRequired]
        public PaymentTermModel PaymentTerm { get; set; }

        [Required]
        [JsonRequired]
        public BalanceInfoValuesModel ValuesAtStatusDate { get; set; }

        [Required]
        [JsonRequired]
        public BalanceInfoValuesModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        [Required]
        [JsonRequired]
        public BalanceInfoValuesModel ValuesAtEndOfLastYearFromStatusDate { get; set; }

        [Required]
        [JsonRequired]
        public BalanceInfoCollectionModel BalanceInfos { get; set; }
    }
}