using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BudgetAccountModel : AccountCoreDataModel
    {
        [JsonProperty(Required = Required.Always)]
        public BudgetAccountGroupModel BudgetAccountGroup { get; set; }
    }
}