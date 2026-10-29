using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

internal static class AccessTokenExpirationResolver
{
    #region Methods

    internal static DateTimeOffset Resolve(string? expiresIn, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        if (int.TryParse(expiresIn, NumberStyles.None, CultureInfo.InvariantCulture, out int expiresInSeconds) == false || expiresInSeconds <= 0)
        {
            throw new InvalidOperationException("The token endpoint response did not contain a valid expires_in value.");
        }

        return timeProvider.GetUtcNow().AddSeconds(expiresInSeconds);
    }

    #endregion
}