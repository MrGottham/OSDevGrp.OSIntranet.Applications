using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountModel : AccountCoreDataModel
    {
        [Required]
        [JsonProperty(Required = Required.Always)]
        public AccountGroupModel AccountGroup { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public CreditInfoValuesModel ValuesAtStatusDate { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public CreditInfoValuesModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public CreditInfoValuesModel ValuesAtEndOfLastYearFromStatusDate { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public CreditInfoCollectionModel CreditInfos { get; set; }
    }
}