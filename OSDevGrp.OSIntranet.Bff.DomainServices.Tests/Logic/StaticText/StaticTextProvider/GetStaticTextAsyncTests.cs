using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;

[TestFixture]
public class GetStaticTextAsyncTests
{
    #region Private variables

    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.MrGotthamsHomepage, "Mr. Gottham's Homepage", 0)]
    [TestCase(StaticTextKey.OSDevelopmentGroup, "OS Development Group", 0)]
    [TestCase(StaticTextKey.Copyright, "OS Development Group © {0}", 1)]
    [TestCase(StaticTextKey.BuildInfo, "Build {0}", 1)]
    [TestCase(StaticTextKey.Start, "Start", 0)]
    [TestCase(StaticTextKey.Login, "Log ind", 0)]
    [TestCase(StaticTextKey.Logout, "Log ud", 0)]
    [TestCase(StaticTextKey.AccessDenied, "Adgang nægtet", 0)]
    [TestCase(StaticTextKey.MissingPermissionToPage, "Du har ikke de nødvendige rettigheder til at tilgå denne side.", 0)]
    [TestCase(StaticTextKey.CheckYourCredentials, "Tjek venligst op på dine rettigheder og prøv igen.", 0)]
    [TestCase(StaticTextKey.SomethingWentWrong, "Noget gik galt", 0)]
    [TestCase(StaticTextKey.WebsiteUsingCookies, "Denne hjemmeside bruger cookies", 0)]
    [TestCase(StaticTextKey.CookieConsentInformation, "Vi bruger cookies til at tilpasse dit indhold, dine funktioner samt optimere din brugeroplevelse her på hjemmeside. Vi deler ikke oplysninger om din brug af vores hjemmesede med partnere, sociale medier, annonceringspartnere ej heller analysepartnere.", 0)]
    [TestCase(StaticTextKey.AllowNecessaryCookies, "Tillad nødvendige", 0)]
    [TestCase(StaticTextKey.FunctionalityNotImplmented, "Funktionaliteten er endnu ikke implementeret", 0)]
    [TestCase(StaticTextKey.FunctionalityNotImplmentedDetails, "Denne funktionalitet er på nuværende tidspunkt endnu ikke implementeret i systemet. Det betyder, at det pågældende element, knap eller funktion ikke er aktiv og derfor ikke udfører nogen handling ved brug. Funktionen er planlagt til fremtidig udvikling og vil blive tilgængelig i en kommende opdatering. Indtil da kan du opleve, at visse funktioner er deaktiverede eller markeret som utilgængelige. Vi arbejder løbende på at udvide og forbedre systemets muligheder.", 0)]
    [TestCase(StaticTextKey.MailAddress, "Mailadresse", 0)]
    [TestCase(StaticTextKey.Permissions, "Rettigheder", 0)]
    [TestCase(StaticTextKey.Administrator, "Administrator", 0)]
    [TestCase(StaticTextKey.Creator, "Skaber", 0)]
    [TestCase(StaticTextKey.Modifier, "Redaktør", 0)]
    [TestCase(StaticTextKey.Viewer, "Læser", 0)]
    [TestCase(StaticTextKey.FinancialManagement, "Finansstyring", 0)]
    [TestCase(StaticTextKey.PrimaryAccounting, "Primær regnskab", 0)]
    [TestCase(StaticTextKey.Accountings, "Regnskaber", 0)]
    [TestCase(StaticTextKey.CreateNewAccounting, "Tilføj regnskab", 0)]
    [TestCase(StaticTextKey.UpdateAccounting, "Redigér regnskab", 0)]
    [TestCase(StaticTextKey.DeleteAccounting, "Slet regnskab", 0)]
    [TestCase(StaticTextKey.MasterData, "Stamdata", 0)]
    [TestCase(StaticTextKey.AccountingNumber, "Regnskabsnummer", 0)]
    [TestCase(StaticTextKey.AccountingName, "Regnskabsnavn", 0)]
    [TestCase(StaticTextKey.BalanceBelowZero, "Saldo under {0}", 1)]
    [TestCase(StaticTextKey.Debtors, "Debitorer", 0)]
    [TestCase(StaticTextKey.Creditors, "Kreditorer", 0)]
    [TestCase(StaticTextKey.BackDating, "Antal dage for tilbagedatering", 0)]
    [TestCase(StaticTextKey.Days, "Dage", 0)]
    [TestCase(StaticTextKey.Day, "Dag", 0)]
    [TestCase(StaticTextKey.CommonData, "Fælles data", 0)]
    [TestCase(StaticTextKey.LetterHead, "Brevhoved", 0)]
    [TestCase(StaticTextKey.LetterHeadNumber, "Nummer på brevhoved", 0)]
    [TestCase(StaticTextKey.RequiredValueValidationError, "{0} skal angives.", 1)]
    [TestCase(StaticTextKey.MinLengthValidationError, "{0} skal minimum have en længde på {1} tegn.", 2)]
    [TestCase(StaticTextKey.MaxLengthValidationError, "{0} må maksimum have en længde på {1} tegn.", 2)]
    [TestCase(StaticTextKey.ShouldBeIntegerValidationError, "{0} skal være et heltal.", 1)]
    [TestCase(StaticTextKey.MinValueValidationError, "{0} skal være større end eller lig med {1}.", 2)]
    [TestCase(StaticTextKey.MaxValueValidationError, "{0} skal være mindre end eller lig med {1}.", 2)]
    [TestCase(StaticTextKey.PatternValidationError, "{0} skal matche mønstret: {1}", 2)]
    [TestCase(StaticTextKey.OneOfValidationError, "{0} skal være en af følgende værdier: {1}", 2)]
    [TestCase(StaticTextKey.Create, "Opret", 0)]
    [TestCase(StaticTextKey.Update, "Opdatér", 0)]
    [TestCase(StaticTextKey.Delete, "Slet", 0)]
    [TestCase(StaticTextKey.Reset, "Nulstil", 0)]
    [TestCase(StaticTextKey.Cancel, "Fortryd", 0)]
    public async Task GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticTesxt(StaticTextKey staticTextKey, string expectedStaticText, int numberOfArguments)
    {
        IStaticTextProvider sut = CreateSut();

        IEnumerable<object> arguments = CreateArguments(numberOfArguments);
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        string result = await sut.GetStaticTextAsync(staticTextKey, arguments, formatProvider);

        Assert.That(result, Is.EqualTo(string.Format(formatProvider, expectedStaticText, arguments.ToArray())));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextAsync_WhenCalledForAllStaticTextKeys_ReturnsStaticText()
    {
        IStaticTextProvider sut = CreateSut();

        foreach (StaticTextKey staticTextKey in Enum.GetValues<StaticTextKey>())
        {
            string result = await sut.GetStaticTextAsync(staticTextKey, CreateArguments(), CultureInfo.InvariantCulture);

            Assert.That(string.IsNullOrWhiteSpace(result), Is.False, $"The static text for {staticTextKey} is not defined");
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextAsync_WhenCancellationIsRequestedOnCancellationToken_ThrowsOperationCanceledException()
    {
        IStaticTextProvider sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        cancellationTokenSource.Cancel();

        try
        {
            await sut.GetStaticTextAsync(StaticTextKey.Start, CreateArguments(), CultureInfo.InvariantCulture, cancellationToken);

            Assert.Fail("An OperationCanceledException was expected.");
        }
        catch (OperationCanceledException)
        {
        }
    }

    private static IStaticTextProvider CreateSut()
    {
        return new DomainServices.Logic.StaticText.StaticTextProvider();
    }

    private IEnumerable<object> CreateArguments(int? numberOfArguments = null)
    {
        if (numberOfArguments.HasValue && numberOfArguments.Value == 0)
        {
            return Array.Empty<object>();
        }

        return _fixture!.CreateMany<string>(numberOfArguments ?? _random!.Next(5, 10)).ToArray();
    }
}