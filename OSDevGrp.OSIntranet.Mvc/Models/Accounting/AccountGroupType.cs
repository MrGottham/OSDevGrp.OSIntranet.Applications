using System.ComponentModel;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public enum AccountGroupType
    {
        [DisplayName("Aktiver")]
        Assets,

        [DisplayName("Passiver")]
        Liabilities
    }
}