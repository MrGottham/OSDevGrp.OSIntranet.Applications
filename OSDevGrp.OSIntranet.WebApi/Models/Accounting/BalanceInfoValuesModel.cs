using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BalanceInfoValuesModel
    {
        [Required]
        [JsonRequired]
        public decimal Balance { get; set; }
    }
}