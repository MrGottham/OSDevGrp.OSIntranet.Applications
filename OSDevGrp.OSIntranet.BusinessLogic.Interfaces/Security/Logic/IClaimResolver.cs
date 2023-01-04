using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface IClaimResolver
    {
        string GetCountryCode();

        int? GetAccountingNumber();

        bool IsAccountingAdministrator();

        bool IsAccountingCreator();

        bool CanModifyAccounting(int accountingNumber);

        bool CanAccessAccounting(int accountingNumber);

        string GetNameIdentifier();

        string GetName();

        string GetMailAddress();

        int? GetNumberOfNewsToCollect();

        TToken GetToken<TToken>(Func<string, string> unprotect) where TToken : class, IToken;
    }
}