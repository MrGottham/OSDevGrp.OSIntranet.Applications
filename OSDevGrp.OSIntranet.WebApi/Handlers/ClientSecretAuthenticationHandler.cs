﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.WebApi.Handlers
{
	internal class ClientSecretAuthenticationHandler : AuthenticationHandler<ClientSecretAuthenticationOptions>
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly Regex _basicAuthenticationRegex = new("^([a-f0-9]{32}):([a-f0-9]{32})$", RegexOptions.Compiled);

        #endregion

        #region Constructor

        public ClientSecretAuthenticationHandler(ICommandBus commandBus, IDataProtectionProvider dataProtectionProvider, IOptionsMonitor<ClientSecretAuthenticationOptions> clientSecretAuthenticationOptions, ILoggerFactory loggerFactory, UrlEncoder urlEncoder) 
            : base(clientSecretAuthenticationOptions, loggerFactory, urlEncoder)
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
            AuthenticationHeaderValue authenticationHeaderValue = GetAuthenticationHeaderValue(Context.Request);
            if (authenticationHeaderValue == null)
            {
                return AuthenticateResult.Fail("Authentication header is missing in the submitted request.");
            }
            if (string.IsNullOrWhiteSpace(authenticationHeaderValue.Parameter))
            {
	            return AuthenticateResult.Fail("No parameter has been defined within the submitted authentication header.");
            }

			Match match = _basicAuthenticationRegex.Match(Encoding.UTF8.GetString(Convert.FromBase64String(authenticationHeaderValue.Parameter)));
            if (string.CompareOrdinal(authenticationHeaderValue.Scheme, Scheme.Name) != 0 || match.Success == false)
            {
                return AuthenticateResult.Fail("Values within the submitted authentication header is invalid.");
            }

            string clientId = match.Groups[1].Value;
            string clientSecret = match.Groups[2].Value;

            IAuthenticateClientSecretCommand command = SecurityCommandFactory.BuildAuthenticateClientSecretCommand(clientId, clientSecret, Scheme.Name, value => _dataProtectionProvider.CreateProtector("TokenProtection").Protect(value));
            ClaimsPrincipal authenticatedClaimsPrincipal = await _commandBus.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(command);
            if (authenticatedClaimsPrincipal == null)
            {
                return AuthenticateResult.Fail("Unknown client id or wrong client secret.");
            }

            AuthenticationTicket authenticationTicket = new AuthenticationTicket(authenticatedClaimsPrincipal, Scheme.Name);

            return AuthenticateResult.Success(authenticationTicket);
        }

        private static AuthenticationHeaderValue GetAuthenticationHeaderValue(HttpRequest request)
        {
            NullGuard.NotNull(request, nameof(request));

            if (string.IsNullOrWhiteSpace(request.Headers["Authorization"]))
            {
                return null;
            }

            return AuthenticationHeaderValue.TryParse(request.Headers["Authorization"], out var authenticationHeader)
	            ? authenticationHeader
	            : null;
        }

        #endregion
    }
}