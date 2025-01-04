using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BudgetAccountModel : AccountCoreDataModel
    {
        [Required]
        [JsonRequired]
        public BudgetAccountGroupModel BudgetAccountGroup { get; set; }

        [Required]
        [JsonRequired]
        public BudgetInfoValuesModel ValuesForMonthOfStatusDate { get; set; }

        [Required]
        [JsonRequired]
        public BudgetInfoValuesModel ValuesForLastMonthOfStatusDate { get; set; }

        [Required]
        [JsonRequired]
        public BudgetInfoValuesModel ValuesForYearToDateOfStatusDate { get; set; }

        [Required]
        [JsonRequired]
        public BudgetInfoValuesModel ValuesForLastYearOfStatusDate { get; set; }

        [Required]
        [JsonRequired]
        public BudgetInfoCollectionModel BudgetInfos { get; set; }
    }
}