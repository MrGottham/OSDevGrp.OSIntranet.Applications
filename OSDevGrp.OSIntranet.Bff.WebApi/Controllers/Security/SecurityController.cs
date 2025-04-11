using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    #endregion

    #region Constructor

    public SecurityController(IProblemDetailsFactory problemDetailsFactory, ITrustedDomainResolver trustedDomainResolver)
    {
        _problemDetailsFactory = problemDetailsFactory;
        _trustedDomainResolver = trustedDomainResolver;
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

    [AllowAnonymous]
    [HttpGet("accessdenied")]
    [ProducesResponseType((int) HttpStatusCode.Redirect)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public IActionResult AccessDenied() => RedirectToPage("/AccessDenied");

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