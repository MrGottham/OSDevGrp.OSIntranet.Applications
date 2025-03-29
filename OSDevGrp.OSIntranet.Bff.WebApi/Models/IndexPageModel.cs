using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Models;

public class IndexPageModel : PageModel
{
    #region Private variables

    private readonly IWebHostEnvironment _environment;
    private string? _title;
    private string? _description;

    #endregion

    #region Constructor

    public IndexPageModel(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    #endregion

    #region Properties

    public string Title => string.IsNullOrWhiteSpace(_title) == false ? _title : throw new InvalidOperationException($"{nameof(Title)} has not been set.");

    public string Description => string.IsNullOrWhiteSpace(_description) == false ? _description : throw new InvalidOperationException($"{nameof(Description)} has not been set.");

    public Uri? OpenApiDocumentUrl { get; private set; }

    public string? OpenApiDocumentName { get; private set; }

    #endregion

    #region Methods

    public Task<IActionResult> OnGetAsync(CancellationToken cancellationToken) 
    {
        return Task.Run<IActionResult>(() => 
        {
            _title = ProgramHelper.GetTitle();
            _description = ProgramHelper.GetDescription();

            OpenApiDocumentUrl = ProgramHelper.GetOpenApiDocumentUrl(_environment);
            OpenApiDocumentName = OpenApiDocumentUrl != null ? ProgramHelper.GetOpenApiDocumentName() : null;

            return Page();
        }, 
        cancellationToken);
    }

    #endregion
}