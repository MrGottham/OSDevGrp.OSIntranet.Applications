using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using System.Net;
using System.Net.Mime;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home;

[Authorize(Policy = Policies.AuthenticatedUser)]
[ApiController]
[Route("api/home")]
public class HomeController : ControllerBase
{
    #region Methods

    [AllowAnonymous]
    [HttpGet("index")]
    [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK, MediaTypeNames.Text.Plain)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]
    public Task<IActionResult> IndexAsync()
    {
        return Task.FromResult<IActionResult>(Ok("Welcome to OS Development Group Backend For Frontend (BFF) API"));
    }

    #endregion
}