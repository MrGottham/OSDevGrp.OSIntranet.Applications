using Microsoft.Extensions.Configuration;
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
using OSDevGrp.OSIntranet.BusinessLogic.Security.Options;
using OSDevGrp.OSIntranet.BusinessLogic.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;

namespace OSDevGrp.OSIntranet.BusinessLogic
{
	public static class ServiceCollectionExtensions
    {
		#region Methods

		public static IServiceCollection AddBusinessLogicConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
		{
			NullGuard.NotNull(serviceCollection, nameof(serviceCollection))
				.NotNull(configuration, nameof(configuration));

			return serviceCollection.Configure<TokenGeneratorOptions>(configuration.GetSection($"{SecurityConfigurationKeys.SecuritySectionName}:{SecurityConfigurationKeys.JwtSectionName}"));
		}

        public static IServiceCollection AddBusinessLogicValidators(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddSingleton<IValidator, Validator>()
                .AddSingleton<IIntegerValidator, IntegerValidator>()
                .AddSingleton<IDecimalValidator, DecimalValidator>()
                .AddSingleton<IStringValidator, StringValidator>()
                .AddSingleton<IDateTimeValidator, DateTimeValidator>()
                .AddSingleton<IObjectValidator, ObjectValidator>()
                .AddSingleton<IEnumerableValidator, EnumerableValidator>()
                .AddSingleton<IPermissionValidator, PermissionValidator>();
        }

        public static IServiceCollection AddBusinessLogicHelpers(this IServiceCollection serviceCollection)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection));

            return serviceCollection.AddTransient<IExternalTokenCreator, ExternalTokenCreator>()
	            .AddTransient<IExternalTokenClaimCreator, ExternalTokenClaimCreator>()
	            .AddTransient<ITokenGenerator, TokenGenerator>()
                .AddTransient<IClaimResolver, ClaimResolver>()
	            .AddTransient<IClaimsIdentityResolver, ClaimsIdentityResolver>()
                .AddTransient<IContactToCsvConverter, ContactToCsvConverter>()
                .AddTransient<ICountryHelper, CountryHelper>()
                .AddTransient<IAccountingHelper, AccountingHelper>()
                .AddScoped<IStatusDateProvider, StatusDateProvider>()
                .AddScoped(serviceProvider => (IStatusDateSetter)serviceProvider.GetRequiredService<IStatusDateProvider>())
                .AddTransient<IAccountToCsvConverter, AccountToCsvConverter>()
                .AddTransient<IBudgetAccountToCsvConverter, BudgetAccountToCsvConverter>()
                .AddTransient<IContactAccountToCsvConverter, ContactAccountToCsvConverter>()
                .AddTransient<IAccountGroupStatusToCsvConverter, AccountGroupStatusToCsvConverter>()
                .AddTransient<IBudgetAccountGroupStatusToCsvConverter, BudgetAccountGroupStatusToCsvConverter>()
                .AddTransient<IMonthlyAccountingStatementToMarkdownConverter, MonthlyAccountingStatementToMarkdownConverter>()
                .AddTransient<IAnnualAccountingStatementToMarkdownConverter, AnnualAccountingStatementToMarkdownConverter>()
                .AddTransient<IBalanceSheetToMarkdownConverter, BalanceSheetToMarkdownConverter>()
                .AddTransient<IContactAccountStatementToMarkdownConverter, ContactAccountStatementToMarkdownConverter>()
                .AddTransient<IHashKeyGenerator, HashKeyGenerator>()
                .AddTransient<IKeyGenerator, KeyGenerator>();
        }

        #endregion
    }
}