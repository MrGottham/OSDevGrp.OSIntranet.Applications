using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs;

internal static class FeatureSetupExtensions
{
    #region Methods

    internal static IServiceCollection AddFeatures(this IServiceCollection serviceCollection, Action<FeatureSetupOptions> configureOptions, params Assembly[] featureAssemblies)
    {
        if (featureAssemblies.Length == 0)
        {
            throw new ArgumentException("No feature assemblies has been provided.", nameof(featureAssemblies));
        }

        FeatureSetupOptions featureSetupOptions = new FeatureSetupOptions(serviceCollection);
        configureOptions(featureSetupOptions);

        Type[] featureTypes = featureAssemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.GetInterfaces().Any(type => type.IsFeature()))
            .Where(type => type.IsDecorator() == false)
            .ToArray();

        foreach (Type featureType in featureTypes)
        {
            serviceCollection.AddFeature(featureType, featureSetupOptions);
        }

        return serviceCollection;
    }

    private static void AddFeature(this IServiceCollection serviceCollection, Type featureType, FeatureSetupOptions featureSetupOptions)
    {
        Type featureInterface = featureType.GetInterfaces().Single(type => type.IsFeature());

        Type[] pipeline = new[] { featureType }
            .SelectMany(featureType => featureType.GetDecorators())
            .Concat(featureInterface.GetMandatoryDecorators(featureType, featureSetupOptions))
            .Concat([featureType])
            .Distinct()
            .Reverse()
            .ToArray();

        serviceCollection.AddTransient(featureInterface, featureInterface.BuildPipeline(pipeline));
    }

    private static IEnumerable<Type> GetDecorators(this Type _)
    {
        yield break;
    }

    private static IEnumerable<Type> GetMandatoryDecorators(this Type featureInterface, Type featureType, FeatureSetupOptions featureSetupOptions)
    {
        IEnumerable<T> GetInnerDecorators<T>(Func<IPipelineExtension, T> selector)
        {
            foreach (IPipelineExtension pipelineExtension in featureSetupOptions.PipelineExtensions)
            {
                if (pipelineExtension.ShouldApplyPipelineExtension(featureType))
                {
                    yield return selector(pipelineExtension);
                }
            }
        }

        if (featureInterface.IsCommandFeature())
        {
            return GetInnerDecorators(pipelineExtensions => pipelineExtensions.CommandTypeDecorator);
        }

        if (featureInterface.IsQueryFeature())
        {
            return GetInnerDecorators(pipelineExtensions => pipelineExtensions.QueryTypeDecorator);
        }

        return [];
    }

    private static Func<IServiceProvider, object> BuildPipeline(this Type featureInterface, params Type[] pipeline)
    {
        IEnumerable<ConstructorInfo> constructors = pipeline
            .SelectMany(type => 
            {
                Type t = type.IsGenericType && featureInterface.GenericTypeArguments.Length != 0 ? type.MakeGenericType(featureInterface.GenericTypeArguments) : type;
                return t.GetConstructors();
            });

        object Func(IServiceProvider serviceProvider)
        {
            object? current = null;

            foreach (ConstructorInfo constructor in constructors)
            {
                ParameterInfo[] parameterInfos = constructor.GetParameters().ToArray();
                current =  constructor.Invoke(parameterInfos.GetParameters(current, serviceProvider));
            }

            return current!;
        }

        return Func;
    }

    private static object[] GetParameters(this ParameterInfo[] parameterInfos, object? current, IServiceProvider serviceProvider)
    {
        object[] parameters = new object[parameterInfos.Length];

        for (int i = 0; i < parameterInfos.Length; i++)
        {
            parameters[i] = parameterInfos[i].GetParameter(current, serviceProvider);
        }

        return parameters;
    }

    private static object GetParameter(this ParameterInfo parameterInfo, object? current, IServiceProvider serviceProvider)
    {
        Type parameterType = parameterInfo.ParameterType;

        if (parameterType.IsFeature())
        {
            if (current == null)
            {
                current = serviceProvider.GetRequiredService(parameterType);
            }

            return current;
        }

        return serviceProvider.GetRequiredService(parameterType);
    }

    #endregion
}