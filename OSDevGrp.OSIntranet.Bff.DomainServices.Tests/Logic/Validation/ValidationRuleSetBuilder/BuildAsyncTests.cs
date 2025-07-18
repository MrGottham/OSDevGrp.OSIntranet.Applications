using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ValidationRuleSetBuilder;

[TestFixture]
public class BuildAsyncTests : ValidationRuleSetBuilderTestBase
{
    #region Private variables

    private Mock<IRequiredValueRuleFactory>? _requiredValueRuleFactoryMock;
    private Mock<IMinLengthRuleFactory>? _minLengthRuleFactoryMock;
    private Mock<IMaxLengthRuleFactory>? _maxLengthRuleFactoryMock;
    private Mock<IMinValueRuleFactory>? _minValueRuleFactoryMock;
    private Mock<IMaxValueRuleFactory>? _maxValueRuleFactoryMock;
    private Mock<IPatternRuleFactory>? _patternRuleFactoryMock;
    private Mock<IOneOfRuleFactory>? _oneOfRuleFactoryMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _requiredValueRuleFactoryMock = new Mock<IRequiredValueRuleFactory>();
        _minLengthRuleFactoryMock = new Mock<IMinLengthRuleFactory>();
        _maxLengthRuleFactoryMock = new Mock<IMaxLengthRuleFactory>();
        _minValueRuleFactoryMock = new Mock<IMinValueRuleFactory>();
        _maxValueRuleFactoryMock = new Mock<IMaxValueRuleFactory>();
        _patternRuleFactoryMock = new Mock<IPatternRuleFactory>();
        _oneOfRuleFactoryMock = new Mock<IOneOfRuleFactory>();
        _fixture = new Fixture();
        _random = new Random(_fixture!.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenNoValidationRulesHasBeenAdded_AssertCreateAsyncWasNotCalledOnRequiredValueRuleFactory()
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _requiredValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenNoValidationRulesHasBeenAdded_AssertCreateAsyncWasNotCalledOnMinLengthRuleFactory()
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _minLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenNoValidationRulesHasBeenAdded_AssertCreateAsyncWasNotCalledOnMaxLengthRuleFactory()
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _maxLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenNoValidationRulesHasBeenAdded_AssertCreateAsyncWasNotCalledOnMinValueRuleFactory()
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _minValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenNoValidationRulesHasBeenAdded_AssertCreateAsyncWasNotCalledOnMaxValueRuleFactoryMock()
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _maxValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenNoValidationRulesHasBeenAdded_AssertCreateAsyncWasNotCalledOnPatternRuleFactory()
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _patternRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<Regex>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenNoValidationRulesHasBeenAdded_AssertCreateAsyncWasNotCalledOnOneOfRuleFactory()
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _oneOfRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<IReadOnlyCollection<int>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenNoValidationRulesHasBeenAdded_ReturnsEmptyValidationRuleSet()
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRequiredValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnRequiredValueRuleFactoryWithGeneratedRuleName(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithRequiredValueRule(field).BuildAsync(CultureInfo.InvariantCulture);

        _requiredValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.Is<string>(value => value == $"{field}:{ValidationRuleType.RequiredValueRule}"),
                It.IsAny<StaticTextKey>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRequiredValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnRequiredValueRuleFactoryWithGivenStaticTextKey(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithRequiredValueRule(field).BuildAsync(CultureInfo.InvariantCulture);

        _requiredValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.Is<StaticTextKey>(value => value == field),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRequiredValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnRequiredValueRuleFactoryWithGivenFormatProvider(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.WithRequiredValueRule(field).BuildAsync(formatProvider);

        _requiredValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRequiredValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnRequiredValueRuleFactoryWithGivenCancellationToken(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.WithRequiredValueRule(field).BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _requiredValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRequiredValueRuleHasBeenCalled_ReturnsNonEmptyValidationRuleSet(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithRequiredValueRule(field).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRequiredValueRuleHasBeenCalled_ReturnsValidationRuleSetContainingOneRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithRequiredValueRule(field).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRequiredValueRuleHasBeenCalled_ReturnsValidationRuleSetContainingRequiredValueRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithRequiredValueRule(field).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.ElementAt(0), Is.InstanceOf<IRequiredValueRule>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinLengthRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinLengthRuleFactoryWithGeneratedRuleName(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithMinLengthRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _minLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.Is<string>(value => value == $"{field}:{ValidationRuleType.MinLengthRule}"),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinLengthRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinLengthRuleFactoryWithGivenStaticTextKey(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithMinLengthRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _minLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.Is<StaticTextKey>(value => value == field),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinLengthRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinLengthRuleFactoryWithGivenMinLength(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        int minLength = _fixture.Create<int>();
        await sut.WithMinLengthRule(field, minLength).BuildAsync(CultureInfo.InvariantCulture);

        _minLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.Is<int>(value => value == minLength),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinLengthRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinLengthRuleFactoryWithGivenFormatProvider(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.WithMinLengthRule(field, _fixture.Create<int>()).BuildAsync(formatProvider);

        _minLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinLengthRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinLengthRuleFactoryWithGivenCancellationToken(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.WithMinLengthRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _minLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinLengthRuleHasBeenCalled_ReturnsNonEmptyValidationRuleSet(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithMinLengthRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinLengthRuleHasBeenCalled_ReturnsValidationRuleSetContainingOneRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithMinLengthRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinLengthRuleHasBeenCalled_ReturnsValidationRuleSetContainingMinLengthRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithMinLengthRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.ElementAt(0), Is.InstanceOf<IMinLengthRule>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxLengthRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxLengthRuleFactoryWithGeneratedRuleName(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithMaxLengthRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _maxLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.Is<string>(value => value == $"{field}:{ValidationRuleType.MaxLengthRule}"),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxLengthRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxLengthRuleFactoryWithGivenStaticTextKey(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithMaxLengthRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _maxLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.Is<StaticTextKey>(value => value == field),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxLengthRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxLengthRuleFactoryWithGivenMaxLength(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        int maxLength = _fixture.Create<int>();
        await sut.WithMaxLengthRule(field, maxLength).BuildAsync(CultureInfo.InvariantCulture);

        _maxLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.Is<int>(value => value == maxLength),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxLengthRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxLengthRuleFactoryWithGivenFormatProvider(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.WithMaxLengthRule(field, _fixture.Create<int>()).BuildAsync(formatProvider);

        _maxLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxLengthRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxLengthRuleFactoryWithGivenCancellationToken(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.WithMaxLengthRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _maxLengthRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxLengthRuleHasBeenCalled_ReturnsNonEmptyValidationRuleSet(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithMaxLengthRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxLengthRuleHasBeenCalled_ReturnsValidationRuleSetContainingOneRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithMaxLengthRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);;

        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxLengthRuleHasBeenCalled_ReturnsValidationRuleSetContainingMaxLengthRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithMaxLengthRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.ElementAt(0), Is.InstanceOf<IMaxLengthRule>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinValueRuleFactoryWithGeneratedRuleName(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithMinValueRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _minValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.Is<string>(value => value == $"{field}:{ValidationRuleType.MinValueRule}"),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinValueRuleFactoryWithGivenStaticTextKey(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithMinValueRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _minValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.Is<StaticTextKey>(value => value == field),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinValueRuleFactoryWithGivenMinValue(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        int minValue = _fixture.Create<int>();
        await sut.WithMinValueRule(field, minValue).BuildAsync(CultureInfo.InvariantCulture);

        _minValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.Is<int>(value => value == minValue),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinValueRuleFactoryWithGivenFormatProvider(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.WithMinValueRule(field, _fixture.Create<int>()).BuildAsync(formatProvider);

        _minValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinValueRuleFactoryWithGivenCancellationToken(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.WithMinValueRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _minValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinValueRuleHasBeenCalled_ReturnsNonEmptyValidationRuleSet(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithMinValueRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinValueRuleHasBeenCalled_ReturnsValidationRuleSetContainingOneRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithMinValueRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMinValueRuleHasBeenCalled_ReturnsValidationRuleSetContainingMinValueRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithMinValueRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.ElementAt(0), Is.InstanceOf<IMinValueRule<int>>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxValueRuleFactoryWithGeneratedRuleName(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithMaxValueRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _maxValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.Is<string>(value => value == $"{field}:{ValidationRuleType.MaxValueRule}"),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxValueRuleFactoryWithGivenStaticTextKey(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithMaxValueRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _maxValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.Is<StaticTextKey>(value => value == field),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxValueRuleFactoryWithGivenMaxValue(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        int maxValue = _fixture.Create<int>();
        await sut.WithMaxValueRule(field, maxValue).BuildAsync(CultureInfo.InvariantCulture);

        _maxValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.Is<int>(value => value == maxValue),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxValueRuleFactoryWithGivenFormatProvider(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.WithMaxValueRule(field, _fixture.Create<int>()).BuildAsync(formatProvider);

        _maxValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxValueRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxValueRuleFactoryWithGivenCancellationToken(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.WithMaxValueRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _maxValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxValueRuleHasBeenCalled_ReturnsNonEmptyValidationRuleSet(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithMaxValueRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxValueRuleHasBeenCalled_ReturnsValidationRuleSetContainingOneRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithMaxValueRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);;

        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithMaxValueRuleHasBeenCalled_ReturnsValidationRuleSetContainingMaxValueRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithMaxValueRule(field, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.ElementAt(0), Is.InstanceOf<IMaxValueRule<int>>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinValueRuleFactoryWithGeneratedRuleName(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithRangeRule(field, _fixture.Create<int>(), _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _minValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.Is<string>(value => value == $"{field}:{ValidationRuleType.MinValueRule}"),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinValueRuleFactoryWithGivenStaticTextKey(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithRangeRule(field, _fixture.Create<int>(), _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _minValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.Is<StaticTextKey>(value => value == field),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinValueRuleFactoryWithGivenMinValue(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        int minValue = _fixture.Create<int>();
        await sut.WithRangeRule(field, minValue, _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _minValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.Is<int>(value => value == minValue),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinValueRuleFactoryWithGivenFormatProvider(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.WithRangeRule(field, _fixture.Create<int>(), _fixture.Create<int>()).BuildAsync(formatProvider);

        _minValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMinValueRuleFactoryWithGivenCancellationToken(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.WithRangeRule(field, _fixture.Create<int>(), _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _minValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxValueRuleFactoryWithGeneratedRuleName(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithRangeRule(field, _fixture.Create<int>(), _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _maxValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.Is<string>(value => value == $"{field}:{ValidationRuleType.MaxValueRule}"),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxValueRuleFactoryWithGivenStaticTextKey(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithRangeRule(field, _fixture.Create<int>(), _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        _maxValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.Is<StaticTextKey>(value => value == field),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxValueRuleFactoryWithGivenMaxValue(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        int maxValue = _fixture.Create<int>();
        await sut.WithRangeRule(field, _fixture.Create<int>(), maxValue).BuildAsync(CultureInfo.InvariantCulture);

        _maxValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.Is<int>(value => value == maxValue),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxValueRuleFactoryWithGivenFormatProvider(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.WithRangeRule(field, _fixture.Create<int>(), _fixture.Create<int>()).BuildAsync(formatProvider);

        _maxValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_AssertCreateAsyncWasCalledOnMaxValueRuleFactoryWithGivenCancellationToken(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.WithRangeRule(field, _fixture.Create<int>(), _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _maxValueRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<int>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_ReturnsNonEmptyValidationRuleSet(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithRangeRule(field, _fixture.Create<int>(), _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_ReturnsValidationRuleSetContainingTwoRules(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithRangeRule(field, _fixture.Create<int>(), _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);;

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_ReturnsValidationRuleSetContainingMinValueRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithRangeRule(field, _fixture.Create<int>(), _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.SingleOrDefault(m => m as IMinValueRule<int> != null), Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithRangeRuleHasBeenCalled_ReturnsValidationRuleSetContainingMaxValueRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithRangeRule(field, _fixture.Create<int>(), _fixture.Create<int>()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.SingleOrDefault(m => m as IMaxValueRule<int> != null), Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithPatternRuleHasBeenCalled_AssertCreateAsyncWasCalledOnPatternRuleFactoryWithGeneratedRuleName(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithPatternRule(field, CreatePattern(_fixture!)).BuildAsync(CultureInfo.InvariantCulture);

        _patternRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.Is<string>(value => value == $"{field}:{ValidationRuleType.PatternRule}"),
                It.IsAny<StaticTextKey>(),
                It.IsAny<Regex>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithPatternRuleHasBeenCalled_AssertCreateAsyncWasCalledOnPatternRuleFactoryWithGivenStaticTextKey(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithPatternRule(field, CreatePattern(_fixture!)).BuildAsync(CultureInfo.InvariantCulture);

        _patternRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.Is<StaticTextKey>(value => value == field),
                It.IsAny<Regex>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithPatternRuleHasBeenCalled_AssertCreateAsyncWasCalledOnPatternRuleFactoryWithRegularExpressionForGivenPattern(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        string pattern = CreatePattern(_fixture!);
        await sut.WithPatternRule(field, pattern).BuildAsync(CultureInfo.InvariantCulture);

        _patternRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.Is<Regex>(value => value.ToString() == pattern),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithPatternRuleHasBeenCalled_AssertCreateAsyncWasCalledOnPatternRuleFactoryWithGivenFormatProvider(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.WithPatternRule(field, CreatePattern(_fixture!)).BuildAsync(formatProvider);

        _patternRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<Regex>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithPatternRuleHasBeenCalled_AssertCreateAsyncWasCalledOnPatternRuleFactoryWithGivenCancellationToken(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.WithPatternRule(field, CreatePattern(_fixture!)).BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _patternRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<Regex>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithPatternRuleHasBeenCalled_ReturnsNonEmptyValidationRuleSet(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithPatternRule(field, CreatePattern(_fixture!)).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithPatternRuleHasBeenCalled_ReturnsValidationRuleSetContainingOneRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithPatternRule(field, CreatePattern(_fixture!)).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithPatternRuleHasBeenCalled_ReturnsValidationRuleSetContainingPatternRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithPatternRule(field, CreatePattern(_fixture!)).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.ElementAt(0), Is.InstanceOf<IPatternRule>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithOneOfRuleHasBeenCalled_AssertCreateAsyncWasCalledOnOneOfRuleFactoryWithGeneratedRuleName(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithOneOfRule(field, _fixture.CreateMany<int>(_random!.Next(1, 10)).ToArray()).BuildAsync(CultureInfo.InvariantCulture);

        _oneOfRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.Is<string>(value => value == $"{field}:{ValidationRuleType.OneOfRule}"),
                It.IsAny<StaticTextKey>(),
                It.IsAny<IReadOnlyCollection<int>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithOneOfRuleHasBeenCalled_AssertCreateAsyncWasCalledOnOneOfRuleFactoryWithGivenStaticTextKey(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        await sut.WithOneOfRule(field, _fixture.CreateMany<int>(_random!.Next(1, 10)).ToArray()).BuildAsync(CultureInfo.InvariantCulture);

        _oneOfRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.Is<StaticTextKey>(value => value == field),
                It.IsAny<IReadOnlyCollection<int>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithOneOfRuleHasBeenCalled_AssertCreateAsyncWasCalledOnOneOfRuleFactoryWithGivenValues(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        int[] values = _fixture.CreateMany<int>(_random!.Next(1, 10)).ToArray();
        await sut.WithOneOfRule(field, values).BuildAsync(CultureInfo.InvariantCulture);

        _oneOfRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.Is<IReadOnlyCollection<int>>(v => v == values),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithOneOfRuleHasBeenCalled_AssertCreateAsyncWasCalledOnOneOfRuleFactoryWithGivenFormatProvider(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.WithOneOfRule(field, _fixture.CreateMany<int>(_random!.Next(1, 10)).ToArray()).BuildAsync(formatProvider);

        _oneOfRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<IReadOnlyCollection<int>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithOneOfRuleHasBeenCalled_AssertCreateAsyncWasCalledOnOneOfRuleFactoryWithGivenCancellationToken(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.WithOneOfRule(field, _fixture.CreateMany<int>(_random!.Next(1, 10)).ToArray()).BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _oneOfRuleFactoryMock!.Verify(m => m.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<StaticTextKey>(),
                It.IsAny<IReadOnlyCollection<int>>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithOneOfRuleHasBeenCalled_ReturnsNonEmptyValidationRuleSet(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithOneOfRule(field, _fixture.CreateMany<int>(_random!.Next(1, 10)).ToArray()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithOneOfRuleHasBeenCalled_ReturnsValidationRuleSetContainingOneRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithOneOfRule(field, _fixture.CreateMany<int>(_random!.Next(1, 10)).ToArray()).BuildAsync(CultureInfo.InvariantCulture);;

        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task BuildAsync_WhenWithOneOfRuleHasBeenCalled_ReturnsValidationRuleSetContainingOneOfRule(StaticTextKey field)
    {
        IValidationRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.WithOneOfRule(field, _fixture.CreateMany<int>(_random!.Next(1, 10)).ToArray()).BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result.ElementAt(0), Is.InstanceOf<IOneOfRule<int>>());
    }

    private IValidationRuleSetBuilder CreateSut()
    {
        return CreateSut(_fixture!, _requiredValueRuleFactoryMock!, _minLengthRuleFactoryMock!, _maxLengthRuleFactoryMock!, _minValueRuleFactoryMock!, _maxValueRuleFactoryMock!, _patternRuleFactoryMock!, _oneOfRuleFactoryMock!);
    }
}