using Microsoft.AspNetCore.Authentication;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.WebApi.Handlers
{
    internal static class AuthenticationBuilderExtensions
    {
        internal static AuthenticationBuilder AddClientSecret(this AuthenticationBuilder authenticationBuilder, string authenticationScheme)
        {
            NullGuard.NotNull(authenticationBuilder, nameof(authenticationBuilder))
                .NotNullOrWhiteSpace(authenticationScheme, nameof(authenticationScheme));

            return authenticationBuilder.AddScheme<ClientSecretAuthenticationOptions, ClientSecretAuthenticationHandler>(authenticationScheme, null);
        }
    }
}