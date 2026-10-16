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

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.ContactAccountTextsBuilder;

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
        IContactAccountTextsBuilder sut = CreateSut();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.BuildAsync((ContactAccountModel)null!, formatProvider));

        Assert.That(result?.ParamName, Is.EqualTo("model"));
    }

    [Test]
    [Category("UnitTest")]
    public void BuildAsync_WhenFormatProviderIsNull_ThrowsArgumentNullException()
    {
        IContactAccountTextsBuilder sut = CreateSut();
        ContactAccountModel model = _fixture!.Create<ContactAccountModel>();

        ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.BuildAsync(model, null!));

        Assert.That(result?.ParamName, Is.EqualTo("formatProvider"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.StatusDate, 1)]
    [TestCase(StaticTextKey.ContactAccountValuesAtStatusDate, 1)]
    [TestCase(StaticTextKey.Balance, 3)]
    [TestCase(StaticTextKey.ContactAccountValuesAtEndOfLastMonthFromStatusDate, 1)]
    [TestCase(StaticTextKey.ContactAccountValuesAtEndOfLastYearFromStatusDate, 1)]
    public async Task BuildAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithExpectedStaticTextKeys(StaticTextKey staticTextKey, int expectedCalls)
    {
        IContactAccountTextsBuilder sut = CreateSut();

        ContactAccountModel model = _fixture!.CreateContactAccountModel(_random!);
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
    public async Task BuildAsync_WhenCalled_ReturnsContactAccountTexts()
    {
        IContactAccountTextsBuilder sut = CreateSut();

        ContactAccountModel model = _fixture!.CreateContactAccountModel(_random!);
        IContactAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<ContactAccountTexts>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsContactAccountTextsWithStatusDateNotNull()
    {
        IContactAccountTextsBuilder sut = CreateSut();

        ContactAccountModel model = _fixture!.CreateContactAccountModel(_random!);
        IContactAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.StatusDate, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsContactAccountTextsWithValuesAtStatusDateNotNull()
    {
        IContactAccountTextsBuilder sut = CreateSut();

        ContactAccountModel model = _fixture!.CreateContactAccountModel(_random!);
        IContactAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValuesAtStatusDate, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsContactAccountTextsWithValuesAtEndOfLastMonthFromStatusDateNotNull()
    {
        IContactAccountTextsBuilder sut = CreateSut();

        ContactAccountModel model = _fixture!.CreateContactAccountModel(_random!);
        IContactAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsContactAccountTextsWithValuesAtEndOfLastYearFromStatusDateNotNull()
    {
        IContactAccountTextsBuilder sut = CreateSut();

        ContactAccountModel model = _fixture!.CreateContactAccountModel(_random!);
        IContactAccountTexts result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
    }

    #region Private methods

    private IContactAccountTextsBuilder CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.DynamicText.ContactAccountTextsBuilder(_staticTextProviderMock!.Object);
    }

    #endregion
}