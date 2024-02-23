using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Security.Principal;

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

        bool IsMediaLibraryModifier();

        bool IsMediaLibraryLender();

        IRefreshableToken GetMicrosoftToken(Func<string, string> unprotect);

        IRefreshableToken GetMicrosoftToken(IPrincipal principal, Func<string, string> unprotect);

        IToken GetGoogleToken(Func<string, string> unprotect);

        IToken GetGoogleToken(IPrincipal principal, Func<string, string> unprotect);
    }
}