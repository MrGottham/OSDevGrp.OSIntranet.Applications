using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal static class AccountGroupTypeExtensions
    {
        internal static string Translate(this AccountGroupType accountGroupType)
        {
            switch (accountGroupType)
            {
                case AccountGroupType.Assets:
                    return "Aktiver";

                case AccountGroupType.Liabilities:
                    return "Passiver";

                default:
                    throw new IntranetExceptionBuilder(ErrorCode.UnableToTranslateValueFor, accountGroupType).Build();
            }
        }
    }
}