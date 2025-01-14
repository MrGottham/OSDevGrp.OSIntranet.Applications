using Microsoft.Extensions.DependencyInjection;

namespace OSDevGrp.OSIntranet.WebApi.ClientApi;

public static class ServiceCollectionExtensions
{
    #region Methods

    public static IServiceCollection AddWebApiClient(this IServiceCollection serviceCollection, Action<IHttpClientBuilder> buildHttpClient, Action<HttpClient> configureHttpClient)
    {
        IHttpClientBuilder httpClientBuilder = serviceCollection.AddHttpClient<IWebApiClient, WebApiClient>();
        buildHttpClient(httpClientBuilder);
        httpClientBuilder.ConfigureHttpClient(configureHttpClient);

        return serviceCollection.AddTransient<IWebApiClient, WebApiClient>();
    }

    #endregion
}