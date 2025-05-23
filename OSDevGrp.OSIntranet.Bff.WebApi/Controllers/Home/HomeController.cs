using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.CookieConsent;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Error;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Index;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home;

[Authorize(Policy = Policies.AuthenticatedUser)]
[ApiController]
[Route("api/home")]
public class HomeController : ControllerBase
{
    #region Private variables

    private readonly IFormatProvider _formatProvider;
    private readonly ISecurityContextProvider _securityContextProvider;

    #endregion

    #region Constructor

    public HomeController(IFormatProvider formatProvider, ISecurityContextProvider securityContextProvider)
    {
        _formatProvider = formatProvider;
        _securityContextProvider = securityContextProvider;
    }

    #endregion

    #region Methods

    [AllowAnonymous]
    [HttpGet("index")]
    [ProducesResponseType(typeof(IndexResponseDto), (int) HttpStatusCode.OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> IndexAsync([FromServices] IQueryFeature<IndexRequest, IndexResponse> queryFeature, CancellationToken cancellationToken)
    {
        ISecurityContext securityContext = await _securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken);

        IndexRequest indexRequest = new IndexRequest(Guid.NewGuid(), Assembly.GetExecutingAssembly(), _formatProvider, securityContext);
        IndexResponse indexResponse = await queryFeature.ExecuteAsync(indexRequest, cancellationToken);

        return Ok(IndexResponseDto.Map(indexResponse));
    }

    [AllowAnonymous]
    [HttpGet("cookie-consent")]
    [ProducesResponseType(typeof(CookieConsentResponseDto), (int) HttpStatusCode.OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> CookieConsentAsync([FromServices] IQueryFeature<CookieConsentRequest, CookieConsentResponse> queryFeature, [FromQuery][Required][MinLength(1)] string applicationName, CancellationToken cancellationToken)
    {
        ISecurityContext securityContext = await _securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken);

        CookieConsentRequest cookieConsentRequest = new CookieConsentRequest(Guid.NewGuid(), applicationName, _formatProvider, securityContext);
        CookieConsentResponse cookieConsentResponse = await queryFeature.ExecuteAsync(cookieConsentRequest, cancellationToken);

        return Ok(CookieConsentResponseDto.Map(cookieConsentResponse));
    }

    [AllowAnonymous]
    [HttpGet("error")]
    [ProducesResponseType(typeof(ErrorResponseDto), (int) HttpStatusCode.OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> ErrorAsync([FromServices] IQueryFeature<ErrorRequest, ErrorResponse> queryFeature, [FromQuery][Required][MinLength(1)] string errorMessage, CancellationToken cancellationToken)
    {
        ISecurityContext securityContext = await _securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken);

        ErrorRequest errorRequest = new ErrorRequest(Guid.NewGuid(), errorMessage, _formatProvider, securityContext);
        ErrorResponse errorResponse = await queryFeature.ExecuteAsync(errorRequest, cancellationToken);

        return Ok(ErrorResponseDto.Map(errorResponse));
    }

    #endregion
}