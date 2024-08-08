using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.WebApi.Helpers.Extensions;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using OSDevGrp.OSIntranet.WebApi.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersionNeutral]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        #region Private constants

        private const string Base64Pattern = @"([A-Za-z0-9+\/]{4})*([A-Za-z0-9+\/]{3}=|[A-Za-z0-9+\/]{2}==)?";

        #endregion

        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IConverter _securityModelConverter = new SecurityModelConverter();
        private static readonly Regex GrantTypeRegex = new("^(authorization_code|client_credentials){1}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(32));
        private static readonly Regex AuthorizationRegex = new($"^(Basic){{1}}\\s+({Base64Pattern}){{1}}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(32));
        private static readonly Regex AuthorizationParameterForClientIdAndClientSecretRegex = new($"^([a-f0-9]{{32}}){{1}}:([a-f0-9]{{32}}){{1}}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(32));

        #endregion

        #region Constructor

        public SecurityController(ICommandBus commandBus, IQueryBus queryBus, IDataProtectionProvider dataProtectionProvider)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(queryBus, nameof(queryBus))
                .NotNull(dataProtectionProvider, nameof(dataProtectionProvider));

            _commandBus = commandBus;
            _queryBus = queryBus;
            _dataProtectionProvider = dataProtectionProvider;
        }

        #endregion

        #region Methods

        [AllowAnonymous]
        [HttpGet("/api/oauth/authorize")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authorize([Required][FromQuery(Name = "response_type")] string responseType, [Required][FromQuery(Name = "client_id")] string clientId, [Required][FromQuery(Name = "redirect_uri")] string redirectUri, [Required][FromQuery(Name = "scope")] string scope, [FromQuery(Name = "state")] string state)
        {
            if (string.IsNullOrWhiteSpace(responseType))
            {
                return BadRequest(ErrorResponseModelResolver.Resolve("unsupported_response_type", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "response_type"), null, state));
            }

            if (string.CompareOrdinal(responseType, "code") != 0)
            {
                return BadRequest(ErrorResponseModelResolver.Resolve("unsupported_response_type", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state));

            }

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return BadRequest(ErrorResponseModelResolver.Resolve("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "client_id"), null, state));
            }

            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                return BadRequest(ErrorResponseModelResolver.Resolve("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "redirect_uri"), null, state));
            }

            if (Uri.TryCreate(redirectUri, UriKind.Absolute, out Uri absoluteRedirectUri) == false)
            {
                return BadRequest(ErrorResponseModelResolver.Resolve("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state));
            }

            if (string.IsNullOrWhiteSpace(scope))
            {
                return BadRequest(ErrorResponseModelResolver.Resolve("invalid_scope", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "scope"), null, state));
            }

            try
            {
                IPrepareAuthorizationCodeFlowCommand command = SecurityCommandFactory.BuildPrepareAuthorizationCodeFlowCommand(responseType, clientId, absoluteRedirectUri, scope.Split(' '), state, _dataProtectionProvider.CreateProtector("AuthorizationStateProtection").Protect);
                string authorizationState = await _commandBus.PublishAsync<IPrepareAuthorizationCodeFlowCommand, string>(command);

                return RedirectToPage("/Security/Login", new {authorizationState});
            }
            catch (IntranetValidationException ex)
            {
                return BadRequest(ErrorResponseModelResolver.Resolve(ex, state));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorResponseModelResolver.Resolve(ex, state));
            }
        }

        [AllowAnonymous]
        [HttpGet("/api/oauth/authorize/callback")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(StatusCodes.Status308PermanentRedirect)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AuthorizeCallback()
        {
            try
            {
                ClaimsPrincipal currentPrincipal = HttpContext.User;
                AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync(Schemes.Internal);
                try
                {
                    if (authenticateResult.Succeeded == false || authenticateResult.Principal?.Identity == null)
                    {
                        return Unauthorized(ErrorResponseModelResolver.Resolve(authenticateResult, null));
                    }

                    ClaimsIdentity externalClaimsIdentity = (ClaimsIdentity) authenticateResult.Principal.Identity;
                    if (externalClaimsIdentity.IsAuthenticated == false)
                    {
                        return Unauthorized(ErrorResponseModelResolver.Resolve(externalClaimsIdentity, null));
                    }

                    string mailAddress = externalClaimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
                    if (string.IsNullOrWhiteSpace(mailAddress))
                    {
                        return Unauthorized(ErrorResponseModelResolver.Resolve(externalClaimsIdentity, null));
                    }

                    if (authenticateResult.Properties.Items.Count == 0 || authenticateResult.Properties.Items.TryGetValue(KeyNames.AuthorizationStateKey, out string authorizationState) == false)
                    {
                        return Unauthorized(ErrorResponseModelResolver.Resolve(authenticateResult.Properties, null));
                    }
                    if (string.IsNullOrWhiteSpace(authorizationState))
                    {
                        return Unauthorized(ErrorResponseModelResolver.Resolve(authenticateResult.Properties, null));
                    }

                    IAuthenticateUserCommand authenticateUserCommand = SecurityCommandFactory.BuildAuthenticateUserCommand(mailAddress, authenticateResult.Principal.Claims.ToArray(), Schemes.Internal, authenticateResult.Properties.Items.AsReadOnly(), value => value);
                    ClaimsPrincipal internalClaimsPrincipal = await _commandBus.PublishAsync<IAuthenticateUserCommand, ClaimsPrincipal>(authenticateUserCommand);
                    if (internalClaimsPrincipal == null)
                    {
                        return Unauthorized(ErrorResponseModelResolver.Resolve("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null));
                    }

                    await HttpContext.SignInAsync(Schemes.Internal, internalClaimsPrincipal, authenticateResult.Properties);
                    HttpContext.User = internalClaimsPrincipal;

                    IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand = SecurityCommandFactory.BuildGenerateAuthorizationCodeCommand(authorizationState, internalClaimsPrincipal.Claims.ToArray(), _dataProtectionProvider.CreateProtector("AuthorizationStateProtection").Unprotect);
                    IAuthorizationState authorizationStateWithAuthorizationCode = await _commandBus.PublishAsync<IGenerateAuthorizationCodeCommand, IAuthorizationState>(generateAuthorizationCodeCommand);
                    if (string.IsNullOrWhiteSpace(authorizationStateWithAuthorizationCode?.AuthorizationCode.Value))
                    {
                        return Unauthorized(ErrorResponseModelResolver.Resolve("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null));
                    }

                    return RedirectPermanent(authorizationStateWithAuthorizationCode.GenerateRedirectUriWithAuthorizationCode().ToString());
                }
                finally
                {
                    await HttpContext.SignOutAsync(Schemes.Internal, authenticateResult.Properties);
                    HttpContext.User = currentPrincipal;
                }
            }
            catch (IntranetValidationException ex)
            {
                return BadRequest(ErrorResponseModelResolver.Resolve(ex, null));
            }
            catch (IntranetBusinessException ex)
            {
                return Unauthorized(ErrorResponseModelResolver.Resolve(ex, null));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorResponseModelResolver.Resolve("server_error", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, null));
            }
        }

        [AllowAnonymous]
        [HttpPost("/api/oauth/token")]
        [ProducesResponseType(typeof(AccessTokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AcquireToken([Required][FromForm(Name = "grant_type")]string grantType, [FromHeader(Name = "Authorization")]string authorization = null, [FromForm(Name = "code")]string code = null, [FromForm(Name = "client_id")]string clientId = null, [FromForm(Name = "client_secret")]string clientSecret = null, [FromForm(Name = "redirect_uri")]string redirectUri = null)
        {
            // TODO: https://datatracker.ietf.org/doc/html/rfc6749#section-4.1

            if (string.IsNullOrWhiteSpace(grantType))
            {
                return BadRequest(ErrorResponseModelResolver.Resolve("unsupported_grant_type", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "grant_type"), null, null));
            }

            if (GrantTypeRegex.IsMatch(grantType) == false)
            {
                return BadRequest(ErrorResponseModelResolver.Resolve("unsupported_grant_type", ErrorDescriptionResolver.Resolve(ErrorCode.ValueShouldMatchPattern, "grant_type", GrantTypeRegex), null, null));
            }

            ClaimsPrincipal currentPrincipal = HttpContext.User;
            try
            {
                ClaimsPrincipal principal;
                switch (grantType)
                {
                    case "authorization_code":
                        throw new NotImplementedException();

                    case "client_credentials":
                        principal = await ResolvePrincipalAsync(authorization, nameof(authorization));
                        break;

                    default:
                        throw new NotSupportedException($"Unsupported {nameof(grantType)}: {grantType}");
                }

                await HttpContext.SignInAsync(Schemes.Internal, principal);
                HttpContext.User = principal;

                IToken token = await ResolveTokenAsync();

                return Ok(_securityModelConverter.Convert<IToken, AccessTokenModel>(token));
            }
            catch (IntranetValidationException ex)
            {
                return BadRequest(ErrorResponseModelResolver.Resolve(ex, null));
            }
            catch (IntranetBusinessException ex)
            {
                return Unauthorized(ErrorResponseModelResolver.Resolve(ex, null));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorResponseModelResolver.Resolve("server_error", ErrorDescriptionResolver.Resolve(ErrorCode.UnableAuthenticateClient), null, null));
            }
            finally
            {
                await HttpContext.SignOutAsync(Schemes.Internal, null);
                HttpContext.User = currentPrincipal;
            }
        }

        [Authorize(Policy = Policies.UserInfoPolity)]
        [HttpGet("/api/oauth/userinfo")]
        public Task<IActionResult> UserInfo()
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpGet("/api/oauth/jwks")]
        public async Task<ActionResult<JsonWebKeySetModel>> JsonWebKeys()
        {
            IGetJsonWebKeySetQuery query = SecurityQueryFactory.BuildGetJsonWebKeySetQuery();
            JsonWebKeySet jsonWebKeySet = await _queryBus.QueryAsync<IGetJsonWebKeySetQuery, JsonWebKeySet>(query);

            return Ok(_securityModelConverter.Convert<JsonWebKeySet, JsonWebKeySetModel>(jsonWebKeySet));
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

        private Task<ClaimsPrincipal> ResolvePrincipalAsync(string authorization, string parameterName)
        {
            NullGuard.NotNullOrWhiteSpace(parameterName, nameof(parameterName));

            return ResolvePrincipalAsync(ToAuthenticationHeaderValue(authorization, parameterName), parameterName);
        }

        private async Task<ClaimsPrincipal> ResolvePrincipalAsync(AuthenticationHeaderValue authenticationHeaderValue, string parameterName)
        {
            NullGuard.NotNull(authenticationHeaderValue, nameof(authenticationHeaderValue))
                .NotNullOrWhiteSpace(parameterName, nameof(parameterName));

            Match match = AuthorizationParameterForClientIdAndClientSecretRegex.Match(Encoding.UTF8.GetString(Convert.FromBase64String(authenticationHeaderValue.Parameter ?? string.Empty)));
            if (match.Success == false)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueShouldMatchPattern, parameterName, AuthorizationParameterForClientIdAndClientSecretRegex)
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(parameterName)
                    .Build();
            }

            string clientId = match.Groups[1].Value;
            string clientSecret = match.Groups[2].Value;

            IAuthenticateClientSecretCommand command = SecurityCommandFactory.BuildAuthenticateClientSecretCommand(clientId, clientSecret, Schemes.Internal, value => value);
            ClaimsPrincipal principal = await _commandBus.PublishAsync<IAuthenticateClientSecretCommand, ClaimsPrincipal>(command);
            if (principal?.Identity == null || principal.Identity.IsAuthenticated == false)
            {
                throw new IntranetExceptionBuilder(ErrorCode.UnableAuthenticateClient).Build();
            }

            return principal;
        }

        private async Task<IToken> ResolveTokenAsync()
        {
            IGenerateTokenCommand generateTokenCommand = SecurityCommandFactory.BuildGenerateTokenCommand();
            IToken token = await _commandBus.PublishAsync<IGenerateTokenCommand, IToken>(generateTokenCommand);
            if (token == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedClient).Build();
            }

            return token;
        }

        private static AuthenticationHeaderValue ToAuthenticationHeaderValue(string authorization, string parameterName)
        {
            NullGuard.NotNullOrWhiteSpace(parameterName, nameof(parameterName));

            if (string.IsNullOrWhiteSpace(authorization))
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNullOrWhiteSpace, parameterName)
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(parameterName)
                    .Build();
            }

            if (AuthorizationRegex.IsMatch(authorization) == false || AuthenticationHeaderValue.TryParse(authorization, out AuthenticationHeaderValue authenticationHeaderValue) == false)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueShouldMatchPattern, parameterName, AuthorizationRegex)
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(parameterName)
                    .Build();
            }

            return authenticationHeaderValue;
        }

        #endregion
    }
}