using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountGroupModel : AccountGroupModelBase
    {
        [Required]
        [JsonRequired]
        public AccountGroupType AccountGroupType { get; set; }
    }
}