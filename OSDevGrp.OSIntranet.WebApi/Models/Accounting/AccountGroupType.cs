using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AccountGroupType
    {
        [EnumMember(Value = "Assets")]
        Assets,

        [EnumMember(Value = "Liabilities")]
        Liabilities
    }
}
