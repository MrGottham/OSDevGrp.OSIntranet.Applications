using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Models;

public class AccessDeniedModel : PageModel
{
    #region Methods

    public Task<IActionResult> OnGetAsync(CancellationToken _) => Task.FromResult<IActionResult>(Page());

    #endregion
}