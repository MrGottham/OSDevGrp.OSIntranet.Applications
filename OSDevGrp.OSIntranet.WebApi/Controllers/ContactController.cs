using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.WebApi.Security;

namespace OSDevGrp.OSIntranet.WebApi.Controllers
{
    [Authorize(Policy = Policies.ContactPolicy)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
    }
}