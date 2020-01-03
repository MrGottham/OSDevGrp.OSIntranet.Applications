using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public enum AccountGroupType
    {
        [EnumMember(Value = "Assets")]
        Assets,

        [EnumMember(Value = "Liabilities")]
        Liabilities
    }
}
