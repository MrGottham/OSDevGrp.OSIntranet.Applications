using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Attributes;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Helpers.Security.Filters
{
    public class AcquireTokenActionFilter : IActionFilter
    {
        #region Private varibales

        private readonly ITokenHelperFactory _tokenHelperFactory;

        #endregion

        #region Constructor

        public AcquireTokenActionFilter(ITokenHelperFactory tokenHelperFactory)
        {
            NullGuard.NotNull(tokenHelperFactory, nameof(tokenHelperFactory));

            _tokenHelperFactory = tokenHelperFactory;
        }

        #endregion

        #region Methods

        public void OnActionExecuting(ActionExecutingContext context)
        {
            NullGuard.NotNull(context, nameof(context));

            ControllerActionDescriptor controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null || controllerActionDescriptor.MethodInfo == null)
            {
                return;
            }

            AcquireTokenAttribute acquireTokenAttribute = controllerActionDescriptor.MethodInfo.GetCustomAttribute<AcquireTokenAttribute>();
            if (acquireTokenAttribute == null)
            {
                return;
            }

            TokenType tokenType = acquireTokenAttribute.TokenType;

            IToken token = GetTokenAsync(tokenType, context.HttpContext).GetAwaiter().GetResult();
            if (token != null)
            {
                return;
            }

            IRefreshableToken refreshableToken = GetRefreshableTokenAsync(tokenType, context.HttpContext).GetAwaiter().GetResult();
            if (refreshableToken == null)
            {
                context.Result = _tokenHelperFactory.AuthorizeAsync(tokenType, context.HttpContext, context.HttpContext.Request.GetDisplayUrl())
                    .GetAwaiter()
                    .GetResult();
                return;
            }

            if (refreshableToken.HasExpired == false)
            {
                return;
            }

            context.Result = _tokenHelperFactory.RefreshTokenAsync(tokenType, context.HttpContext, context.HttpContext.Request.GetDisplayUrl())
                .GetAwaiter()
                .GetResult();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            NullGuard.NotNull(context, nameof(context));
        }

        private Task<IToken> GetTokenAsync(TokenType tokenType, HttpContext httpContext)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            switch (tokenType)
            {
                default:
                    return Task.Run(() => (IToken) null);
            }
       }

        private Task<IRefreshableToken> GetRefreshableTokenAsync(TokenType tokenType, HttpContext httpContext)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            switch (tokenType)
            {
                case TokenType.MicrosoftGraphToken:
                    return _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(tokenType, httpContext);

                default:
                    return Task.Run(() => (IRefreshableToken) null);
            }
        }

        #endregion
    }
}