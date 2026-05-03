using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.PostingLineCollectionTextsBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.AccountingTextsBuilder;

[TestFixture]
public class BuildAsyncTests
{
    #region Private variables

    private Mock<IPostingLineCollectionTextsBuilder>? _postingLineCollectionTextsBuilderMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _postingLineCollectionTextsBuilderMock = new Mock<IPostingLineCollectionTextsBuilder>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnPostingLineCollectionTextsBuilder()
    {
        IAccountingTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel(postingLineModels: postingLineModels);
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(model, formatProvider, cancellationToken);

        _postingLineCollectionTextsBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IReadOnlyCollection<PostingLineModel>>(value => value == postingLineModels),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.StatusDate, 6)]
    [TestCase(StaticTextKey.BalanceBelowZero, 1)]
    [TestCase(StaticTextKey.Debtors, 4)]
    [TestCase(StaticTextKey.Creditors, 4)]
    [TestCase(StaticTextKey.BackDating, 1)]
    [TestCase(StaticTextKey.Days, 1)]
    [TestCase(StaticTextKey.Day, 1)]
    [TestCase(StaticTextKey.BalanceSheet, 1)]
    [TestCase(StaticTextKey.BalanceSheetAtStatusDate, 2)]
    [TestCase(StaticTextKey.BalanceSheetAtEndOfLastMonthFromStatusDate, 2)]
    [TestCase(StaticTextKey.BalanceSheetAtEndOfLastMonthFromStatusDate, 2)]
    [TestCase(StaticTextKey.Credit, 1)]
    [TestCase(StaticTextKey.Balance, 2)]
    [TestCase(StaticTextKey.Assets, 4)]
    [TestCase(StaticTextKey.AssetsTotal, 1)]
    [TestCase(StaticTextKey.Liabilities, 4)]
    [TestCase(StaticTextKey.LiabilitiesTotal, 1)]
    [TestCase(StaticTextKey.BudgetStatementForMonthOfStatusDate, 2)]
    [TestCase(StaticTextKey.BudgetStatementForLastMonthOfStatusDate, 2)]
    [TestCase(StaticTextKey.BudgetStatementForYearToDateOfStatusDate, 2)]
    [TestCase(StaticTextKey.BudgetStatementForLastYearOfStatusDate, 2)]
    [TestCase(StaticTextKey.Budget, 6)]
    [TestCase(StaticTextKey.Posted, 1)]
    [TestCase(StaticTextKey.Result, 5)]
    [TestCase(StaticTextKey.Available, 7)]
    [TestCase(StaticTextKey.ObligeePartiesAtStatusDate, 1)]
    [TestCase(StaticTextKey.ObligeePartiesAtEndOfLastMonthFromStatusDate, 1)]
    [TestCase(StaticTextKey.ObligeePartiesAtEndOfLastYearFromStatusDate, 1)]
    [TestCase(StaticTextKey.IncomeStatement, 1)]
    [TestCase(StaticTextKey.IncomeStatementTotal, 1)]
    [TestCase(StaticTextKey.Accounts, 1)]
    [TestCase(StaticTextKey.BudgetAccounts, 1)]
    [TestCase(StaticTextKey.ContactAccounts, 1)]
    [TestCase(StaticTextKey.AccountNumberShort, 3)]
    [TestCase(StaticTextKey.AccountName, 3)]
    public async Task BuildAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithExpectedStaticTextKeys(StaticTextKey staticTextKey, int expectedCalls)
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(model, formatProvider, cancellationToken);

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

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<AccountingTexts>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelOnStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.StatusDate.Label, Does.StartWith($"{StaticTextKey.StatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelOnBalanceBelowZeroIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

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
        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel(accountingModel: accountingModel);
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceBelowZero.Value, Does.StartWith($"{staticTextKey}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelOnBackDatingIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

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
        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel(accountingModel: accountingModel);
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BackDating.Value, Does.StartWith($"{backDating} {staticTextKey.ToString().ToLower()}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBalanceSheetAtStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtStatusDate.Header, Does.StartWith($"{StaticTextKey.BalanceSheetAtStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAssetsOnBalanceSheetAtStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtStatusDate.Assets.Label, Does.StartWith($"{StaticTextKey.Assets}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForLiabilitiesOnBalanceSheetAtStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtStatusDate.Liabilities.Label, Does.StartWith($"{StaticTextKey.Liabilities}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBalanceSheetAtEndOfLastMonthFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtEndOfLastMonthFromStatusDate.Header, Does.StartWith($"{StaticTextKey.BalanceSheetAtEndOfLastMonthFromStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAssetsOnBalanceSheetAtEndOfLastMonthFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtEndOfLastMonthFromStatusDate.Assets.Label, Does.StartWith($"{StaticTextKey.Assets}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForLiabilitiesOnBalanceSheetAtEndOfLastMonthFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtEndOfLastMonthFromStatusDate.Liabilities.Label, Does.StartWith($"{StaticTextKey.Liabilities}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBalanceSheetAtEndOfLastYearFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtEndOfLastYearFromStatusDate.Header, Does.StartWith($"{StaticTextKey.BalanceSheetAtEndOfLastYearFromStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAssetsOnBalanceSheetAtEndOfLastYearFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtEndOfLastYearFromStatusDate.Assets.Label, Does.StartWith($"{StaticTextKey.Assets}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForLiabilitiesOnBalanceSheetAtEndOfLastYearFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheetAtEndOfLastYearFromStatusDate.Liabilities.Label, Does.StartWith($"{StaticTextKey.Liabilities}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBudgetStatementForMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForMonthOfStatusDate.Header, Does.StartWith($"{StaticTextKey.BudgetStatementForMonthOfStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForBudgetOnBudgetStatementForMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForMonthOfStatusDate.Budget.Label, Does.StartWith($"{StaticTextKey.Budget}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForPostedOnBudgetStatementForMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForMonthOfStatusDate.Posted.Label, Does.StartWith($"{StaticTextKey.Result}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAvailableOnBudgetStatementForMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForMonthOfStatusDate.Available.Label, Does.StartWith($"{StaticTextKey.Available}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBudgetStatementForLastMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastMonthOfStatusDate.Header, Does.StartWith($"{StaticTextKey.BudgetStatementForLastMonthOfStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForBudgetOnBudgetStatementForLastMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastMonthOfStatusDate.Budget.Label, Does.StartWith($"{StaticTextKey.Budget}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForPostedOnBudgetStatementForLastMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastMonthOfStatusDate.Posted.Label, Does.StartWith($"{StaticTextKey.Result}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAvailableOnBudgetStatementForLastMonthOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastMonthOfStatusDate.Available.Label, Does.StartWith($"{StaticTextKey.Available}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBudgetStatementForYearToDateOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForYearToDateOfStatusDate.Header, Does.StartWith($"{StaticTextKey.BudgetStatementForYearToDateOfStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForBudgetOnBudgetStatementForYearToDateOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForYearToDateOfStatusDate.Budget.Label, Does.StartWith($"{StaticTextKey.Budget}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForPostedOnBudgetStatementForYearToDateOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForYearToDateOfStatusDate.Posted.Label, Does.StartWith($"{StaticTextKey.Result}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAvailableOnBudgetStatementForYearToDateOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForYearToDateOfStatusDate.Available.Label, Does.StartWith($"{StaticTextKey.Available}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnBudgetStatementForLastYearOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastYearOfStatusDate.Header, Does.StartWith($"{StaticTextKey.BudgetStatementForLastYearOfStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForBudgetOnBudgetStatementForLastYearOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastYearOfStatusDate.Budget.Label, Does.StartWith($"{StaticTextKey.Budget}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForPostedOnBudgetStatementForLastYearOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastYearOfStatusDate.Posted.Label, Does.StartWith($"{StaticTextKey.Result}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForAvailableOnBudgetStatementForLastYearOfStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetStatementForLastYearOfStatusDate.Available.Label, Does.StartWith($"{StaticTextKey.Available}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnObligeePartiesAtStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtStatusDate.Header, Does.StartWith($"{StaticTextKey.ObligeePartiesAtStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForDebtorsOnObligeePartiesAtStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtStatusDate.Debtors.Label, Does.StartWith($"{StaticTextKey.Debtors}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForCreditorsOnObligeePartiesAtStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtStatusDate.Creditors.Label, Does.StartWith($"{StaticTextKey.Creditors}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnObligeePartiesAtEndOfLastMonthFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtEndOfLastMonthFromStatusDate.Header, Does.StartWith($"{StaticTextKey.ObligeePartiesAtEndOfLastMonthFromStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForDebtorsOnObligeePartiesAtEndOfLastMonthFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtEndOfLastMonthFromStatusDate.Debtors.Label, Does.StartWith($"{StaticTextKey.Debtors}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForCreditorsOnObligeePartiesAtEndOfLastMonthFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtEndOfLastMonthFromStatusDate.Creditors.Label, Does.StartWith($"{StaticTextKey.Creditors}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereHeaderOnObligeePartiesAtEndOfLastYearFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtEndOfLastYearFromStatusDate.Header, Does.StartWith($"{StaticTextKey.ObligeePartiesAtEndOfLastYearFromStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForDebtorsOnObligeePartiesAtEndOfLastYearFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtEndOfLastYearFromStatusDate.Debtors.Label, Does.StartWith($"{StaticTextKey.Debtors}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelForCreditorsOnObligeePartiesAtEndOfLastYearFromStatusDateIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ObligeePartiesAtEndOfLastYearFromStatusDate.Creditors.Label, Does.StartWith($"{StaticTextKey.Creditors}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereIncomeStatementLabelOnIncomeStatementIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.IncomeStatement.IncomeStatementLabel, Does.StartWith($"{StaticTextKey.IncomeStatement}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereMonthOfStatusDateLabelOnIncomeStatementIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.IncomeStatement.MonthOfStatusDateLabel, Does.StartWith($"{StaticTextKey.BudgetStatementForMonthOfStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLastMonthOfStatusDateLabelOnIncomeStatementIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.IncomeStatement.LastMonthOfStatusDateLabel, Does.StartWith($"{StaticTextKey.BudgetStatementForLastMonthOfStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereYearToDateOfStatusDateLabelOnIncomeStatementIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.IncomeStatement.YearToDateOfStatusDateLabel, Does.StartWith($"{StaticTextKey.BudgetStatementForYearToDateOfStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLastYearOfStatusDateLabelOnIncomeStatementIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.IncomeStatement.LastYearOfStatusDateLabel, Does.StartWith($"{StaticTextKey.BudgetStatementForLastYearOfStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereBudgetLabelOnIncomeStatementIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.IncomeStatement.BudgetLabel, Does.StartWith($"{StaticTextKey.Budget}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWherePostedLabelOnIncomeStatementIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.IncomeStatement.PostedLabel, Does.StartWith($"{StaticTextKey.Result}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereAvailableLabelOnIncomeStatementIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.IncomeStatement.AvailableLabel, Does.StartWith($"{StaticTextKey.Available}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelOnStatusDateAtIncomeStatementIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.IncomeStatement.StatusDate.Label, Does.StartWith($"{StaticTextKey.StatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(2024, 1, 1)]
    [TestCase(2024, 6, 15)]
    [TestCase(2024, 12, 31)]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereValueOnStatusDateAtIncomeStatementIsEqualToFormatedDate(int year, int month, int day)
    {
        IAccountingTextsBuilder sut = CreateSut();

        DateTimeOffset statusDate = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!, statusDate: statusDate);
        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel(accountingModel: accountingModel);
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.IncomeStatement.StatusDate.Value, Is.EqualTo(statusDate.ToString("D", CultureInfo.InvariantCulture)));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereIncomeStatementHasLines()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.IncomeStatement.Lines.Count, Is.GreaterThan(0));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereBalanceSheetLabelOnBalanceSheetIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheet.BalanceSheetLabel, Does.StartWith($"{StaticTextKey.BalanceSheet}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereBalanceSheetAtStatusDateLabelOnBalanceSheetIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheet.BalanceSheetAtStatusDateLabel, Does.StartWith($"{StaticTextKey.BalanceSheetAtStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereBalanceSheetAtEndOfLastMonthFromStatusDateLabelOnBalanceSheetIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheet.BalanceSheetAtEndOfLastMonthFromStatusDateLabel, Does.StartWith($"{StaticTextKey.BalanceSheetAtEndOfLastMonthFromStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereBalanceSheetAtEndOfLastYearFromStatusDateLabelOnBalanceSheetIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheet.BalanceSheetAtEndOfLastYearFromStatusDateLabel, Does.StartWith($"{StaticTextKey.BalanceSheetAtEndOfLastYearFromStatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereAssetsLabelOnBalanceSheetIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheet.AssetsLabel, Does.StartWith($"{StaticTextKey.Assets}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLiabilitiesLabelOnBalanceSheetIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheet.LiabilitiesLabel, Does.StartWith($"{StaticTextKey.Liabilities}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelOnStatusDateAtBalanceSheetIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheet.StatusDate.Label, Does.StartWith($"{StaticTextKey.StatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(2024, 1, 1)]
    [TestCase(2024, 6, 15)]
    [TestCase(2024, 12, 31)]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereValueOnStatusDateAtBalanceSheetIsEqualToFormatedDate(int year, int month, int day)
    {
        IAccountingTextsBuilder sut = CreateSut();

        DateTimeOffset statusDate = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!, statusDate: statusDate);
        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel(accountingModel: accountingModel);
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.BalanceSheet.StatusDate.Value, Is.EqualTo(statusDate.ToString("D", CultureInfo.InvariantCulture)));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereChartOfAccountsLabelOnChartOfAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfAccounts.ChartOfAccountsLabel, Does.StartWith($"{StaticTextKey.Accounts}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereAccountNumberLabelOnChartOfAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfAccounts.AccountNumberLabel, Does.StartWith($"{StaticTextKey.AccountNumberShort}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereAccountNameLabelOnChartOfAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfAccounts.AccountNameLabel, Does.StartWith($"{StaticTextKey.AccountName}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereCreditLabelOnChartOfAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfAccounts.CreditLabel, Does.StartWith($"{StaticTextKey.Credit}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereBalanceLabelOnChartOfAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfAccounts.BalanceLabel, Does.StartWith($"{StaticTextKey.Balance}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereAvailableLabelOnChartOfAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfAccounts.AvailableLabel, Does.StartWith($"{StaticTextKey.Available}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelOnStatusDateAtChartOfAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfAccounts.StatusDate.Label, Does.StartWith($"{StaticTextKey.StatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(2024, 1, 1)]
    [TestCase(2024, 6, 15)]
    [TestCase(2024, 12, 31)]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereValueOnStatusDateAtChartOfAccountsIsEqualToFormatedDate(int year, int month, int day)
    {
        IAccountingTextsBuilder sut = CreateSut();

        DateTimeOffset statusDate = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!, statusDate: statusDate);
        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel(accountingModel: accountingModel);
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfAccounts.StatusDate.Value, Is.EqualTo(statusDate.ToString("D", CultureInfo.InvariantCulture)));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereAccountCreationPossibleOnChartOfAccountsIsEqualToModifiableFromAccountingModel(bool modifiable)
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!, modifiable: modifiable);
        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel(accountingModel: accountingModel);
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfAccounts.AccountCreationPossible, Is.EqualTo(modifiable));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereSectionsOnChartOfAccountsIsNotEmpty()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfAccounts.Sections, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereChartOfBudgetAccountsLabelOnChartOfBudgetAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfBudgetAccounts.ChartOfBudgetAccountsLabel, Does.StartWith($"{StaticTextKey.BudgetAccounts}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereAccountNumberLabelOnChartOfBudgetAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel(accountingModel: accountingModel);
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfBudgetAccounts.AccountNumberLabel, Does.StartWith($"{StaticTextKey.AccountNumberShort}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereAccountNameLabelOnChartOfBudgetAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfBudgetAccounts.AccountNameLabel, Does.StartWith($"{StaticTextKey.AccountName}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereBudgetLabelOnChartOfBudgetAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfBudgetAccounts.BudgetLabel, Does.StartWith($"{StaticTextKey.Budget}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWherePostedLabelOnChartOfBudgetAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfBudgetAccounts.PostedLabel, Does.StartWith($"{StaticTextKey.Posted}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereAvailableLabelOnChartOfBudgetAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfBudgetAccounts.AvailableLabel, Does.StartWith($"{StaticTextKey.Available}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelOnStatusDateAtChartOfBudgetAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfBudgetAccounts.StatusDate.Label, Does.StartWith($"{StaticTextKey.StatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(2024, 1, 1)]
    [TestCase(2024, 6, 15)]
    [TestCase(2024, 12, 31)]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereValueOnStatusDateAtChartOfBudgetAccountsIsEqualToFormatedDate(int year, int month, int day)
    {
        IAccountingTextsBuilder sut = CreateSut();

        DateTimeOffset statusDate = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!, statusDate: statusDate);
        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel(accountingModel: accountingModel);
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfBudgetAccounts.StatusDate.Value, Is.EqualTo(statusDate.ToString("D", CultureInfo.InvariantCulture)));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereBudgetAccountCreationPossibleOnChartOfBudgetAccountsIsEqualToModifiableFromAccountingModel(bool modifiable)
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!, modifiable: modifiable);
        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel(accountingModel: accountingModel);
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfBudgetAccounts.BudgetAccountCreationPossible, Is.EqualTo(modifiable));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereSectionsOnChartOfBudgetAccountsIsNotEmpty()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfBudgetAccounts.Sections, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereChartOfContactAccountsLabelOnChartOfContactAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfContactAccounts.ChartOfContactAccountsLabel, Does.StartWith($"{StaticTextKey.ContactAccounts}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereAccountNumberLabelOnChartOfContactAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfContactAccounts.AccountNumberLabel, Does.StartWith($"{StaticTextKey.AccountNumberShort}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereAccountNameLabelOnChartOfContactAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfContactAccounts.AccountNameLabel, Does.StartWith($"{StaticTextKey.AccountName}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereBalanceLabelOnChartOfContactAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfContactAccounts.BalanceLabel, Does.StartWith($"{StaticTextKey.Balance}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLabelOnStatusDateAtChartOfContactAccountsIsEqualToStaticTextFromStaticTextProvider()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfContactAccounts.StatusDate.Label, Does.StartWith($"{StaticTextKey.StatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(2024, 1, 1)]
    [TestCase(2024, 6, 15)]
    [TestCase(2024, 12, 31)]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereValueOnStatusDateAtChartOfContactAccountsIsEqualToFormatedDate(int year, int month, int day)
    {
        IAccountingTextsBuilder sut = CreateSut();

        DateTimeOffset statusDate = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!, statusDate: statusDate);
        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel(accountingModel: accountingModel);
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfContactAccounts.StatusDate.Value, Is.EqualTo(statusDate.ToString("D", CultureInfo.InvariantCulture)));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereContactAccountCreationPossibleOnChartOfContactAccountsIsEqualToModifiableFromAccountingModel(bool modifiable)
    {
        IAccountingTextsBuilder sut = CreateSut();

        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!, modifiable: modifiable);
        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel(accountingModel: accountingModel);
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfContactAccounts.ContactAccountCreationPossible, Is.EqualTo(modifiable));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLinesOnChartOfContactAccountsIsNotEmpty()
    {
        IAccountingTextsBuilder sut = CreateSut();

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ChartOfContactAccounts.Lines, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWherePostingLineCollectionIsEqualToPostingLineCollectionTextsFromPostingLineCollectionTextsBuilder()
    {
        IPostingLineCollectionTexts postingLineCollectionTexts = new Mock<IPostingLineCollectionTexts>().Object;
        IAccountingTextsBuilder sut = CreateSut(postingLineCollectionTexts);

        Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model = CreateModel();
        IAccountingTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.PostingLineCollection, Is.EqualTo(postingLineCollectionTexts));
    }

    private IAccountingTextsBuilder CreateSut(IPostingLineCollectionTexts? postingLineCollectionTexts = null)
    {
        _postingLineCollectionTextsBuilderMock!.Setup(postingLineCollectionTexts: postingLineCollectionTexts);
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.DynamicText.AccountingTextsBuilder(_postingLineCollectionTextsBuilderMock!.Object, _staticTextProviderMock!.Object);
    }

    private Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> CreateModel(AccountingModel? accountingModel = null, IReadOnlyCollection<PostingLineModel>? postingLineModels = null, IReadOnlyCollection<LetterHeadIdentificationModel>? letterHeadIdentificationModels = null)
    {
        return new Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>>(
            accountingModel ?? _fixture!.CreateAccountingModel(_random!),
            postingLineModels ?? _fixture!.CreatePostingLineModels(_random!),
            letterHeadIdentificationModels ?? _fixture!.CreateLetterHeadIdentificationModels(_random!));
    }
}