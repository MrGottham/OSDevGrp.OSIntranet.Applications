using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Helpers.Security
{
    public interface ITokenHelperFactory
    {
        Task<IActionResult> AuthorizeAsync(TokenType tokenType, HttpContext httpContext, string returnUrl);

        Task<IActionResult> AcquireTokenAsync(TokenType tokenType, HttpContext httpContext, params object[] arguments);

        Task<IActionResult> RefreshTokenAsync(TokenType tokenType, HttpContext httpContext, string returnUrl);

        Task<T> GetTokenAsync<T>(TokenType tokenType, HttpContext httpContext) where T : class, IToken;

        Task StoreTokenAsync(TokenType tokenType, HttpContext httpContext, string base64Token);

        Task HandleLogoutAsync(HttpContext httpContext);
    }
}