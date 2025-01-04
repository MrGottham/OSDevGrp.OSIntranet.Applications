using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class BudgetInfoModel : InfoModelBase
    {
        [Required]
        [Range(typeof(decimal), "0", "99999999")]
        [JsonRequired]
        public decimal Income { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "99999999")]
        [JsonRequired]
        public decimal Expenses { get; set; }

        [Required]
        [JsonRequired]
        public decimal Budget { get; set; }

        [Required]
        [JsonRequired]
        public decimal Posted { get; set; }

        [Required]
        [JsonRequired]
        public decimal Available { get; set; }
    }
}