namespace OSDevGrp.OSIntranet.Bff.WebApi;

internal static class ProgramHelper
{
    #region Constants

    private const string DotnetRunningInContainerEnvironmentVariable = "DOTNET_RUNNING_IN_CONTAINER";

    #endregion

    #region Methods

    internal static string GetNamespace()
    {
        string? result = typeof(ProgramHelper).Namespace;
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException("The namespace of the program could not be determined.");
        }
        return result;
    }

    internal static string GetTitle()
    {
        return "OS Development Group Backend For Frontend API";
    }

    internal static string GetDescription()
    {
        return "Web API supporting frontend applications for OS Development Group.";
    }

    internal static string GetOpenApiDocumentName()
    {
        return "swagger";
    }

    internal static Uri? GetOpenApiDocumentUrl(IHostEnvironment environment)
    {
        if (environment.IsDevelopment() == false)
        {
            return null;
        }

        return new Uri($"/openapi/{GetOpenApiDocumentName()}.json", UriKind.Relative);
    }

    internal static bool RunningInDocker()
    {
        return RunningInDocker(Environment.GetEnvironmentVariable(DotnetRunningInContainerEnvironmentVariable));
    }

    private static bool RunningInDocker(string? environmentVariable)
    {
        if (string.IsNullOrWhiteSpace(environmentVariable) || bool.TryParse(environmentVariable, out bool result) == false)
        {
            return false;
        }
        return result;
    }

    #endregion
}