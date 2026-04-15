using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Security.GenerateVerification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.AccessDeniedContent;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.Verification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security;

[Authorize(Policy = Policies.AuthenticatedUser)]
[ApiController]
[Route("api/security")]
public class SecurityController : ControllerBase
{
    #region Private variables

    private readonly IProblemDetailsFactory _problemDetailsFactory;
    private readonly ITrustedDomainResolver _trustedDomainResolver;
    private readonly IFormatProvider _formatProvider;
    private readonly ISecurityContextProvider _securityContextProvider;

    #endregion

    #region Constructor

    public SecurityController(IProblemDetailsFactory problemDetailsFactory, ITrustedDomainResolver trustedDomainResolver, IFormatProvider formatProvider, ISecurityContextProvider securityContextProvider)
    {
        _problemDetailsFactory = problemDetailsFactory;
        _trustedDomainResolver = trustedDomainResolver;
        _formatProvider = formatProvider;
        _securityContextProvider = securityContextProvider;
    }

    #endregion

    #region Methods

    [AllowAnonymous]
    [HttpGet("login")]
    [ProducesResponseType((int) HttpStatusCode.Redirect)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public IActionResult Login([FromQuery][Required] string returnUrl)
    {
        Uri? validatedReturnUri = GetValidatedReturnUri(returnUrl, out IActionResult? validationResult);
        if (validationResult != null)
        {
            return validationResult;
        }

        AuthenticationProperties authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = validatedReturnUri!.AbsoluteUri
        };
        return Challenge(authenticationProperties, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet("logout")]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public IActionResult Logout([FromQuery][Required] string returnUrl)
    {
        Uri? validatedReturnUri = GetValidatedReturnUri(returnUrl, out IActionResult? validationResult);
        if (validatedReturnUri == null && validationResult != null)
        {
            return validationResult;
        }

        AuthenticationProperties authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = validatedReturnUri!.AbsoluteUri
        };
        return SignOut(authenticationProperties, OpenIdConnectDefaults.AuthenticationScheme, Schemes.Internal);
    }

    [HttpGet("userinfo")]
    [ProducesResponseType(typeof(UserInfoResponseDto), (int)HttpStatusCode.OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> UserInfoAsync([FromServices] IQueryFeature<UserInfoRequest, UserInfoResponse> queryFeature, CancellationToken cancellationToken)
    {
        ISecurityContext securityContext = await _securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken);

        UserInfoRequest userInfoRequest = new UserInfoRequest(Guid.NewGuid(), _formatProvider, securityContext);
        UserInfoResponse userInfoResponse = await queryFeature.ExecuteAsync(userInfoRequest, cancellationToken);

        return Ok(UserInfoResponseDto.Map(userInfoResponse));
    }

    [AllowAnonymous]
    [HttpGet("accessdenied")]
    [ProducesResponseType((int) HttpStatusCode.Redirect)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public IActionResult AccessDenied() => RedirectToPage("/AccessDenied");

    [AllowAnonymous]
    [HttpGet("accessdenied/content")]
    [ProducesResponseType(typeof(AccessDeniedContentResponseDto), (int) HttpStatusCode.OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> AccessDeniedContentAsync([FromServices] IQueryFeature<AccessDeniedContentRequest, AccessDeniedContentResponse> queryFeature, CancellationToken cancellationToken)
    {
        ISecurityContext securityContext = await _securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken);

        AccessDeniedContentRequest accessDeniedContentRequest = new AccessDeniedContentRequest(Guid.NewGuid(), _formatProvider, securityContext);
        AccessDeniedContentResponse accessDeniedContentResponse = await queryFeature.ExecuteAsync(accessDeniedContentRequest, cancellationToken);

        return Ok(AccessDeniedContentResponseDto.Map(accessDeniedContentResponse));
    }

    [HttpPost("verification")]
    [ProducesResponseType(typeof(GenerateVerificationResponseDto), (int)HttpStatusCode.OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> GenerateVerificationAsync([FromServices] ICommandFeature<GenerateVerificationRequest> commandFeature, CancellationToken cancellationToken)
    {
        ISecurityContext securityContext = await _securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken);

        GenerateVerificationResponseDto? generateVerificationResponseDto = null;
        Action<string, IReadOnlyCollection<byte>, DateTimeOffset> onVerificationCreated = (verificationKey, verificationImage, expires) =>
        {
            generateVerificationResponseDto = new GenerateVerificationResponseDto
            {
                VerificationKey = verificationKey,
                VerificationImage = Convert.ToBase64String(verificationImage.ToArray()),
                Expires = expires
            };
        };

        GenerateVerificationRequest generateVerificationRequest = new GenerateVerificationRequest(Guid.NewGuid(), onVerificationCreated, securityContext);
        await commandFeature.ExecuteAsync(generateVerificationRequest, cancellationToken);

        return Ok(generateVerificationResponseDto!);
    }

    [HttpPost("verification/verify")]
    [ProducesResponseType(typeof(VerificationResponseDto), (int)HttpStatusCode.OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> VerifyAsync([FromServices] IQueryFeature<VerificationRequest, VerificationResponse> queryFeature, [FromBody][Required] VerificationRequestDto verificationRequestDto, CancellationToken cancellationToken)
    {
        ISecurityContext securityContext = await _securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken);

        VerificationRequest verificationRequest = new VerificationRequest(Guid.NewGuid(), verificationRequestDto.VerificationKey, verificationRequestDto.VerificationCode, securityContext);
        VerificationResponse verificationResponse = await queryFeature.ExecuteAsync(verificationRequest, cancellationToken);

        return Ok(VerificationResponseDto.Map(verificationResponse));
    }

    private Uri? GetValidatedReturnUri(string returnUrl, out IActionResult? actionResult)
    {
        actionResult = null;

        if (string.IsNullOrWhiteSpace(returnUrl) || Uri.TryCreate(returnUrl, UriKind.Absolute, out Uri? returnUri) == false || returnUri.IsWellFormedOriginalString() == false)
        {
            BadRequestObjectResult result = BadRequest(_problemDetailsFactory.CreateProblemDetailsForBadRequest(HttpContext.Request, "The given return URL is not an absolute URL."));
            result.ContentTypes.Add(MediaTypeNames.Application.ProblemJson);

            actionResult = result;

            return null;
        }

        if (_trustedDomainResolver.IsTrustedDomain(returnUri) == false)
        {
            BadRequestObjectResult result = BadRequest(_problemDetailsFactory.CreateProblemDetailsForBadRequest(HttpContext.Request, "The given return URL is not a valid URL in this context."));
            result.ContentTypes.Add(MediaTypeNames.Application.ProblemJson);

            actionResult = result;

            return null;
        }

        return returnUri;
    }

    #endregion
}