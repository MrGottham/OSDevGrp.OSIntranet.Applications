using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

public class TokenKeyProviderOptions
{
    #region Properties

    public Type TokenStorageType { get; set; } = typeof(TokenStorage);

    public string AnonymousUserIdentifier { get; set; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public string Salt { get; set; } =  Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    #endregion
}