using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.WebApi.PostBuild;

internal class PostBuildExecutorContext
{
    #region Private variables

    private readonly IReadOnlyCollection<string> _arguments;

    private readonly ISwaggerProvider _swaggerProvider;

    private readonly string _webApiVersion;

    private readonly ILogger _logger;

    private readonly static Regex SolutionDirectoryRegex = new($@"(--solutionDir){{1}}\s([A-Za-z0-9{Path.VolumeSeparatorChar}\{Path.DirectorySeparatorChar}.]+){{1}}", RegexOptions.Compiled, TimeSpan.FromMilliseconds(32));

    #endregion

    #region Constructor

    private PostBuildExecutorContext(IReadOnlyCollection<string> arguments, Func<bool> runningInDockerCalculator, ISwaggerProvider swaggerProvider, string webApiVersion, string generatedCodeNamespace, string generatedCodeClassName, ILogger logger, CancellationToken cancellationToken)
    {
        _arguments = arguments;
        _swaggerProvider = swaggerProvider;
        _webApiVersion = webApiVersion;
        _logger = logger;

        IsPostBuild = arguments.Count > 0 && arguments.ElementAt(0) == "postbuild";
        RunningInDocker = runningInDockerCalculator();
        GeneratedCodeNamespace = generatedCodeNamespace;
        GeneratedCodeClassName = generatedCodeClassName;
        CancellationToken = cancellationToken;
    }

    #endregion

    #region Properties

    internal bool IsPostBuild { get; }

    internal bool RunningInDocker { get; }

    internal OpenApiDocument OpenApiDocument 
    {
        get
        {
            OpenApiDocument? openApiDocument = _swaggerProvider.GetSwagger(_webApiVersion);
            if (openApiDocument == null)
            {
                throw new Exception($"Could not reslove the Open Api Document for: {_webApiVersion}");
            }
            return openApiDocument;
        }
    }

    internal string GeneratedCodeNamespace { get; }

    internal string GeneratedCodeClassName { get; }

    internal DirectoryInfo SolutionDirectory 
    {
        get
        {
            Match match = SolutionDirectoryRegex.Match(string.Join(' ', _arguments));
            if (match.Success == false)
            {
                throw new Exception($"No arguments mathing: {SolutionDirectoryRegex}");
            }
            return new DirectoryInfo(match.Groups[2].Value);
        }
    }

    internal CancellationToken CancellationToken { get; }

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

    internal static PostBuildExecutorContext Create(IReadOnlyCollection<string> arguments, Func<bool> runningInDockerCalculator, ISwaggerProvider swaggerProvider, string webApiVersion, string generatedCodeNamespace, string generatedCodeClassName, ILogger logger, CancellationToken cancellationToken)
    {
        return new PostBuildExecutorContext(arguments, runningInDockerCalculator, swaggerProvider, webApiVersion, generatedCodeNamespace, generatedCodeClassName, logger, cancellationToken);
    }

    #endregion
}