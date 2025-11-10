using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.AccountingTextsBuilder;

[TestFixture]
public class BuildAsyncTests
{
    #region Private variables

    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.StatusDate, 1)]
    [TestCase(StaticTextKey.BalanceBelowZero, 1)]
    [TestCase(StaticTextKey.Debtors, 4)]
    [TestCase(StaticTextKey.Creditors, 4)]
    [TestCase(StaticTextKey.BackDating, 1)]
    [TestCase(StaticTextKey.Days, 1)]
    [TestCase(StaticTextKey.Day, 1)]
    [TestCase(StaticTextKey.BalanceSheetAtStatusDate, 1)]
    [TestCase(StaticTextKey.BalanceSheetAtEndOfLastMonthFromStatusDate, 1)]
    [TestCase(StaticTextKey.BalanceSheetAtEndOfLastMonthFromStatusDate, 1)]
    [TestCase(StaticTextKey.Assets, 3)]
    [TestCase(StaticTextKey.Liabilities, 3)]
    [TestCase(StaticTextKey.BudgetStatementForMonthOfStatusDate, 1)]
    [TestCase(StaticTextKey.BudgetStatementForLastMonthOfStatusDate, 1)]
    [TestCase(StaticTextKey.BudgetStatementForYearToDateOfStatusDate, 1)]
    [TestCase(StaticTextKey.BudgetStatementForLastYearOfStatusDate, 1)]
    [TestCase(StaticTextKey.Budget, 4)]
    [TestCase(StaticTextKey.Result, 4)]
    [TestCase(StaticTextKey.ObligeePartiesAtStatusDate, 1)]
    [TestCase(StaticTextKey.ObligeePartiesAtEndOfLastMonthFromStatusDate, 1)]
    [TestCase(StaticTextKey.ObligeePartiesAtEndOfLastYearFromStatusDate, 1)]
    public async Task BuildAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithExpectedStaticTextKeys(StaticTextKey staticTextKey, int expectedCalls)
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(accountingModel, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Exactly(expectedCalls));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTexts()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<AccountingTexts>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelOnStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.StatusDate.Label, Does.StartWith($"{StaticTextKey.StatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelOnBalanceBelowZeroIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceBelowZero.Label, Does.StartWith($"{StaticTextKey.BalanceBelowZero}:"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(BalanceBelowZeroType.Debtors, StaticTextKey.Debtors)]
    [TestCase(BalanceBelowZeroType.Creditors, StaticTextKey.Creditors)]

    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereValueOnBalanceBelowZeroIsEqualToStaticTextFromStaticTextProvider(BalanceBelowZeroType balanceBelowZeroType, StaticTextKey staticTextKey)
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!, balanceBelowZeroType: balanceBelowZeroType);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceBelowZero.Value, Does.StartWith($"{staticTextKey}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelOnBackDatingIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BackDating.Label, Does.StartWith($"{StaticTextKey.BackDating}:"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(0, StaticTextKey.Days)]
    [TestCase(1, StaticTextKey.Day)]
    [TestCase(2, StaticTextKey.Days)]
    [TestCase(30, StaticTextKey.Days)]

    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereValueOnBackDatingIsEqualToBackDatingValuePostedFixedWithStaticTextFromStaticTextProvider(int backDating, StaticTextKey staticTextKey)
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!, backDating: backDating);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BackDating.Value, Does.StartWith($"{backDating} {staticTextKey.ToString().ToLower()}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBalanceSheetAtStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtStatusDate.Header, Does.StartWith($"{StaticTextKey.BalanceSheetAtStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAssetsOnBalanceSheetAtStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtStatusDate.Assets.Label, Does.StartWith($"{StaticTextKey.Assets}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForLiabilitiesOnBalanceSheetAtStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtStatusDate.Liabilities.Label, Does.StartWith($"{StaticTextKey.Liabilities}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBalanceSheetAtEndOfLastMonthFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtEndOfLastMonthFromStatusDate.Header, Does.StartWith($"{StaticTextKey.BalanceSheetAtEndOfLastMonthFromStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAssetsOnBalanceSheetAtEndOfLastMonthFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtEndOfLastMonthFromStatusDate.Assets.Label, Does.StartWith($"{StaticTextKey.Assets}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForLiabilitiesOnBalanceSheetAtEndOfLastMonthFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtEndOfLastMonthFromStatusDate.Liabilities.Label, Does.StartWith($"{StaticTextKey.Liabilities}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBalanceSheetAtEndOfLastYearFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtEndOfLastYearFromStatusDate.Header, Does.StartWith($"{StaticTextKey.BalanceSheetAtEndOfLastYearFromStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAssetsOnBalanceSheetAtEndOfLastYearFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtEndOfLastYearFromStatusDate.Assets.Label, Does.StartWith($"{StaticTextKey.Assets}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForLiabilitiesOnBalanceSheetAtEndOfLastYearFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtEndOfLastYearFromStatusDate.Liabilities.Label, Does.StartWith($"{StaticTextKey.Liabilities}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBudgetStatementForMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForMonthOfStatusDate.Header, Does.StartWith($"{StaticTextKey.BudgetStatementForMonthOfStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForBudgetOnBudgetStatementForMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForMonthOfStatusDate.Budget.Label, Does.StartWith($"{StaticTextKey.Budget}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForPostedOnBudgetStatementForMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForMonthOfStatusDate.Posted.Label, Does.StartWith($"{StaticTextKey.Result}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAvailableOnBudgetStatementForMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForMonthOfStatusDate.Available.Label, Does.StartWith($"{StaticTextKey.Available}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBudgetStatementForLastMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastMonthOfStatusDate.Header, Does.StartWith($"{StaticTextKey.BudgetStatementForLastMonthOfStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForBudgetOnBudgetStatementForLastMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastMonthOfStatusDate.Budget.Label, Does.StartWith($"{StaticTextKey.Budget}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForPostedOnBudgetStatementForLastMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastMonthOfStatusDate.Posted.Label, Does.StartWith($"{StaticTextKey.Result}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAvailableOnBudgetStatementForLastMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastMonthOfStatusDate.Available.Label, Does.StartWith($"{StaticTextKey.Available}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBudgetStatementForYearToDateOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForYearToDateOfStatusDate.Header, Does.StartWith($"{StaticTextKey.BudgetStatementForYearToDateOfStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForBudgetOnBudgetStatementForYearToDateOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForYearToDateOfStatusDate.Budget.Label, Does.StartWith($"{StaticTextKey.Budget}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForPostedOnBudgetStatementForYearToDateOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForYearToDateOfStatusDate.Posted.Label, Does.StartWith($"{StaticTextKey.Result}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAvailableOnBudgetStatementForYearToDateOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForYearToDateOfStatusDate.Available.Label, Does.StartWith($"{StaticTextKey.Available}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBudgetStatementForLastYearOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastYearOfStatusDate.Header, Does.StartWith($"{StaticTextKey.BudgetStatementForLastYearOfStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForBudgetOnBudgetStatementForLastYearOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastYearOfStatusDate.Budget.Label, Does.StartWith($"{StaticTextKey.Budget}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForPostedOnBudgetStatementForLastYearOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastYearOfStatusDate.Posted.Label, Does.StartWith($"{StaticTextKey.Result}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAvailableOnBudgetStatementForLastYearOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastYearOfStatusDate.Available.Label, Does.StartWith($"{StaticTextKey.Available}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnObligeePartiesAtStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtStatusDate.Header, Does.StartWith($"{StaticTextKey.ObligeePartiesAtStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForDebtorsOnObligeePartiesAtStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtStatusDate.Debtors.Label, Does.StartWith($"{StaticTextKey.Debtors}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForCreditorsOnObligeePartiesAtStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtStatusDate.Creditors.Label, Does.StartWith($"{StaticTextKey.Creditors}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnObligeePartiesAtEndOfLastMonthFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtEndOfLastMonthFromStatusDate.Header, Does.StartWith($"{StaticTextKey.ObligeePartiesAtEndOfLastMonthFromStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForDebtorsOnObligeePartiesAtEndOfLastMonthFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtEndOfLastMonthFromStatusDate.Debtors.Label, Does.StartWith($"{StaticTextKey.Debtors}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForCreditorsOnObligeePartiesAtEndOfLastMonthFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtEndOfLastMonthFromStatusDate.Creditors.Label, Does.StartWith($"{StaticTextKey.Creditors}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnObligeePartiesAtEndOfLastYearFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtEndOfLastYearFromStatusDate.Header, Does.StartWith($"{StaticTextKey.ObligeePartiesAtEndOfLastYearFromStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForDebtorsOnObligeePartiesAtEndOfLastYearFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtEndOfLastYearFromStatusDate.Debtors.Label, Does.StartWith($"{StaticTextKey.Debtors}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForCreditorsOnObligeePartiesAtEndOfLastYearFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IAccountingTexts result = await sut.BuildAsync(accountingModel, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtEndOfLastYearFromStatusDate.Creditors.Label, Does.StartWith($"{StaticTextKey.Creditors}:"));
    }

    private IAccountingTextsBuilder CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.DynamicText.AccountingTextsBuilder(_staticTextProviderMock!.Object);
    }
}