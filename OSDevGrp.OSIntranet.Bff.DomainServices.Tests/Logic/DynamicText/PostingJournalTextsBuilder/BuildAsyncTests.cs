using System.Globalization;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.PostingJournalTextsBuilder;

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
    [TestCase(StaticTextKey.PostingJournal, 1)]
    [TestCase(StaticTextKey.PostingDate, 1)]
    [TestCase(StaticTextKey.PostingReference, 1)]
    [TestCase(StaticTextKey.Account, 1)]
    [TestCase(StaticTextKey.AccountName, 1)]
    [TestCase(StaticTextKey.PostingText, 1)]
    [TestCase(StaticTextKey.BudgetAccount, 1)]
    [TestCase(StaticTextKey.Debit, 1)]
    [TestCase(StaticTextKey.Credit, 1)]
    [TestCase(StaticTextKey.PostingValue, 1)]
    [TestCase(StaticTextKey.Balance, 1)]
    [TestCase(StaticTextKey.Budget, 1)]
    [TestCase(StaticTextKey.Posted, 1)]
    [TestCase(StaticTextKey.Available, 1)]
    [TestCase(StaticTextKey.ContactAccount, 1)]
    public async Task BuildAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithExpectedStaticTextKeys(StaticTextKey staticTextKey, int expectedCalls)
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        Tuple<ApplyPostingJournalModel, Predicate<int>> model = Tuple.Create(postingJournalModel, new Predicate<int>(_ => true));
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
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTexts()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<PostingJournalTexts>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWherePostingJournalHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.PostingJournalHeader, Does.StartWith($"{StaticTextKey.PostingJournal}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWherePostingDateHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.PostingDateHeader, Does.StartWith($"{StaticTextKey.PostingDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWherePostingReferenceHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.PostingReferenceHeader, Does.StartWith($"{StaticTextKey.PostingReference}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereAccountHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.AccountHeader, Does.StartWith($"{StaticTextKey.Account}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereAccountNameLabelIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.AccountNameLabel, Does.StartWith($"{StaticTextKey.AccountName}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereAccountCreditLabelIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.AccountCreditLabel, Does.StartWith($"{StaticTextKey.Credit}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereAccountBalanceLabelIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.AccountBalanceLabel, Does.StartWith($"{StaticTextKey.Balance}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereAccountAvailableLabelIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.AccountAvailableLabel, Does.StartWith($"{StaticTextKey.Available}:"));
    }


    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWherePostingTextHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.PostingTextHeader, Does.StartWith($"{StaticTextKey.PostingText}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereBudgetAccountHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.BudgetAccountHeader, Does.StartWith($"{StaticTextKey.BudgetAccount}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereBudgetAccountNameLabelIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.BudgetAccountNameLabel, Does.StartWith($"{StaticTextKey.AccountName}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereBudgetAccountBudgetLabelIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.BudgetAccountBudgetLabel, Does.StartWith($"{StaticTextKey.Budget}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereBudgetAccountPostedLabelIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.BudgetAccountPostedLabel, Does.StartWith($"{StaticTextKey.Posted}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereBudgetAccountAvailableLabelIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.BudgetAccountAvailableLabel, Does.StartWith($"{StaticTextKey.Available}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereDebitHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.DebitHeader, Does.StartWith($"{StaticTextKey.Debit}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereCreditHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.CreditHeader, Does.StartWith($"{StaticTextKey.Credit}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWherePostingValueHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.PostingValueHeader, Does.StartWith($"{StaticTextKey.PostingValue}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereContactAccountHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.ContactAccountHeader, Does.StartWith($"{StaticTextKey.ContactAccount}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereContactAccountNameLabelIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.ContactAccountNameLabel, Does.StartWith($"{StaticTextKey.AccountName}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereContactAccountBalanceLabelIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.ContactAccountBalanceLabel, Does.StartWith($"{StaticTextKey.Balance}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereAccountingNumberIsEqualToAccountingNumberFromModel()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.AccountingNumber, Is.EqualTo(postingJournalModel.AccountingNumber));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWhereModifiableIsEqualToPredicateResult(bool modifiable)
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => modifiable)), CultureInfo.InvariantCulture);

        Assert.That(result.Modifiable, Is.EqualTo(modifiable));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertPredicateWasEvaluatedWithAccountingNumberFromPostingJournalModel()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        int accountingNumber = 0;
        Predicate<int> predicate = value =>
        {
            accountingNumber = value;
            return true;
        };

        await sut.BuildAsync(Tuple.Create(postingJournalModel, predicate), CultureInfo.InvariantCulture);

        Assert.That(accountingNumber, Is.EqualTo(postingJournalModel.AccountingNumber));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWherePostingJournalLinesIsNotEmpty()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(result.PostingJournalLines, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingJournalTextsWherePostingJournalLinesContainsEachPostingLine()
    {
        IPostingJournalTextsBuilder sut = CreateSut();

        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IPostingJournalTexts result = await sut.BuildAsync(Tuple.Create(postingJournalModel, new Predicate<int>(_ => true)), CultureInfo.InvariantCulture);

        Assert.That(postingJournalModel.ApplyPostingLines!.All(postingLine =>
            result.PostingJournalLines.Any(postingLineDisplayer => postingLineDisplayer.PostingJournalLine == postingLine)));
    }

    private IPostingJournalTextsBuilder CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.DynamicText.PostingJournalTextsBuilder(_staticTextProviderMock!.Object);
    }
}