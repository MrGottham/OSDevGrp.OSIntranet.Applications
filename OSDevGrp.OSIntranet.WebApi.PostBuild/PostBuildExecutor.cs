using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace OSDevGrp.OSIntranet.WebApi.PostBuild;

public static class PostBuildExecutor
{
    #region Methods

    public static void Execute(IReadOnlyCollection<string> arguments, Func<bool> runningInDockerCalculator, IServiceProvider serviceProvider)
    {
        ExecuteAsync(arguments, runningInDockerCalculator, serviceProvider)
            .GetAwaiter()
            .GetResult();
    }

    public static Task ExecuteAsync(IReadOnlyCollection<string> arguments, Func<bool> runningInDockerCalculator, IServiceProvider serviceProvider)
    {
        using IServiceScope serviceScope = serviceProvider.CreateScope();

        ILogger logger = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(PostBuildExecutor));

        return ExecuteAsync(PostBuildExecutorContext.Create(arguments, runningInDockerCalculator, logger));
    }

    private static async Task ExecuteAsync(PostBuildExecutorContext context)
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

    private class PostBuildExecutorContext
    {
        #region Private variables

        private readonly ILogger _logger;

        #endregion

        #region Constructor

        private PostBuildExecutorContext(IReadOnlyCollection<string> arguments, Func<bool> runningInDockerCalculator, ILogger logger)
        {
            IsPostBuild = arguments.Count > 0 && arguments.ElementAt(0) == "postbuild";
            RunningInDocker = runningInDockerCalculator();

            _logger = logger;
        }

        #endregion

        #region Properties

        internal bool IsPostBuild { get; }

        internal bool RunningInDocker { get;}

        #endregion

        #region Methods

        internal async Task NotifyAsync(string message)
        {
            _logger.LogInformation(message);

            if (IsPostBuild)
            {
                await Console.Out.WriteLineAsync(message);
            }
        }

        internal async Task NotifyAsync(Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            if (IsPostBuild)
            {
                await Console.Error.WriteLineAsync(exception.Message);
            }
        }

        internal static PostBuildExecutorContext Create(IReadOnlyCollection<string> arguments, Func<bool> runningInDockerCalculator, ILogger logger)
        {
            return new PostBuildExecutorContext(arguments, runningInDockerCalculator, logger);
        }

        #endregion
    }
}