using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;

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

    internal static string GetConsentCookieName() => $"{GetNamespace()}.Consent";

    internal static string GetApplicationCookieName() => $"{GetNamespace()}.Application";

    internal static string GetAntiforgeryCookieName() => $"{GetNamespace()}.Antiforgery";

    internal static string GetInternalAuthenticationCookieName() => $"{GetNamespace()}.Authentication.{Schemes.Internal}";

    internal static string GetOpenIdConnectAuthenticationCookieName() => $"{ProgramHelper.GetNamespace()}.Authentication.{OpenIdConnectDefaults.AuthenticationScheme}";

    internal static string GetTitle() => "OS Development Group Backend For Frontend API";

    internal static string GetDescription() => "Web API supporting frontend applications for OS Development Group.";

    internal static string GetOpenApiDocumentName() => "swagger";

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

    internal static string GetReturnUrlParameter() => "returnUrl";

    internal static string GetLoginPath() => "/api/security/login";

    internal static string GetLogoutPath() => "/api/security/logout";

    internal static string GetAccessDeniedPath() => "/api/security/accessdenied";

    #endregion
}