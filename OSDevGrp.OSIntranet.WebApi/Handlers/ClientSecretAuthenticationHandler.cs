using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;

namespace OSDevGrp.OSIntranet.WebApi.Handlers
{
    internal class ClientSecretAuthenticationHandler : AuthenticationHandler<ClientSecretAuthenticationOptions>
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly Regex _basicAuthenticationRegex = new Regex("^([a-f0-9]{32}):([a-f0-9]{32})$", RegexOptions.Compiled);

        #endregion

        #region Constructor

        public ClientSecretAuthenticationHandler(ICommandBus commandBus, IDataProtectionProvider dataProtectionProvider, IOptionsMonitor<ClientSecretAuthenticationOptions> clientSecretAuthenticationOptions, ILoggerFactory loggerFactory, UrlEncoder urlEncoder, ISystemClock systemClock) 
            : base(clientSecretAuthenticationOptions, loggerFactory, urlEncoder, systemClock)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(dataProtectionProvider, nameof(dataProtectionProvider));

            _commandBus = commandBus;
            _dataProtectionProvider = dataProtectionProvider;
        }

        #endregion

        #region Methods

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            ClaimsPrincipal currentClaimsPrincipal = Context.User;
            if (currentClaimsPrincipal == null)
            {
                return AuthenticateResult.Fail("User is not specified in the context.");
            }

            AuthenticationHeaderValue authenticationHeaderValue = GetAuthenticationHeaderValue(Context.Request);
            if (authenticationHeaderValue == null)
            {
                return AuthenticateResult.Fail("Authentication header is missing in the submitted request.");
            }

            Match match = _basicAuthenticationRegex.Match(Encoding.UTF8.GetString(Convert.FromBase64String(authenticationHeaderValue.Parameter)));
            if (string.CompareOrdinal(authenticationHeaderValue.Scheme, Scheme.Name) != 0 || match.Success == false)
            {
                return AuthenticateResult.Fail("Values within the submitted authentication header is invalid.");
            }

            string clientId = match.Groups[1].Value;
            string clientSecret = match.Groups[2].Value;

            ClaimsPrincipal authenticatedClaimsPrincipal = await GetAuthenticatedClaimsPrincipalAsync(clientId, clientSecret);
            if (authenticatedClaimsPrincipal == null)
            {
                return AuthenticateResult.Fail("Unknown client id or wrong client secret.");
            }

            AuthenticationTicket authenticationTicket = new AuthenticationTicket(authenticatedClaimsPrincipal, Scheme.Name);

            return AuthenticateResult.Success(authenticationTicket);
        }

        private async Task<ClaimsPrincipal> GetAuthenticatedClaimsPrincipalAsync(string clientId, string clientSecret)
        {
            NullGuard.NotNull(clientId, nameof(clientId))
                .NotNull(clientSecret, nameof(clientSecret));

            IAuthenticateClientSecretCommand command = new AuthenticateClientSecretCommand(clientId, clientSecret);
            IClientSecretIdentity clientSecretIdentity = await _commandBus.PublishAsync<IAuthenticateClientSecretCommand, IClientSecretIdentity>(command);
            if (clientSecretIdentity == null)
            {
                return null;
            }

            //TODO: Handle this
            //clientSecretIdentity.AddClaims(new[] {ClaimHelper.CreateTokenClaim(clientSecretIdentity.Token, ProtectBase64Token)});

            ClaimsIdentity authenticatedClaimsIdentity = new ClaimsIdentity(clientSecretIdentity.ToClaimsIdentity().Claims, Scheme.Name);
            return new ClaimsPrincipal(authenticatedClaimsIdentity);
        }

        private string ProtectBase64Token(string base64Token)
        {
            NullGuard.NotNullOrWhiteSpace(base64Token, nameof(base64Token));

            IDataProtector dataProtector = _dataProtectionProvider.CreateProtector("TokenProtection");

            return dataProtector.Protect(base64Token);
        }

        private static AuthenticationHeaderValue GetAuthenticationHeaderValue(HttpRequest request)
        {
            NullGuard.NotNull(request, nameof(request));

            if (request.Headers == null || string.IsNullOrWhiteSpace(request.Headers["Authorization"]))
            {
                return null;
            }

            if (AuthenticationHeaderValue.TryParse(request.Headers["Authorization"], out var authenticationHeader) == false)
            {
                return null;
            }

            return authenticationHeader;
        }

        #endregion
    }
}