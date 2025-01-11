using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class CreditInfoValuesModel : BalanceInfoValuesModel
    {
        [Required]
        [JsonRequired]
        public decimal Credit { get; set; }

        [Required]
        [JsonRequired]
        public decimal Available { get; set; }
    }
}