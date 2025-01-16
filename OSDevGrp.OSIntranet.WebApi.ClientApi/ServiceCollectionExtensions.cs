using Microsoft.Extensions.DependencyInjection;

namespace OSDevGrp.OSIntranet.WebApi.ClientApi;

public static class ServiceCollectionExtensions
{
    #region Methods

    public static IServiceCollection AddWebApiClient(this IServiceCollection serviceCollection, Action<IServiceProvider, HttpClient> configureHttpClient, Action<IHttpClientBuilder> buildHttpClient)
    {
        IHttpClientBuilder httpClientBuilder = serviceCollection.AddHttpClient<IWebApiClient, WebApiClient>(configureHttpClient);
        buildHttpClient(httpClientBuilder);

        return serviceCollection;
    }

    #endregion
}