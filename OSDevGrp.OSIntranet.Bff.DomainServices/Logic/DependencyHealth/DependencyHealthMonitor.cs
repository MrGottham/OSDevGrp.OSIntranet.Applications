using OSDevGrp.OSIntranet.Bff.DomainServices.Models.Local;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DependencyHealth;

internal class DependencyHealthMonitor : IDependencyHealthMonitor
{
    #region Privat variables

    private readonly IHttpClientFactory _httpClientFactory;

    #endregion

    #region Constructor

    public DependencyHealthMonitor(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    #endregion

    #region Methods

    public async Task<DependencyHealthResultModel> CheckHealthAsync(DependencyHealthModel dependencyHealthModel, CancellationToken cancellationToken = default)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient($"{GetType().Namespace}.{GetType().Name}:Client");

        try
        {
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, dependencyHealthModel.HealthEndpoint);
            using HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

            return dependencyHealthModel.GenerateResult(httpResponseMessage.StatusCode == HttpStatusCode.OK);
        }
        catch (HttpRequestException)
        {
            return dependencyHealthModel.GenerateResult(false);
        }
    }

    #endregion
}