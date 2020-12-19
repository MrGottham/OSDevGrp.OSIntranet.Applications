﻿using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.WebApi.Helpers.Security;
using OSDevGrp.OSIntranet.WebApi.Models.Core;
using OSDevGrp.OSIntranet.WebApi.Models.Security;

namespace OSDevGrp.OSIntranet.WebApi.Controllers
{
    [Authorize(Policy = "SecurityAdmin")]
    [ApiVersion("1.0")]
    [ApiVersionNeutral]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly ISecurityContextReader _securityContextReader;
        private readonly IAcmeChallengeResolver _acmeChallengeResolver;
        private readonly IConverter _securityModelConverter = new SecurityModelConverter();
        private readonly IConverter _coreModelConverter = new CoreModelConverter();
        private readonly Regex _basicAuthenticationRegex = new Regex("^([a-f0-9]{32}):([a-f0-9]{32})$", RegexOptions.Compiled);

        #endregion

        #region Constructor

        public SecurityController(ICommandBus commandBus, ISecurityContextReader securityContextReader, IAcmeChallengeResolver acmeChallengeResolver)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(securityContextReader, nameof(securityContextReader))
                .NotNull(acmeChallengeResolver, nameof(acmeChallengeResolver));

            _commandBus = commandBus;
            _securityContextReader = securityContextReader;
            _acmeChallengeResolver = acmeChallengeResolver;
        }

        #endregion

        #region Methods

        [AllowAnonymous]
        [HttpPost("/api/authorize")]
        [HttpPost("/api/authenticate")]
        [ProducesResponseType(typeof(AccessTokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AccessTokenModel>> AuthenticateAsync()
        {
            try
            {
                AuthenticationHeaderValue authenticationHeader = _securityContextReader.GetBasicAuthenticationHeader(Request);
                if (authenticationHeader == null)
                {
                    return BadRequest("An Authorization Header is missing in the submitted request.");
                }

                string credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationHeader.Parameter));
                if (_basicAuthenticationRegex.IsMatch(credentials) == false)
                {
                    return Unauthorized();
                }

                MatchCollection matchCollection = _basicAuthenticationRegex.Matches(credentials);
                string clientId = matchCollection[0].Groups[1].Value;
                string clientSecret = matchCollection[0].Groups[2].Value;
                IAuthenticateClientSecretCommand authenticateClientSecretCommand = new AuthenticateClientSecretCommand(clientId, clientSecret);
                IClientSecretIdentity clientSecretIdentity = await _commandBus.PublishAsync<IAuthenticateClientSecretCommand, IClientSecretIdentity>(authenticateClientSecretCommand);
                if (clientSecretIdentity == null)
                {
                    return Unauthorized();
                }

                return Ok(_securityModelConverter.Convert<IToken, AccessTokenModel>(clientSecretIdentity.Token));
            }
            catch (IntranetExceptionBase ex)
            {
                return BadRequest(_coreModelConverter.Convert<IntranetExceptionBase, ErrorModel>(ex));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/.well-known/acme-challenge/{challengeToken}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AcmeChallenge(string challengeToken)
        {
            NullGuard.NotNullOrWhiteSpace(challengeToken, nameof(challengeToken));

            string constructedKeyAuthorization = _acmeChallengeResolver.GetConstructedKeyAuthorization(challengeToken);
            if (string.IsNullOrWhiteSpace(constructedKeyAuthorization))
            {
                return BadRequest();
            }

            return File(Encoding.UTF8.GetBytes(constructedKeyAuthorization), "application/octet-stream");
        }

        #endregion
    }
}