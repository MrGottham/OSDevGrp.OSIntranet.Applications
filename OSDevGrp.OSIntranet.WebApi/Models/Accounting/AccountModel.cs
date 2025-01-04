using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountModel : AccountCoreDataModel
    {
        [Required]
        [JsonRequired]
        public AccountGroupModel AccountGroup { get; set; }

        [Required]
        [JsonRequired]
        public CreditInfoValuesModel ValuesAtStatusDate { get; set; }

        [Required]
        [JsonRequired]
        public CreditInfoValuesModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        [Required]
        [JsonRequired]
        public CreditInfoValuesModel ValuesAtEndOfLastYearFromStatusDate { get; set; }

        [Required]
        [JsonRequired]
        public CreditInfoCollectionModel CreditInfos { get; set; }
    }
}