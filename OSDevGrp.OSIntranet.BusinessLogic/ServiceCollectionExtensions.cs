using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogicValidators(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddSingleton<IValidator, Validator>()
                .AddSingleton<IIntegerValidator, IntegerValidator>()
                .AddSingleton<IDecimalValidator, DecimalValidator>()
                .AddSingleton<IStringValidator, StringValidator>()
                .AddSingleton<IDateTimeValidator, DateTimeValidator>()
                .AddSingleton<IObjectValidator, ObjectValidator>()
                .AddSingleton<IEnumerableValidator, EnumerableValidator>();
        }

        public static IServiceCollection AddBusinessLogicHelpers(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddTransient<ITokenHelper, TokenHelper>()
                .AddTransient<IClaimResolver, ClaimResolver>()
                .AddTransient<IContactToCsvConverter, ContactToCsvConverter>()
                .AddTransient<ICountryHelper, CountryHelper>()
                .AddTransient<IAccountingHelper, AccountingHelper>()
                .AddScoped<IStatusDateProvider, StatusDateProvider>()
                .AddScoped(serviceProvider => (IStatusDateSetter)serviceProvider.GetRequiredService<IStatusDateProvider>())
                .AddTransient<IAccountToCsvConverter, AccountToCsvConverter>()
                .AddTransient<IBudgetAccountToCsvConverter, BudgetAccountToCsvConverter>()
                .AddTransient<IContactAccountToCsvConverter, ContactAccountToCsvConverter>()
                .AddTransient<IHashKeyGenerator, HashKeyGenerator>()
                .AddTransient<IKeyGenerator, KeyGenerator>();
        }
    }
}