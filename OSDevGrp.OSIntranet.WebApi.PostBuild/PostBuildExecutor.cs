using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace OSDevGrp.OSIntranet.WebApi.PostBuild;

public static class PostBuildExecutor
{
    #region Methods

    public static void Execute(IReadOnlyCollection<string> arguments, Func<bool> runningInDockerCalculator, string webApiVersion, string generatedCodeNamespace, string generatedCodeClassName, IServiceProvider serviceProvider)
    {
        ExecuteAsync(arguments, runningInDockerCalculator, webApiVersion, generatedCodeNamespace, generatedCodeClassName, serviceProvider)
            .GetAwaiter()
            .GetResult();
    }

    public static Task ExecuteAsync(IReadOnlyCollection<string> arguments, Func<bool> runningInDockerCalculator, string webApiVersion, string generatedCodeNamespace, string generatedCodeClassName, IServiceProvider serviceProvider)
    {
        using IServiceScope serviceScope = serviceProvider.CreateScope();

        ILogger logger = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(PostBuildExecutor));
        ISwaggerProvider swaggerProvider = serviceScope.ServiceProvider.GetRequiredService<ISwaggerProvider>();
        ClientApiCodeGenerator clientApiCodeGenerator = new ClientApiCodeGenerator();

        return ExecuteAsync(PostBuildExecutorContext.Create(arguments, runningInDockerCalculator, swaggerProvider, webApiVersion, generatedCodeNamespace, generatedCodeClassName, logger, CancellationToken.None), clientApiCodeGenerator);
    }

    private static async Task ExecuteAsync(PostBuildExecutorContext context, ClientApiCodeGenerator clientApiCodeGenerator)
    {
        bool shouldContinueExecution = await Task.Run(async () => 
        {
            await context.NotifyAsync($"Starting {nameof(PostBuildExecutor)}");
            try
            {
                if (context.IsPostBuild == false)
                {
                    await context.NotifyAsync($"{nameof(PostBuildExecutor)} has been disabled while postbuild was not given as argument");
                    return true;
                }

                if (context.RunningInDocker)
                {
                    await context.NotifyAsync($"{nameof(PostBuildExecutor)} has been disabled while running in docker");
                    return false;
                }

                await clientApiCodeGenerator.GenerateAsync(context);

                return false;
            }
            catch (Exception ex)
            {
                await context.NotifyAsync(ex);

                throw;
            }
            finally
            {
                await context.NotifyAsync($"Finishing {nameof(PostBuildExecutor)}");
            }
        });

        if (shouldContinueExecution == false)
        {
            Environment.Exit(0);
        }
    }

    #endregion
}