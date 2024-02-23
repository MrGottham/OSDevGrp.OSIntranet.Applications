using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Helpers.Security
{
	public class MicrosoftGraphTokenHelper : TokenHelperBase<IRefreshableToken>
    {
        #region Constructor

        public MicrosoftGraphTokenHelper(IQueryBus queryBus, ICommandBus commandBus, ITrustedDomainHelper trustedDomainHelper, IDataProtectionProvider dataProtectionProvider) 
            : base(queryBus, commandBus, trustedDomainHelper, dataProtectionProvider)
        {
        }

        #endregion

        #region Properties

        public override TokenType TokenType => TokenType.MicrosoftGraphToken;

        protected override string TokenCookieName => "OSDevGrp.OSIntranet.Microsoft.Graph.Token";

        #endregion

        #region Methods

        protected override Task<Uri> GenerateAuthorizeUriAsync(HttpContext httpContext, Guid stateIdentifier)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            IGetAuthorizeUriForMicrosoftGraphQuery query = new GetAuthorizeUriForMicrosoftGraphQuery(GetRedirectUriForMicrosoftGraph(httpContext.Request), stateIdentifier); 
            return QueryBus.QueryAsync<IGetAuthorizeUriForMicrosoftGraphQuery, Uri>(query);
        }

        protected override Task<Guid?> GetStateIdentifierAsync(HttpContext httpContext, params object[] arguments)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            return Task.Run(() =>
            {
                if (arguments == null || arguments.Length < 1 || arguments[0].GetType() != typeof(Guid))
                {
                    return null;
                }

                return (Guid?) arguments[0];
            });
        }

        protected override async Task<IRefreshableToken> DoAcquireTokenAsync(HttpContext httpContext, params object[] arguments)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext));

            if (arguments == null || arguments.Length < 2 || arguments[1].GetType() != typeof(string))
            {
                return null;
            }

            string code = (string) arguments[1];
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            IAcquireTokenForMicrosoftGraphCommand acquireTokenForMicrosoftGraphCommand = new AcquireTokenForMicrosoftGraphCommand(GetRedirectUriForMicrosoftGraph(httpContext.Request), code);
            return await CommandBus.PublishAsync<IAcquireTokenForMicrosoftGraphCommand, IRefreshableToken>(acquireTokenForMicrosoftGraphCommand);
        }

        protected override Task<IRefreshableToken> DoRefreshTokenAsync(HttpContext httpContext, IRefreshableToken expiredToken)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNull(expiredToken, nameof(expiredToken));

            IRefreshTokenForMicrosoftGraphCommand command = new RefreshTokenForMicrosoftGraphCommand(GetRedirectUriForMicrosoftGraph(httpContext.Request), expiredToken);
            return CommandBus.PublishAsync<IRefreshTokenForMicrosoftGraphCommand, IRefreshableToken>(command);
        }

        protected override Task<IRefreshableToken> GenerateTokenAsync(HttpContext httpContext, string base64Token)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNullOrWhiteSpace(base64Token, nameof(base64Token));

            return Task.Run(() => RefreshableTokenFactory.Create().FromBase64String(base64Token));
        }

        private Uri GetRedirectUriForMicrosoftGraph(HttpRequest httpRequest)
        {
            NullGuard.NotNull(httpRequest, nameof(httpRequest));

            return new Uri(httpRequest.ToAbsoluteUri("/Account/MicrosoftGraphCallback").ToLower());
        }

        #endregion
    }
}