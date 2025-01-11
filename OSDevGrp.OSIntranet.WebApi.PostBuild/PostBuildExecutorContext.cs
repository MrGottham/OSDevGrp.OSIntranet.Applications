using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace OSDevGrp.OSIntranet.WebApi.PostBuild;

internal class PostBuildExecutorContext
{
    #region Private variables

    private readonly ILogger _logger;

    #endregion

    #region Constructor

    private PostBuildExecutorContext(IReadOnlyCollection<string> arguments, Func<bool> runningInDockerCalculator, ISwaggerProvider swaggerProvider, string webApiVersion, ILogger logger)
    {
        _logger = logger;

        IsPostBuild = arguments.Count > 0 && arguments.ElementAt(0) == "postbuild";
        RunningInDocker = runningInDockerCalculator();

        OpenApiDocument openApiDocument = swaggerProvider.GetSwagger(webApiVersion);
        if (openApiDocument == null)
        {
            throw new ArgumentException($"Could not reslove the Open Api Document for: {webApiVersion}");
        }
        OpenApiDocument = openApiDocument;
    }

    #endregion

    #region Properties

    internal bool IsPostBuild { get; }

    internal bool RunningInDocker { get;}

    internal OpenApiDocument OpenApiDocument { get;}

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

    internal static PostBuildExecutorContext Create(IReadOnlyCollection<string> arguments, Func<bool> runningInDockerCalculator, ISwaggerProvider swaggerProvider, string webApiVersion, ILogger logger)
    {
        return new PostBuildExecutorContext(arguments, runningInDockerCalculator, swaggerProvider, webApiVersion, logger);
    }

    #endregion
}