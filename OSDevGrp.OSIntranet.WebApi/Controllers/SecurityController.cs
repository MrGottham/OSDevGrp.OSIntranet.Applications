using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.WebApi.Helpers.Extensions;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using OSDevGrp.OSIntranet.WebApi.Security;
using System;
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
        private readonly IQueryBus _queryBus;
        private readonly IConverter _securityModelConverter = new SecurityModelConverter();

        #endregion

        #region Constructor

        public SecurityController(ICommandBus commandBus, IQueryBus queryBus)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(queryBus, nameof(queryBus));

            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        #endregion

        #region Methods

        [AllowAnonymous]
        [HttpGet("/api/oauth/authorize")]
        public Task<IActionResult> Authorize()
        {
            throw new NotImplementedException();
        }

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

        [Authorize(Policy = Policies.UserInfoPolity)]
        [HttpGet("/api/oauth/userinfo")]
        public Task<IActionResult> UserInfo()
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpGet("/api/oauth/jwks")]
        public Task<IActionResult> JsonWebKeys()
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpGet("/.well-known/openid-configuration")]
        public async Task<ActionResult<OpenIdProviderConfigurationModel>> OpenIdProviderConfiguration()
        {
            IGetOpenIdProviderConfigurationQuery query = SecurityQueryFactory.BuildGetOpenIdProviderConfigurationQuery(
                Url.AbsoluteAction(nameof(Authorize), "Security"),
                Url.AbsoluteAction(nameof(AcquireToken), "Security"),
                Url.AbsoluteAction(nameof(JsonWebKeys), "Security"),
                Url.AbsoluteAction(nameof(UserInfo), "Security"));
            IOpenIdProviderConfiguration openIdProviderConfiguration = await _queryBus.QueryAsync<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration>(query);

            return Ok(_securityModelConverter.Convert<IOpenIdProviderConfiguration, OpenIdProviderConfigurationModel>(openIdProviderConfiguration));
        }

        [AllowAnonymous]
        [HttpGet("/.well-known/acme-challenge/{challengeToken}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> AcmeChallenge(string challengeToken)
        {
            if (string.IsNullOrWhiteSpace(challengeToken))
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNullOrWhiteSpace, nameof(challengeToken))
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(nameof(challengeToken))
                    .Build();
            }

            byte[] constructedKeyAuthorization = await _commandBus.PublishAsync<IAcmeChallengeCommand, byte[]>(SecurityCommandFactory.BuildAcmeChallengeCommand(challengeToken));

            return File(constructedKeyAuthorization, "application/octet-stream");
        }

        #endregion
    }
}