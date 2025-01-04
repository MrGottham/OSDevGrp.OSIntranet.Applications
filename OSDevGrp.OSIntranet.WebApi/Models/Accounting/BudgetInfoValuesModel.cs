using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BudgetInfoValuesModel
    {
        [Required]
        [JsonRequired]
        public decimal Budget { get; set; }

        [Required]
        [JsonRequired]
        public decimal Posted { get; set; }

        [Required]
        [JsonRequired]
        public decimal Available { get; set;  }
    }
}