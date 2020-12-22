using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface IClaimResolver
    {
        string GetCountryCode();

        int? GetAccountingNumber();

        TToken GetToken<TToken>(Func<string, string> unprotect) where TToken : class, IToken;
    }
}