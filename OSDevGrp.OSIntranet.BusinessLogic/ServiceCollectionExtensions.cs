using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Helpers;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Helpers;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogicHelpers(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddTransient<ITokenHelper, TokenHelper>();
        }
    }
}
