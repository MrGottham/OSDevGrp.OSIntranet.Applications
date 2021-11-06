using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class ContactAccountModel : AccountCoreDataModel
    {
        [StringLength(256, MinimumLength = 1)]
        [RegularExpression(ValidationRegexPatterns.MailAddressRegexPattern)]
        [JsonProperty(Required = Required.Default)]
        public string MailAddress { get; set; }

        [StringLength(32, MinimumLength = 1)]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern)]
        [JsonProperty(Required = Required.Default)]
        public string PrimaryPhone { get; set; }

        [StringLength(32, MinimumLength = 1)]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern)]
        [JsonProperty(Required = Required.Default)]
        public string SecondaryPhone { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public PaymentTermModel PaymentTerm { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public BalanceInfoValuesModel ValuesAtStatusDate { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public BalanceInfoValuesModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public BalanceInfoValuesModel ValuesAtEndOfLastYearFromStatusDate { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public BalanceInfoCollectionModel BalanceInfos { get; set; }
    }
}