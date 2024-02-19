using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using OSDevGrp.OSIntranet.WebApi.Security;
using System.Text;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.WebApi.Controllers
{
	[ApiVersion("1.0")]
    [ApiVersionNeutral]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
		#region Private variables

		private readonly ICommandBus _commandBus;
        private readonly IAcmeChallengeResolver _acmeChallengeResolver;
        private readonly IConverter _securityModelConverter = new SecurityModelConverter();

        #endregion

        #region Constructor

        public SecurityController(ICommandBus commandBus, IAcmeChallengeResolver acmeChallengeResolver)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(acmeChallengeResolver, nameof(acmeChallengeResolver));

            _commandBus = commandBus;
            _acmeChallengeResolver = acmeChallengeResolver;
        }

        #endregion

        #region Methods

        [Authorize(Policy = Policies.AcquireTokenPolicy)]
        [HttpPost("/api/oauth/token")]
        public async Task<ActionResult<AccessTokenModel>> AcquireToken([FromForm(Name = "grant_type")]string grantType)
        {
            if (string.IsNullOrWhiteSpace(grantType))
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNullOrWhiteSpace, nameof(grantType))
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(nameof(grantType))
                    .Build();
            }

            IGenerateTokenCommand generateTokenCommand = SecurityCommandFactory.BuildGenerateTokenCommand();
            IToken token = await _commandBus.PublishAsync<IGenerateTokenCommand, IToken>(generateTokenCommand);

            if (string.CompareOrdinal(grantType, "client_credentials") != 0 || token == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedUser).Build();
            }

            return Ok(_securityModelConverter.Convert<IToken, AccessTokenModel>(token));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/.well-known/acme-challenge/{challengeToken}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public IActionResult AcmeChallenge(string challengeToken)
        {
            if (string.IsNullOrWhiteSpace(challengeToken))
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNullOrWhiteSpace, nameof(challengeToken))
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(nameof(challengeToken))
                    .Build();
            }

            string constructedKeyAuthorization = _acmeChallengeResolver.GetConstructedKeyAuthorization(challengeToken);
            if (string.IsNullOrWhiteSpace(constructedKeyAuthorization))
            {
                throw new IntranetExceptionBuilder(ErrorCode.CannotRetrieveAcmeChallengeForToken).Build();
            }

            return File(Encoding.UTF8.GetBytes(constructedKeyAuthorization), "application/octet-stream");
        }

        #endregion
    }
}