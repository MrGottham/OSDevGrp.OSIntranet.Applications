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
    [TestCase(StaticTextKey.BalanceBelowZero)]
    [TestCase(StaticTextKey.Debtors)]
    [TestCase(StaticTextKey.Creditors)]
    [TestCase(StaticTextKey.BackDating)]
    [TestCase(StaticTextKey.Days)]
    [TestCase(StaticTextKey.Day)]
    public async Task BuildAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithExpectedStaticTextKeys(StaticTextKey staticTextKey)
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
            Times.Once);
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

    private IAccountingTextsBuilder CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.DynamicText.AccountingTextsBuilder(_staticTextProviderMock!.Object);
    }
}