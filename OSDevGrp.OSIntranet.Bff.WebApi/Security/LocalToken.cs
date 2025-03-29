using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

internal class LocalToken : IToken
{
    #region Private variables

    private readonly TimeProvider _timeProvider;

    #endregion

    #region Constructor

    public LocalToken(string tokenType, string token, DateTimeOffset expires, TimeProvider timeProvider)
    {
        TokenType = tokenType;
        Token = token;
        Expires = expires.ToUniversalTime();

        _timeProvider = timeProvider;
    }

    #endregion

    #region Properties

    public string TokenType { get; }

    public string Token { get; }

    public DateTimeOffset Expires { get; }

    public bool Expired => Expires <= _timeProvider.GetUtcNow();

    #endregion
}