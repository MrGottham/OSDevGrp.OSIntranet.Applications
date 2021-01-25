using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BudgetAccountModel : AccountCoreDataModel
    {
        [JsonProperty(Required = Required.Always)]
        public BudgetAccountGroupModel BudgetAccountGroup { get; set; }

        [JsonProperty(Required = Required.Always)]
        public BudgetInfoValuesModel ValuesForMonthOfStatusDate { get; set; }

        [JsonProperty(Required = Required.Always)]
        public BudgetInfoValuesModel ValuesForLastMonthOfStatusDate { get; set; }

        [JsonProperty(Required = Required.Always)]
        public BudgetInfoValuesModel ValuesForYearToDateOfStatusDate { get; set; }

        [JsonProperty(Required = Required.Always)]
        public BudgetInfoValuesModel ValuesForLastYearOfStatusDate { get; set; }

        [JsonProperty(Required = Required.Always)]
        public BudgetInfoCollectionModel BudgetInfos { get; set; }
    }
}