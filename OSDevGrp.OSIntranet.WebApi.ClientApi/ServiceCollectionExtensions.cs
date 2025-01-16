using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.WebApi.ClientApi.Handlers;

namespace OSDevGrp.OSIntranet.WebApi.ClientApi;

public static class ServiceCollectionExtensions
{
    #region Methods

    public static IServiceCollection AddWebApiClient(this IServiceCollection serviceCollection, Action<IServiceProvider, HttpClient> configureHttpClient, Action<IHttpClientBuilder> buildHttpClient)
    {
        serviceCollection.AddTransient<WebApiClientHandler>();

        IHttpClientBuilder httpClientBuilder = serviceCollection.AddHttpClient<IWebApiClient, WebApiClient>(configureHttpClient)
            .ConfigurePrimaryHttpMessageHandler<WebApiClientHandler>();

        buildHttpClient(httpClientBuilder);

        return serviceCollection;
    }

    #endregion
}