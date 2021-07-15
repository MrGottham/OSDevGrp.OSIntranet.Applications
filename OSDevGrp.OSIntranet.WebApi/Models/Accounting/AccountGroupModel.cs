using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountGroupModel : AccountGroupModelBase
    {
        [Required]
        [JsonProperty(Required = Required.Always)]
        public AccountGroupType AccountGroupType { get; set; }
    }
}