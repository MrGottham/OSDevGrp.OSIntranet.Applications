using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public enum BalanceBelowZeroType
    {
        [EnumMember(Value = "Debtors")]
        Debtors,

        [EnumMember(Value = "Creditors")]
        Creditors
    }
}