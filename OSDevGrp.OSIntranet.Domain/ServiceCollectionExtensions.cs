using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainLogic(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            return AddAccountingLogic(serviceCollection);
        }

        private static IServiceCollection AddAccountingLogic(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddTransient<IPostingWarningCalculator, PostingWarningCalculator>();
        }
    }
}