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
        staticTexts.Add(StaticTextKey.FunctionalityNotImplmented, "Funktionaliteten er endnu ikke implementeret");
        staticTexts.Add(StaticTextKey.FunctionalityNotImplmentedDetails, "Denne funktionalitet er på nuværende tidspunkt endnu ikke implementeret i systemet. Det betyder, at det pågældende element, knap eller funktion ikke er aktiv og derfor ikke udfører nogen handling ved brug. Funktionen er planlagt til fremtidig udvikling og vil blive tilgængelig i en kommende opdatering. Indtil da kan du opleve, at visse funktioner er deaktiverede eller markeret som utilgængelige. Vi arbejder løbende på at udvide og forbedre systemets muligheder.");
        staticTexts.Add(StaticTextKey.MailAddress, "Mailadresse");
        staticTexts.Add(StaticTextKey.Permissions, "Rettigheder");
        staticTexts.Add(StaticTextKey.Administrator, "Administrator");
        staticTexts.Add(StaticTextKey.Creator, "Skaber");
        staticTexts.Add(StaticTextKey.Modifier, "Redaktør");
        staticTexts.Add(StaticTextKey.Viewer, "Læser");
        staticTexts.Add(StaticTextKey.FinancialManagement, "Finansstyring");
        staticTexts.Add(StaticTextKey.PrimaryAccounting, "Primær regnskab");
        staticTexts.Add(StaticTextKey.Accountings, "Regnskaber");
        staticTexts.Add(StaticTextKey.CreateNewAccounting, "Tilføj regnskab");
        staticTexts.Add(StaticTextKey.UpdateAccounting, "Redigér regnskab");
        staticTexts.Add(StaticTextKey.DeleteAccounting, "Slet regnskab");
        staticTexts.Add(StaticTextKey.MasterData, "Stamdata");
        staticTexts.Add(StaticTextKey.AccountingNumber, "Regnskabsnummer");
        staticTexts.Add(StaticTextKey.AccountingName, "Regnskabsnavn");
        staticTexts.Add(StaticTextKey.BalanceBelowZero, "Saldo under {0:C}");
        staticTexts.Add(StaticTextKey.Debtors, "Debitorer");
        staticTexts.Add(StaticTextKey.Creditors, "Kreditorer");
        staticTexts.Add(StaticTextKey.BackDating, "Antal dage for tilbagedatering");
        staticTexts.Add(StaticTextKey.Days, "Dage");
        staticTexts.Add(StaticTextKey.Day, "Dag");
        staticTexts.Add(StaticTextKey.CommonData, "Fælles data");
        staticTexts.Add(StaticTextKey.LetterHead, "Brevhoved");
        staticTexts.Add(StaticTextKey.LetterHeadNumber, "Nummer på brevhoved");
        staticTexts.Add(StaticTextKey.RequiredValueValidationError, "{0} skal angives.");
        staticTexts.Add(StaticTextKey.MinLengthValidationError, "{0} skal minimum have en længde på {1} tegn.");
        staticTexts.Add(StaticTextKey.MaxLengthValidationError, "{0} må maksimum have en længde på {1} tegn.");
        staticTexts.Add(StaticTextKey.ShouldBeIntegerValidationError, "{0} skal være et heltal.");
        staticTexts.Add(StaticTextKey.MinValueValidationError, "{0} skal være større end eller lig med {1}.");
        staticTexts.Add(StaticTextKey.MaxValueValidationError, "{0} skal være mindre end eller lig med {1}.");
        staticTexts.Add(StaticTextKey.PatternValidationError, "{0} skal matche mønstret: {1}");
        staticTexts.Add(StaticTextKey.OneOfValidationError, "{0} skal være en af følgende værdier: {1}");
        return staticTexts.AsReadOnly();
    }

    #endregion
}