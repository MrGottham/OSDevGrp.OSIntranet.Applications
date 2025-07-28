using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accounting;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingPreCreation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accountings;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting;

[Authorize(Policy = Policies.Accounting)]
[ApiController]
[Route("api/accounting")]
public class AccountingController : ControllerBase
{
    #region Private variables

    private readonly TimeProvider _timeProvider;
    private readonly IFormatProvider _formatProvider;
    private readonly ISecurityContextProvider _securityContextProvider;

    #endregion

    #region Constructor

    public AccountingController(TimeProvider timeProvider, IFormatProvider formatProvider, ISecurityContextProvider securityContextProvider)
    {
        _timeProvider = timeProvider;
        _formatProvider = formatProvider;
        _securityContextProvider = securityContextProvider;
    }

    #endregion

    #region Methods

    [HttpGet()]
    [HttpGet("acccountings")]
    [ProducesResponseType(typeof(AccountingsResponseDto), (int)HttpStatusCode.OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> AccountingsAsync([FromServices] IQueryFeature<AccountingsRequest, AccountingsResponse> queryFeature, CancellationToken cancellationToken)
    {
        ISecurityContext securityContext = await _securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken);

        AccountingsRequest accountingsRequest = new AccountingsRequest(Guid.NewGuid(), _formatProvider, securityContext);
        AccountingsResponse accountingsResponse = await queryFeature.ExecuteAsync(accountingsRequest, cancellationToken);

        return Ok(AccountingsResponseDto.Map(accountingsResponse));
    }

    [Authorize(Policy = Policies.AccountingCreator)]
    [HttpGet("create")]
    [ProducesResponseType(typeof(AccountingPreCreationResponseDto), (int)HttpStatusCode.OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> AccountingPreCreationAsync([FromServices] IQueryFeature<AccountingPreCreationRequest, AccountingPreCreationResponse> queryFeature, CancellationToken cancellationToken)
    {
        ISecurityContext securityContext = await _securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken);

        AccountingPreCreationRequest accountingPreCreationRequest = new AccountingPreCreationRequest(Guid.NewGuid(), _formatProvider, securityContext);
        AccountingPreCreationResponse accountingPreCreationResponse = await queryFeature.ExecuteAsync(accountingPreCreationRequest, cancellationToken);

        return Ok(AccountingPreCreationResponseDto.Map(accountingPreCreationResponse));
    }

    [Authorize(Policy = Policies.AccountingViewer)]
    [HttpGet("{accountingNumber:int}")]
    [ProducesResponseType(typeof(AccountingResponseDto), (int)HttpStatusCode.OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public async Task<IActionResult> AccountingAsync([FromServices] IQueryFeature<AccountingRequest, AccountingResponse> queryFeature, [FromRoute][Required][Range(AccountingRuleSetSpecifications.AccountingNumberMinValue, AccountingRuleSetSpecifications.AccountingNumberMaxValue)] int accountingNumber, CancellationToken cancellationToken, [FromQuery] DateTimeOffset? statusDate = null)
    {
        ISecurityContext securityContext = await _securityContextProvider.GetCurrentSecurityContextAsync(cancellationToken);

        AccountingRequest accountingRequest = new AccountingRequest(Guid.NewGuid(), accountingNumber, ResolveStatusDate(statusDate), _formatProvider, securityContext);
        AccountingResponse accountingResponse = await queryFeature.ExecuteAsync(accountingRequest, cancellationToken);

        return Ok(AccountingResponseDto.Map(accountingResponse));
    }

    private DateTimeOffset ResolveStatusDate(DateTimeOffset? value)
    {
        return new DateTimeOffset((value ?? _timeProvider.GetLocalNow()).Date);
    }

    #endregion
}