﻿using Microsoft.Extensions.DependencyInjection;
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

            return serviceCollection.AddSingleton<IValidator, Validator>()
                .AddSingleton<IIntegerValidator, IntegerValidator>()
                .AddSingleton<IDecimalValidator, DecimalValidator>()
                .AddSingleton<IStringValidator, StringValidator>()
                .AddSingleton<IDateTimeValidator, DateTimeValidator>()
                .AddSingleton<IObjectValidator, ObjectValidator>();
        }

        public static IServiceCollection AddBusinessLogicHelpers(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddTransient<ITokenHelper, TokenHelper>();
        }
    }
}