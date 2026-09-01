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

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.BudgetAccountTextsBuilder;

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
        
        _staticTextProviderMock.Setup(_fixture);
    }

    [Test]
    [Category("UnitTest")]
    public void BuildAsync_WhenModelIsNull_ThrowsArgumentNullException()
    {
        IBudgetAccountTextsBuilder sut = CreateSut();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.BuildAsync((BudgetAccountModel)null!, formatProvider));

        Assert.That(result?.ParamName, Is.EqualTo("model"));
    }

    [Test]
    [Category("UnitTest")]
    public void BuildAsync_WhenFormatProviderIsNull_ThrowsArgumentNullException()
    {
        IBudgetAccountTextsBuilder sut = CreateSut();
        BudgetAccountModel model = _fixture!.Create<BudgetAccountModel>();

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.BuildAsync(model, null!));

        Assert.That(result?.ParamName, Is.EqualTo("formatProvider"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.StatusDate, 1)]
    [TestCase(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, 1)]
    [TestCase(StaticTextKey.Budget, 4)]
    [TestCase(StaticTextKey.Posted, 4)]
    [TestCase(StaticTextKey.Available, 4)]
    [TestCase(StaticTextKey.BudgetAccountValuesForLastMonthOfStatusDate, 1)]
    [TestCase(StaticTextKey.BudgetAccountValuesForYearToDateOfStatusDate, 1)]
    [TestCase(StaticTextKey.BudgetAccountValuesForLastYearOfStatusDate, 1)]
    public async Task BuildAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithExpectedStaticTextKeys(StaticTextKey staticTextKey, int expectedCalls)
    {
        IBudgetAccountTextsBuilder sut = CreateSut();

        BudgetAccountModel model = _fixture!.CreateBudgetAccountModel(_random!);
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
    public async Task BuildAsync_WhenCalled_ReturnsBudgetAccountTexts()
    {
        IBudgetAccountTextsBuilder sut = CreateSut();

        BudgetAccountModel model = _fixture!.CreateBudgetAccountModel(_random!);
        IBudgetAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<BudgetAccountTexts>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsBudgetAccountTextsWithStatusDateNotNull()
    {
        IBudgetAccountTextsBuilder sut = CreateSut();

        BudgetAccountModel model = _fixture!.CreateBudgetAccountModel(_random!);
        IBudgetAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.StatusDate, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsBudgetAccountTextsWithValuesForMonthOfStatusDateNotNull()
    {
        IBudgetAccountTextsBuilder sut = CreateSut();

        BudgetAccountModel model = _fixture!.CreateBudgetAccountModel(_random!);
        IBudgetAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValuesForMonthOfStatusDate, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsBudgetAccountTextsWithValuesForLastMonthOfStatusDateNotNull()
    {
        IBudgetAccountTextsBuilder sut = CreateSut();

        BudgetAccountModel model = _fixture!.CreateBudgetAccountModel(_random!);
        IBudgetAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValuesForLastMonthOfStatusDate, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsBudgetAccountTextsWithValuesForYearToDateOfStatusDateNotNull()
    {
        IBudgetAccountTextsBuilder sut = CreateSut();

        BudgetAccountModel model = _fixture!.CreateBudgetAccountModel(_random!);
        IBudgetAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValuesForYearToDateOfStatusDate, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsBudgetAccountTextsWithValuesForLastYearOfStatusDateNotNull()
    {
        IBudgetAccountTextsBuilder sut = CreateSut();

        BudgetAccountModel model = _fixture!.CreateBudgetAccountModel(_random!);
        IBudgetAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValuesForLastYearOfStatusDate, Is.Not.Null);
    }

    #region Private methods

    private IBudgetAccountTextsBuilder CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.DynamicText.BudgetAccountTextsBuilder(_staticTextProviderMock!.Object);
    }

    #endregion
}