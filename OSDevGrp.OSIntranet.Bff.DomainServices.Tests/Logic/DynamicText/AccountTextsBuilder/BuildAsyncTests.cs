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

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.AccountTextsBuilder;

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
        IAccountTextsBuilder sut = CreateSut();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.BuildAsync((AccountModel)null!, formatProvider));

        Assert.That(result?.ParamName, Is.EqualTo("model"));
    }

    [Test]
    [Category("UnitTest")]
    public void BuildAsync_WhenFormatProviderIsNull_ThrowsArgumentNullException()
    {
        IAccountTextsBuilder sut = CreateSut();
        AccountModel model = _fixture!.Create<AccountModel>();

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.BuildAsync(model, null!));

        Assert.That(result?.ParamName, Is.EqualTo("formatProvider"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.StatusDate, 1)]
    [TestCase(StaticTextKey.AccountValuesAtStatusDate, 1)]
    [TestCase(StaticTextKey.Credit, 3)]
    [TestCase(StaticTextKey.Balance, 3)]
    [TestCase(StaticTextKey.Available, 3)]
    [TestCase(StaticTextKey.AccountValuesAtEndOfLastMonthFromStatusDateAtStatusDate, 1)]
    [TestCase(StaticTextKey.AccountValuesAtEndOfLastYearFromStatusDate, 1)]
    public async Task BuildAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithExpectedStaticTextKeys(StaticTextKey staticTextKey, int expectedCalls)
    {
        IAccountTextsBuilder sut = CreateSut();

        AccountModel model = _fixture!.CreateAccountModel(_random!);
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
    public async Task BuildAsync_WhenCalled_ReturnsAccountTexts()
    {
        IAccountTextsBuilder sut = CreateSut();

        AccountModel accountModel = _fixture!.CreateAccountModel(_random!);
        IAccountTexts result = await sut.BuildAsync(accountModel, CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<AccountTexts>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountTextsWithStatusDateNotNull()
    {
        IAccountTextsBuilder sut = CreateSut();

        AccountModel model = _fixture!.CreateAccountModel(_random!);
        IAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.StatusDate, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountTextsWithValuesAtStatusDateNotNull()
    {
        IAccountTextsBuilder sut = CreateSut();

        AccountModel model = _fixture!.CreateAccountModel(_random!);
        IAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValuesAtStatusDate, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountTextsWithValuesAtEndOfLastMonthFromStatusDateNotNull()
    {
        IAccountTextsBuilder sut = CreateSut();

        AccountModel model = _fixture!.CreateAccountModel(_random!);
        IAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountTextsWithValuesAtEndOfLastYearFromStatusDateNotNull()
    {
        IAccountTextsBuilder sut = CreateSut();

        AccountModel model = _fixture!.CreateAccountModel(_random!);
        IAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
    }

    #region Private methods

    private IAccountTextsBuilder CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.DynamicText.AccountTextsBuilder(_staticTextProviderMock!.Object);
    }

    #endregion
}