using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxValueRuleFactory;

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
        IMaxValueRuleFactory sut = CreateSut();

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
        IMaxValueRuleFactory sut = CreateSut();

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
        IMaxValueRuleFactory sut = CreateSut();

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
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForMaxValueValidationError(StaticTextKey staticTextKey)
    {
        IMaxValueRuleFactory sut = CreateSut();

        int maxValue = _fixture!.Create<int>();
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, maxValue, CultureInfo.InvariantCulture);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.MaxValueValidationError),
                It.Is<IEnumerable<object>>(value => value.Count() == 2 && ((string) value.ElementAt(0)).StartsWith($"{staticTextKey}:") && ((int) value.ElementAt(1)) == maxValue),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForMaxValueValidationErrorAndGivenFormatProvider(StaticTextKey staticTextKey)
    {
        IMaxValueRuleFactory sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.Create<int>(), formatProvider);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.MaxValueValidationError),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForMaxValueValidationErrorAndGivenCancellationToken(StaticTextKey staticTextKey)
    {
        IMaxValueRuleFactory sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.Create<int>(), CultureInfo.InvariantCulture, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.MaxValueValidationError),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMaxValueRule(StaticTextKey staticTextKey)
    {
        IMaxValueRuleFactory sut = CreateSut();

        IMaxValueRule<int> result = (IMaxValueRule<int>)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture.Create<int>(), CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<MaxValueRule<int>>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMaxValueRuleWhereNameIsEqualToGivenName(StaticTextKey staticTextKey)
    {
        IMaxValueRuleFactory sut = CreateSut();

        string name = _fixture!.Create<string>();
        IMaxValueRule<int> result = (IMaxValueRule<int>)await sut.CreateAsync(name, staticTextKey, _fixture.Create<int>(), CultureInfo.InvariantCulture);

        Assert.That(result.Name, Is.EqualTo(name));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMaxValueRuleWhereRuleTypeIsEqualToMaxValueRule(StaticTextKey staticTextKey)
    {
        IMaxValueRuleFactory sut = CreateSut();

        IMaxValueRule<int> result = (IMaxValueRule<int>)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture.Create<int>(), CultureInfo.InvariantCulture);

        Assert.That(result.RuleType, Is.EqualTo(ValidationRuleType.MaxValueRule));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMaxValueRuleWhereMaxValueIsEqualToGivenMaxValue(StaticTextKey staticTextKey)
    {
        IMaxValueRuleFactory sut = CreateSut();

        int maxValue = _fixture!.Create<int>();
        IMaxValueRule<int> result = (IMaxValueRule<int>)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, maxValue, CultureInfo.InvariantCulture);

        Assert.That(result.MaxValue, Is.EqualTo(maxValue));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMaxValueRuleWhereValidationErrorIsEqualToStaticTextResolvedByStaticTextProvider(StaticTextKey staticTextKey)
    {
        IMaxValueRuleFactory sut = CreateSut();

        IMaxValueRule<int> result = (IMaxValueRule<int>)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture.Create<int>(), CultureInfo.InvariantCulture);

        Assert.That(result.ValidationError, Does.StartWith($"{StaticTextKey.MaxValueValidationError}:"));
    }

    private IMaxValueRuleFactory CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.Validation.MaxValueRuleFactory(_staticTextProviderMock!.Object);
    }
}