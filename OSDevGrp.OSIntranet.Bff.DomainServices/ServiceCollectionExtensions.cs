using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Bff.DomainServices.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureCancellation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureHumanVerifier;
using OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeatureLogging;
using OSDevGrp.OSIntranet.Bff.DomainServices.Cqs.PipelineExtensions.FeaturePermissionVerifier;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.BuildInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.BuildInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DependencyHealth;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMemoryCache();

        return serviceCollection.AddSingleton(TimeProvider.System)
            .AddSingleton<IHashGenerator, HashGenerator>()
            .AddSingleton<IVerificationCodeStorage, VerificationCodeStorage>()
            .AddSingleton<IBuildInfoProvider, BuildInfoProvider>()
            .AddSingleton<IStaticTextProvider, StaticTextProvider>()
            .AddTransient<IUserInfoProvider, UserInfoProvider>()
            .AddTransient<IPermissionValidator, PermissionValidator>()
            .AddTransient<IPermissionChecker, UserHelper>()
            .AddTransient<IUserHelper, UserHelper>()
            .AddTransient<IVerificationCodeGenerator, VerificationCodeGenerator>()
            .AddTransient<ICaptchaGenerator, CaptchaGenerator>()
            .AddTransient<IVerificationCodeVerifier, VerificationCodeVerifier>()
            .AddTransient<IDependencyHealthMonitor, DependencyHealthMonitor>()
            .AddTransient<IAccountingTextsBuilder, AccountingTextsBuilder>()
            .AddTransient<IAccountTextsBuilder, AccountTextsBuilder>()
            .AddTransient<IBudgetAccountTextsBuilder, BudgetAccountTextsBuilder>()
            .AddTransient<IContactAccountTextsBuilder, ContactAccountTextsBuilder>()
            .AddTransient<IRequiredValueRuleFactory, RequiredValueRuleFactory>()
            .AddTransient<IMinLengthRuleFactory, MinLengthRuleFactory>()
            .AddTransient<IMaxLengthRuleFactory, MaxLengthRuleFactory>()
            .AddTransient<IShouldBeIntegerRuleFactory, ShouldBeIntegerRuleFactory>()
            .AddTransient<IMinValueRuleFactory, MinValueRuleFactory>()
            .AddTransient<IMaxValueRuleFactory, MaxValueRuleFactory>()
            .AddTransient<IPatternRuleFactory, PatternRuleFactory>()
            .AddTransient<IOneOfRuleFactory, OneOfRuleFactory>()
            .AddTransient<IExtendedValidationRuleSetBuilder, ExtendedValidationRuleSetBuilder>()
            .AddTransient<IAccountingNumberRuleSetBuilder, AccountingNumberRuleSetBuilder>()
            .AddTransient<IAccountingNameRuleSetBuilder, AccountingNameRuleSetBuilder>()
            .AddTransient<IBalanceBelowZeroRuleSetBuilder, BalanceBelowZeroRuleSetBuilder>()
            .AddTransient<IBackDatingRuleSetBuilder, BackDatingRuleSetBuilder>()
            .AddTransient<IAccountingRuleSetBuilder, AccountingRuleSetBuilder>()
            .AddTransient<ILetterHeadNumberRuleSetBuilder, LetterHeadNumberRuleSetBuilder>()
            .AddFeatures(featureSetupOptions => featureSetupOptions.AddPipelineExtensions(GetPipelineExtensions()), typeof(ServiceCollectionExtensions).Assembly);
    }

    private static IEnumerable<IPipelineExtension> GetPipelineExtensions()
    {
        yield return new FeatureLoggingPipelineExtension();
        yield return new FeaturePermissionVerifierPipelineExtension();
        yield return new FeatureHumanVerifierPipelineExtension();
        yield return new FeatureCancellationPipelineExtension();
    }
}