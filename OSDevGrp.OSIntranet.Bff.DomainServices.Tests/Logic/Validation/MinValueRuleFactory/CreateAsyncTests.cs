using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinValueRuleFactory;

[TestFixture]
public class CreateAsyncTests
{
    #region Private variables

    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithGivenStaticTextKey(StaticTextKey staticTextKey)
    {
        IMinValueRuleFactory sut = CreateSut();

        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.Create<int>(), CultureInfo.InvariantCulture);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.Is<IEnumerable<object>>(value => value.Any() == false),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithGivenStaticTextKeyAndGivenFormatProvider(StaticTextKey staticTextKey)
    {
        IMinValueRuleFactory sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.Create<int>(), formatProvider);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithGivenStaticTextKeyAndGivenCancellationToken(StaticTextKey staticTextKey)
    {
        IMinValueRuleFactory sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.Create<int>(), CultureInfo.InvariantCulture, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForMinValueValidationError(StaticTextKey staticTextKey)
    {
        IMinValueRuleFactory sut = CreateSut();

        int minValue = _fixture!.Create<int>();
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, minValue, CultureInfo.InvariantCulture);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.MinValueValidationError),
                It.Is<IEnumerable<object>>(value => value.Count() == 2 && ((string) value.ElementAt(0)).StartsWith($"{staticTextKey}:") && ((int) value.ElementAt(1)) == minValue),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForMinValueValidationErrorAndGivenFormatProvider(StaticTextKey staticTextKey)
    {
        IMinValueRuleFactory sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.Create<int>(), formatProvider);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.MinValueValidationError),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForMinValueValidationErrorAndGivenCancellationToken(StaticTextKey staticTextKey)
    {
        IMinValueRuleFactory sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.Create<int>(), CultureInfo.InvariantCulture, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.MinValueValidationError),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMinValueRule(StaticTextKey staticTextKey)
    {
        IMinValueRuleFactory sut = CreateSut();

        IMinValueRule<int> result = (IMinValueRule<int>)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture.Create<int>(), CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<MinValueRule<int>>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMinValueRuleWhereNameIsEqualToGivenName(StaticTextKey staticTextKey)
    {
        IMinValueRuleFactory sut = CreateSut();

        string name = _fixture!.Create<string>();
        IMinValueRule<int> result = (IMinValueRule<int>)await sut.CreateAsync(name, staticTextKey, _fixture.Create<int>(), CultureInfo.InvariantCulture);

        Assert.That(result.Name, Is.EqualTo(name));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMinValueRuleWhereRuleTypeIsEqualToMinValueRule(StaticTextKey staticTextKey)
    {
        IMinValueRuleFactory sut = CreateSut();

        IMinValueRule<int> result = (IMinValueRule<int>)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture.Create<int>(), CultureInfo.InvariantCulture);

        Assert.That(result.RuleType, Is.EqualTo(ValidationRuleType.MinValueRule));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMinValueRuleWhereMinValueIsEqualToGivenMinValue(StaticTextKey staticTextKey)
    {
        IMinValueRuleFactory sut = CreateSut();

        int minValue = _fixture!.Create<int>();
        IMinValueRule<int> result = (IMinValueRule<int>)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, minValue, CultureInfo.InvariantCulture);

        Assert.That(result.MinValue, Is.EqualTo(minValue));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMinValueRuleWhereValidationErrorIsEqualToStaticTextResolvedByStaticTextProvider(StaticTextKey staticTextKey)
    {
        IMinValueRuleFactory sut = CreateSut();

        IMinValueRule<int> result = (IMinValueRule<int>)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture.Create<int>(), CultureInfo.InvariantCulture);

        Assert.That(result.ValidationError, Does.StartWith($"{StaticTextKey.MinValueValidationError}:"));
    }

    private IMinValueRuleFactory CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.Validation.MinValueRuleFactory(_staticTextProviderMock!.Object);
    }
}