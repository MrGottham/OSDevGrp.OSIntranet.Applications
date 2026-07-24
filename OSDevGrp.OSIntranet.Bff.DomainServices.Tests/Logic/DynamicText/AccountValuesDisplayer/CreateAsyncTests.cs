using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;
using AccountValuesDisplayerImpl = OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText.AccountValuesDisplayer;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.AccountValuesDisplayer;

[TestFixture]
public class CreateAsyncTests
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

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await AccountValuesDisplayerImpl.CreateAsync(StaticTextKey.AccountValuesAtStatusDate, null!, _staticTextProviderMock!.Object, formatProvider));

        Assert.That(result?.ParamName, Is.EqualTo("values"));
    }

    [Test]
    [Category("UnitTest")]
    public void CreateAsync_WhenStaticTextProviderIsNull_ThrowsArgumentNullException()
    {
        CreditInfoValuesModel values = _fixture!.Create<CreditInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await AccountValuesDisplayerImpl.CreateAsync(StaticTextKey.AccountValuesAtStatusDate, values, null!, formatProvider));

        Assert.That(result?.ParamName, Is.EqualTo("staticTextProvider"));
    }

    [Test]
    [Category("UnitTest")]
    public void CreateAsync_WhenFormatProviderIsNull_ThrowsArgumentNullException()
    {
        CreditInfoValuesModel values = _fixture!.Create<CreditInfoValuesModel>();

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await AccountValuesDisplayerImpl.CreateAsync(StaticTextKey.AccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, null!));

        Assert.That(result?.ParamName, Is.EqualTo("formatProvider"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountValuesAtStatusDate, 0)]
    [TestCase(StaticTextKey.AccountValuesAtEndOfLastMonthFromStatusDateAtStatusDate, 0)]
    [TestCase(StaticTextKey.AccountValuesAtEndOfLastYearFromStatusDate, 0)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithHeaderKey(StaticTextKey headerKey, int expectedHeaderCallCount)
    {
        CreditInfoValuesModel values = _fixture!.Create<CreditInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await AccountValuesDisplayerImpl.CreateAsync(headerKey, values, _staticTextProviderMock!.Object, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == headerKey),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithCreditKey()
    {
        CreditInfoValuesModel values = _fixture!.Create<CreditInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await AccountValuesDisplayerImpl.CreateAsync(StaticTextKey.AccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.Credit),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithBalanceKey()
    {
        CreditInfoValuesModel values = _fixture!.Create<CreditInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await AccountValuesDisplayerImpl.CreateAsync(StaticTextKey.AccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.Balance),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithAvailableKey()
    {
        CreditInfoValuesModel values = _fixture!.Create<CreditInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await AccountValuesDisplayerImpl.CreateAsync(StaticTextKey.AccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.Available),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsAccountValuesDisplayer()
    {
        CreditInfoValuesModel values = _fixture!.Create<CreditInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IAccountValuesDisplayer result = await AccountValuesDisplayerImpl.CreateAsync(StaticTextKey.AccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result, Is.TypeOf<AccountValuesDisplayerImpl>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsAccountValuesDisplayerWithHeaderNotNull()
    {
        CreditInfoValuesModel values = _fixture!.Create<CreditInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IAccountValuesDisplayer result = await AccountValuesDisplayerImpl.CreateAsync(StaticTextKey.AccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result.Header, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsAccountValuesDisplayerWithCreditNotNull()
    {
        CreditInfoValuesModel values = _fixture!.Create<CreditInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IAccountValuesDisplayer result = await AccountValuesDisplayerImpl.CreateAsync(StaticTextKey.AccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result.Credit, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsAccountValuesDisplayerWithBalanceNotNull()
    {
        CreditInfoValuesModel values = _fixture!.Create<CreditInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IAccountValuesDisplayer result = await AccountValuesDisplayerImpl.CreateAsync(StaticTextKey.AccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result.Balance, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsAccountValuesDisplayerWithAvailableNotNull()
    {
        CreditInfoValuesModel values = _fixture!.Create<CreditInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IAccountValuesDisplayer result = await AccountValuesDisplayerImpl.CreateAsync(StaticTextKey.AccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result.Available, Is.Not.Null);
    }
}
