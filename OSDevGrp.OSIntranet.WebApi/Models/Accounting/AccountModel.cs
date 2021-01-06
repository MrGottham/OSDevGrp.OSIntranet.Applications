using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountModel : AccountCoreDataModel
    {
        [JsonProperty(Required = Required.Always)]
        public AccountGroupModel AccountGroup { get; set; }

        [JsonProperty(Required = Required.Always)]
        public CreditInfoValuesModel ValuesAtStatusDate { get; set; }

        [JsonProperty(Required = Required.Always)]
        public CreditInfoValuesModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        [JsonProperty(Required = Required.Always)]
        public CreditInfoValuesModel ValuesAtEndOfLastYearFromStatusDate { get; set; }
    }
}