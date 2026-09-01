using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;
using BudgetAccountValuesDisplayerImpl = OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText.BudgetAccountValuesDisplayer;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText;

[TestFixture]
public class BudgetAccountValuesDisplayerCreateAsyncTests
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
    public void CreateAsync_WhenValuesIsNull_ThrowsArgumentNullException()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BudgetAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, null!, _staticTextProviderMock!.Object, formatProvider));

        Assert.That(result?.ParamName, Is.EqualTo("values"));
    }

    [Test]
    [Category("UnitTest")]
    public void CreateAsync_WhenStaticTextProviderIsNull_ThrowsArgumentNullException()
    {
        BudgetInfoValuesModel values = _fixture!.Create<BudgetInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BudgetAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, values, null!, formatProvider));

        Assert.That(result?.ParamName, Is.EqualTo("staticTextProvider"));
    }

    [Test]
    [Category("UnitTest")]
    public void CreateAsync_WhenFormatProviderIsNull_ThrowsArgumentNullException()
    {
        BudgetInfoValuesModel values = _fixture!.Create<BudgetInfoValuesModel>();

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BudgetAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, values, _staticTextProviderMock!.Object, null!));

        Assert.That(result?.ParamName, Is.EqualTo("formatProvider"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, 0)]
    [TestCase(StaticTextKey.BudgetAccountValuesForLastMonthOfStatusDate, 0)]
    [TestCase(StaticTextKey.BudgetAccountValuesForYearToDateOfStatusDate, 0)]
    [TestCase(StaticTextKey.BudgetAccountValuesForLastYearOfStatusDate, 0)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithHeaderKey(StaticTextKey headerKey, int expectedHeaderCallCount)
    {
        BudgetInfoValuesModel values = _fixture!.Create<BudgetInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await BudgetAccountValuesDisplayerImpl.CreateAsync(headerKey, values, _staticTextProviderMock!.Object, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == headerKey),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithBudgetKey()
    {
        BudgetInfoValuesModel values = _fixture!.Create<BudgetInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await BudgetAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, values, _staticTextProviderMock!.Object, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.Budget),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithPostedKey()
    {
        BudgetInfoValuesModel values = _fixture!.Create<BudgetInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await BudgetAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, values, _staticTextProviderMock!.Object, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.Posted),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithAvailableKey()
    {
        BudgetInfoValuesModel values = _fixture!.Create<BudgetInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await BudgetAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, values, _staticTextProviderMock!.Object, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.Available),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsBudgetAccountValuesDisplayer()
    {
        BudgetInfoValuesModel values = _fixture!.Create<BudgetInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IBudgetAccountValuesDisplayer result = await BudgetAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result, Is.TypeOf<BudgetAccountValuesDisplayerImpl>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsBudgetAccountValuesDisplayerWithHeaderNotNull()
    {
        BudgetInfoValuesModel values = _fixture!.Create<BudgetInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IBudgetAccountValuesDisplayer result = await BudgetAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result.Header, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsBudgetAccountValuesDisplayerWithBudgetNotNull()
    {
        BudgetInfoValuesModel values = _fixture!.Create<BudgetInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IBudgetAccountValuesDisplayer result = await BudgetAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result.Budget, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsBudgetAccountValuesDisplayerWithPostedNotNull()
    {
        BudgetInfoValuesModel values = _fixture!.Create<BudgetInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IBudgetAccountValuesDisplayer result = await BudgetAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result.Posted, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsBudgetAccountValuesDisplayerWithAvailableNotNull()
    {
        BudgetInfoValuesModel values = _fixture!.Create<BudgetInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IBudgetAccountValuesDisplayer result = await BudgetAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result.Available, Is.Not.Null);
    }
}