using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Helpers.Security
{
    public interface ITokenHelper
    {
        TokenType TokenType { get; }

        Task<IActionResult> AuthorizeAsync(HttpContext httpContext, string returnUrl);

        Task<IActionResult> AcquireTokenAsync(HttpContext httpContext, params object[] arguments);

        Task<IActionResult> RefreshTokenAsync(HttpContext httpContext, string returnUrl);

        Task StoreTokenAsync(HttpContext httpContext, string base64Token);

        Task HandleLogoutAsync(HttpContext httpContext);
    }

    public interface ITokenHelper<T> : ITokenHelper where T : class, IToken
    {
        Task<T> GetTokenAsync(HttpContext httpContext);
    }
}