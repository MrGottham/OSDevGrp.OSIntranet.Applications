using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Helpers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Helpers;
using OSDevGrp.OSIntranet.BusinessLogic.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogicValidators(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddTransient<IValidator, Validator>()
                .AddTransient<IIntegerValidator, IntegerValidator>()
                .AddTransient<IDecimalValidator, DecimalValidator>()
                .AddTransient<IStringValidator, StringValidator>()
                .AddTransient<IDateTimeValidator, DateTimeValidator>()
                .AddTransient<IObjectValidator, ObjectValidator>();
        }

        public static IServiceCollection AddBusinessLogicHelpers(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddTransient<ITokenHelper, TokenHelper>();
        }
    }
}
