using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class CreditInfoModel : BalanceInfoModel
    {
        [Required]
        [Range(typeof(decimal), "0", "99999999")]
        [JsonRequired]
        public decimal Credit { get; set; }

        [Required]
        [JsonRequired]
        public decimal Available { get; set; }
    }
}