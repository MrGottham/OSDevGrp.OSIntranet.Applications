using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.CookieConsent;

public class CookieConsentResponse : PageResponseBase
{
    #region Private variables

    private readonly TimeProvider _timeProvider;

    #endregion

    #region Constructor

    public CookieConsentResponse(string cookieName, string cookieValue, DateTimeOffset expires, TimeProvider timeProvider, IReadOnlyDictionary<StaticTextKey, string> staticTexts)
        : base(staticTexts)
    {
        CookieName = cookieName;
        CookieValue = cookieValue;
        Expires = expires;

        _timeProvider = timeProvider;
    }

    #endregion

    #region Properties

    public string CookieName { get; }

    public string CookieValue { get; }

    public DateTimeOffset Expires { get; }

    public int DaysUntilExpiry => Math.Max(0, (int) Expires.UtcDateTime.Date.Subtract(_timeProvider.GetUtcNow().UtcDateTime.Date).TotalDays);

    #endregion
}