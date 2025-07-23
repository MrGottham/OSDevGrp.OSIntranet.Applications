using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.OneOfRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ExtendedValidationRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ShouldBeIntegerRuleFactory;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.BalanceBelowZeroRuleSetBuilder
;

[TestFixture]
public class BuildAsyncTests
{
    #region Private variables

    private Mock<IExtendedValidationRuleSetBuilder>? _extendedValidationRuleSetBuilderMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _extendedValidationRuleSetBuilderMock = new Mock<IExtendedValidationRuleSetBuilder>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.Debtors)]
    [TestCase(StaticTextKey.Creditors)]
    public async Task BuildAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithSpecificStaticTextKey(StaticTextKey staticTextKey)
    {
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.Debtors)]
    [TestCase(StaticTextKey.Creditors)]
    public async Task BuildAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithSpecificStaticTextKeyAndNoArguments(StaticTextKey staticTextKey)
    {
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.Is<IEnumerable<object>>(value => value.Any() == false),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.Debtors)]
    [TestCase(StaticTextKey.Creditors)]
    public async Task BuildAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithSpecificStaticTextKeyAndGivenFormatProvider(StaticTextKey staticTextKey)
    {
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.Debtors)]
    [TestCase(StaticTextKey.Creditors)]
    public async Task BuildAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithSpecificStaticTextKeyAndGivenCancellationToken(StaticTextKey staticTextKey)
    {
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithRequiredValueRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyForBalanceBelowZero()
    {
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithRequiredValueRule(It.Is<StaticTextKey>(value => value == StaticTextKey.BalanceBelowZero)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithShouldBeIntegerRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyForBalanceBelowZero()
    {
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithShouldBeIntegerRule(It.Is<StaticTextKey>(value => value == StaticTextKey.BalanceBelowZero)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithOneOfRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyForBalanceBelowZero()
    {
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithOneOfRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.BalanceBelowZero),
                It.IsAny<IValueSpecification<int>[]>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithOneOfRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyForBalanceBelowZeroAndTwoValueSpecifications()
    {
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithOneOfRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.BalanceBelowZero),
                It.Is<IValueSpecification<int>[]>(value => value.Length == 2)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithOneOfRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyForBalanceBelowZeroAndValueSpecificationsForDebtors()
    {
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithOneOfRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.BalanceBelowZero),
                It.Is<IValueSpecification<int>[]>(value => value[0].Value == AccountingRuleSetSpecifications.BalanceBelowZeroDebtorsValue && value[0].Description.StartsWith($"{StaticTextKey.Debtors}:"))),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithOneOfRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyForBalanceBelowZeroAndValueSpecificationsForCreditors()
    {
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithOneOfRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.BalanceBelowZero),
                It.Is<IValueSpecification<int>[]>(value => value[1].Value == AccountingRuleSetSpecifications.BalanceBelowZeroCreditorsValue && value[1].Description.StartsWith($"{StaticTextKey.Creditors}:"))),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnExtendedValidationRuleSetBuilderWithGivenFormatProvider()
    {
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnExtendedValidationRuleSetBuilderWithGivenCancellationToken()
    {
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsValidationRuleSetBuildedByExtendedValidationRuleSetBuilder()
    {
        IValidationRule[] validationRuleSet =
        [
            _fixture!.CreateRequiredValueRule(),
            _fixture!.CreateShouldBeIntegerRule(),
            _fixture!.CreateOneOfRule<int>(),
        ];
        IBalanceBelowZeroRuleSetBuilder sut = CreateSut(validationRuleSet: validationRuleSet);

        IReadOnlyCollection<IValidationRule> result = await sut.BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.EqualTo(validationRuleSet));
    }

    private IBalanceBelowZeroRuleSetBuilder CreateSut(IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        _extendedValidationRuleSetBuilderMock!.Setup(_fixture!, validationRuleSet: validationRuleSet);
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.Validation.BalanceBelowZeroRuleSetBuilder(_extendedValidationRuleSetBuilderMock!.Object, _staticTextProviderMock!.Object);
    }
}