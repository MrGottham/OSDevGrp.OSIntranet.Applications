using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BalanceInfoModel : InfoModelBase
    {
        [Required]
        [JsonRequired]
        public decimal Balance { get; set; }
    }
}