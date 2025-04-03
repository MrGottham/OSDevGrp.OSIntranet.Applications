using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Local.HealthMonitor;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Models.Local;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Options;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Models;

public class IndexPageModel : PageModel
{
    #region Private variables

    private readonly IWebHostEnvironment _environment;
    private readonly IOptions<WebApiOptions> _webApiOptions;
    private IQueryFeature<HealthMonitorRequest, HealthMonitorResponse> _healthMonitorQueryFeature;
    private string? _title;
    private string? _description;
    private IEnumerable<DependencyHealthResultModel>? _dependencies;

    #endregion

    #region Constructor

    public IndexPageModel(IWebHostEnvironment environment, IOptions<WebApiOptions> webApiOptions, IQueryFeature<HealthMonitorRequest, HealthMonitorResponse> healthMonitorQueryFeature)
    {
        _environment = environment;
        _webApiOptions = webApiOptions;
        _healthMonitorQueryFeature = healthMonitorQueryFeature;
    }

    #endregion

    #region Properties

    public string Title => string.IsNullOrWhiteSpace(_title) == false ? _title : throw new InvalidOperationException($"{nameof(Title)} has not been set.");

    public string Description => string.IsNullOrWhiteSpace(_description) == false ? _description : throw new InvalidOperationException($"{nameof(Description)} has not been set.");

    public Uri? OpenApiDocumentUrl { get; private set; }

    public string? OpenApiDocumentName { get; private set; }

    public IEnumerable<DependencyHealthResultModel> Dependencies => _dependencies ?? throw new InvalidOperationException($"{nameof(Dependencies)} has not been set.");

    #endregion

    #region Methods

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken) 
    {
        _title = ProgramHelper.GetTitle();
        _description = ProgramHelper.GetDescription();

        OpenApiDocumentUrl = ProgramHelper.GetOpenApiDocumentUrl(_environment);
        OpenApiDocumentName = OpenApiDocumentUrl != null ? ProgramHelper.GetOpenApiDocumentName() : null;

        _dependencies = await GetDependenciesHealthAsync(_webApiOptions.Value, cancellationToken);

        return Page();
    }

    private async Task<IEnumerable<DependencyHealthResultModel>> GetDependenciesHealthAsync(WebApiOptions webApiOptions, CancellationToken cancellationToken)
    {
        DependencyHealthModel[] dependencies =
        [
            new DependencyHealthModel("OS Development Group Web API", new Uri($"{webApiOptions.AsEndpointAddressUrl().AbsoluteUri}health", UriKind.Absolute))
        ];

        HealthMonitorRequest healthMonitorRequest = new HealthMonitorRequest(dependencies, Guid.NewGuid(), LocalSecurityContext.None);
        HealthMonitorResponse healthMonitorResponse = await _healthMonitorQueryFeature.ExecuteAsync(healthMonitorRequest, cancellationToken); 

        return healthMonitorResponse.Dependencies;
    }

    #endregion
}