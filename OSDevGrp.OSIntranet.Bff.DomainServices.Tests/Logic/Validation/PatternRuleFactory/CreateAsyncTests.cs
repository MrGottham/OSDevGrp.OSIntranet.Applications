using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PatternRuleFactory;

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
        IPatternRuleFactory sut = CreateSut();

        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, CreatePattern(), CultureInfo.InvariantCulture);

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
        IPatternRuleFactory sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, CreatePattern(), formatProvider);

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
        IPatternRuleFactory sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, CreatePattern(), CultureInfo.InvariantCulture, cancellationToken);

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
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForPatternValidationError(StaticTextKey staticTextKey)
    {
        IPatternRuleFactory sut = CreateSut();

        Regex pattern = CreatePattern();
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, pattern, CultureInfo.InvariantCulture);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.PatternValidationError),
                It.Is<IEnumerable<object>>(value => value.Count() == 2 && ((string)value.ElementAt(0)).StartsWith($"{staticTextKey}:") && ((string)value.ElementAt(1)) == pattern.ToString()),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForPatternValidationErrorAndGivenFormatProvider(StaticTextKey staticTextKey)
    {
        IPatternRuleFactory sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, CreatePattern(), formatProvider);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.PatternValidationError),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForPatternValidationErrorAndGivenCancellationToken(StaticTextKey staticTextKey)
    {
        IPatternRuleFactory sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, CreatePattern(), CultureInfo.InvariantCulture, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.PatternValidationError),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsPatternRule(StaticTextKey staticTextKey)
    {
        IPatternRuleFactory sut = CreateSut();

        IPatternRule result = (IPatternRule)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, CreatePattern(), CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<PatternRule>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsPatternRuleWhereNameIsEqualToGivenName(StaticTextKey staticTextKey)
    {
        IPatternRuleFactory sut = CreateSut();

        string name = _fixture!.Create<string>();
        IPatternRule result = (IPatternRule)await sut.CreateAsync(name, staticTextKey, CreatePattern(), CultureInfo.InvariantCulture);

        Assert.That(result.Name, Is.EqualTo(name));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsPatternRuleWhereRuleTypeIsEqualToPatternRule(StaticTextKey staticTextKey)
    {
        IPatternRuleFactory sut = CreateSut();

        IPatternRule result = (IPatternRule)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, CreatePattern(), CultureInfo.InvariantCulture);

        Assert.That(result.RuleType, Is.EqualTo(ValidationRuleType.PatternRule));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsPatternRuleWherePatternIsEqualToGivenPattern(StaticTextKey staticTextKey)
    {
        IPatternRuleFactory sut = CreateSut();

        Regex pattern = CreatePattern();
        IPatternRule result = (IPatternRule)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, pattern, CultureInfo.InvariantCulture);

        Assert.That(result.Pattern, Is.EqualTo(pattern));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsPatternRuleWhereValidationErrorIsEqualToStaticTextResolvedByStaticTextProvider(StaticTextKey staticTextKey)
    {
        IPatternRuleFactory sut = CreateSut();

        IPatternRule result = (IPatternRule)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, CreatePattern(), CultureInfo.InvariantCulture);

        Assert.That(result.ValidationError, Does.StartWith($"{StaticTextKey.PatternValidationError}:"));
    }

    private IPatternRuleFactory CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.Validation.PatternRuleFactory(_staticTextProviderMock!.Object);
    }

    private static Regex CreatePattern()
    {
        return new Regex("^[0-9A-Za-z]{32}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(32));
    }
}