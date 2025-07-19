using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.CookieConsent;

internal class CookieConsentFeature : PageFeatureBase<CookieConsentRequest, CookieConsentResponse, object>
{
    #region Private variables

    private readonly TimeProvider _timeProvider;

    #endregion

    #region Constructor

    public CookieConsentFeature(TimeProvider timeProvider, IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
        _timeProvider = timeProvider;
    }

    #endregion

    #region Methods

    public override async Task<CookieConsentResponse> ExecuteAsync(CookieConsentRequest request, CancellationToken cancellationToken = default)
    {
        DateTimeOffset expires = _timeProvider.GetUtcNow().AddDays(90);

        IReadOnlyDictionary<StaticTextKey, string> staticTexts = await GetStaticTextsAsync(request, new object(), cancellationToken);

        return new CookieConsentResponse($"{request.ApplicationName}.CookieConsent", Convert.ToString(true).ToLower(), expires, _timeProvider, staticTexts);
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(CookieConsentRequest request, object argument)
    {
        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.WebsiteUsingCookies, StaticTextKey.WebsiteUsingCookies.DefaultArguments() },
            { StaticTextKey.CookieConsentInformation, StaticTextKey.CookieConsentInformation.DefaultArguments() },
            { StaticTextKey.AllowNecessaryCookies, StaticTextKey.AllowNecessaryCookies.DefaultArguments() }
        };
    }

    #endregion
}