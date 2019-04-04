using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
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
        private readonly IConverter _securityModelConverter = new SecurityModelConverter();
        private readonly IConverter _coreModelConverter = new CoreModelConverter();
        private readonly Regex _basicAuthenticationRegex = new Regex("^([a-f0-9]{32}):([a-f0-9]{32})$", RegexOptions.Compiled);

        #endregion

        #region Constructor

        public SecurityController(ICommandBus commandBus, ISecurityContextReader securityContextReader)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(securityContextReader, nameof(securityContextReader));

            _commandBus = commandBus;
            _securityContextReader = securityContextReader;
        }

        #endregion

        #region Methods

        [AllowAnonymous]
        [Route("/api/authenticate")]
        [HttpPost("authenticate")]
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

        #endregion
    }
}
