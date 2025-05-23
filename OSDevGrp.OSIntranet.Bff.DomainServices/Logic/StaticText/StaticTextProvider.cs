using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using System.Collections.Concurrent;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

internal class StaticTextProvider : IStaticTextProvider
{
    #region Private variables

    private readonly IReadOnlyDictionary<StaticTextKey, string> _staticTexts = GenerateStaticTexts();

    #endregion

    #region Methods

    public Task<string> GetStaticTextAsync(StaticTextKey staticTextKey, IEnumerable<object> arguments, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(formatProvider);

        cancellationToken.ThrowIfCancellationRequested();

        if (_staticTexts.TryGetValue(staticTextKey, out string? staticText) == false)
        {
            throw new ArgumentOutOfRangeException(nameof(staticTextKey), staticTextKey, $"The static text key {staticTextKey} is not supported.");
        }

        return Task.FromResult(string.Format(formatProvider, staticText, arguments.ToArray()));
    }

    private static IReadOnlyDictionary<StaticTextKey, string> GenerateStaticTexts()
    {
        IDictionary<StaticTextKey, string> staticTexts = new ConcurrentDictionary<StaticTextKey, string>();
        staticTexts.Add(StaticTextKey.MrGotthamsHomepage, "Mr. Gottham's Homepage");
        staticTexts.Add(StaticTextKey.OSDevelopmentGroup, "OS Development Group");
        staticTexts.Add(StaticTextKey.Copyright, "OS Development Group © {0}");
        staticTexts.Add(StaticTextKey.BuildInfo, "Build {0:yyyyMMddHHmm}");
        staticTexts.Add(StaticTextKey.Start, "Start");
        staticTexts.Add(StaticTextKey.Login, "Log ind");
        staticTexts.Add(StaticTextKey.Logout, "Log ud");
        staticTexts.Add(StaticTextKey.AccessDenied, "Adgang nægtet");
        staticTexts.Add(StaticTextKey.MissingPermissionToPage, "Du har ikke de nødvendige rettigheder til at tilgå denne side.");
        staticTexts.Add(StaticTextKey.CheckYourCredentials, "Tjek venligst op på dine rettigheder og prøv igen.");
        staticTexts.Add(StaticTextKey.SomethingWentWrong, "Noget gik galt");
        staticTexts.Add(StaticTextKey.WebsiteUsingCookies, "Denne hjemmeside bruger cookies");
        staticTexts.Add(StaticTextKey.CookieConsentInformation, "Vi bruger cookies til at tilpasse dit indhold, dine funktioner samt optimere din brugeroplevelse her på hjemmeside. Vi deler ikke oplysninger om din brug af vores hjemmesede med partnere, sociale medier, annonceringspartnere ej heller analysepartnere.");
        staticTexts.Add(StaticTextKey.AllowNecessaryCookies, "Tillad nødvendige");
        staticTexts.Add(StaticTextKey.FinancialManagement, "Finansstyring");
        staticTexts.Add(StaticTextKey.Accountings, "Regnskaber");
        return staticTexts.AsReadOnly();
    }

    #endregion
}