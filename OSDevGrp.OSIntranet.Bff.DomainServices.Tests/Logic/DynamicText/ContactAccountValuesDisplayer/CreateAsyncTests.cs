using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;
using ContactAccountValuesDisplayerImpl = OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText.ContactAccountValuesDisplayer;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText;

[TestFixture]
public class ContactAccountValuesDisplayerCreateAsyncTests
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

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await ContactAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.ContactAccountValuesAtStatusDate, null!, _staticTextProviderMock!.Object, formatProvider));

        Assert.That(result?.ParamName, Is.EqualTo("values"));
    }

    [Test]
    [Category("UnitTest")]
    public void CreateAsync_WhenStaticTextProviderIsNull_ThrowsArgumentNullException()
    {
        BalanceInfoValuesModel values = _fixture!.Create<BalanceInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await ContactAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.ContactAccountValuesAtStatusDate, values, null!, formatProvider));

        Assert.That(result?.ParamName, Is.EqualTo("staticTextProvider"));
    }

    [Test]
    [Category("UnitTest")]
    public void CreateAsync_WhenFormatProviderIsNull_ThrowsArgumentNullException()
    {
        BalanceInfoValuesModel values = _fixture!.Create<BalanceInfoValuesModel>();

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await ContactAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.ContactAccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, null!));

        Assert.That(result?.ParamName, Is.EqualTo("formatProvider"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.ContactAccountValuesAtStatusDate, 0)]
    [TestCase(StaticTextKey.ContactAccountValuesAtEndOfLastMonthFromStatusDate, 0)]
    [TestCase(StaticTextKey.ContactAccountValuesAtEndOfLastYearFromStatusDate, 0)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithHeaderKey(StaticTextKey headerKey, int expectedHeaderCallCount)
    {
        BalanceInfoValuesModel values = _fixture!.Create<BalanceInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await ContactAccountValuesDisplayerImpl.CreateAsync(headerKey, values, _staticTextProviderMock!.Object, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == headerKey),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithBalanceKey()
    {
        BalanceInfoValuesModel values = _fixture!.Create<BalanceInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await ContactAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.ContactAccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.Balance),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsContactAccountValuesDisplayer()
    {
        BalanceInfoValuesModel values = _fixture!.Create<BalanceInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IContactAccountValuesDisplayer result = await ContactAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.ContactAccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result, Is.TypeOf<ContactAccountValuesDisplayerImpl>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsContactAccountValuesDisplayerWithHeaderNotNull()
    {
        BalanceInfoValuesModel values = _fixture!.Create<BalanceInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IContactAccountValuesDisplayer result = await ContactAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.ContactAccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result.Header, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CreateAsync_WhenCalled_ReturnsContactAccountValuesDisplayerWithBalanceNotNull()
    {
        BalanceInfoValuesModel values = _fixture!.Create<BalanceInfoValuesModel>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        IContactAccountValuesDisplayer result = await ContactAccountValuesDisplayerImpl.CreateAsync(StaticTextKey.ContactAccountValuesAtStatusDate, values, _staticTextProviderMock!.Object, formatProvider);

        Assert.That(result.Balance, Is.Not.Null);
    }
}