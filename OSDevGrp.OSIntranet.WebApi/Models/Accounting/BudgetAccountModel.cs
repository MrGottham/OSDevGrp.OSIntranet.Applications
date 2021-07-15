using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BudgetAccountModel : AccountCoreDataModel
    {
        [Required]
        [JsonProperty(Required = Required.Always)]
        public BudgetAccountGroupModel BudgetAccountGroup { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public BudgetInfoValuesModel ValuesForMonthOfStatusDate { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public BudgetInfoValuesModel ValuesForLastMonthOfStatusDate { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public BudgetInfoValuesModel ValuesForYearToDateOfStatusDate { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public BudgetInfoValuesModel ValuesForLastYearOfStatusDate { get; set; }

        [Required]
        [JsonProperty(Required = Required.Always)]
        public BudgetInfoCollectionModel BudgetInfos { get; set; }
    }
}