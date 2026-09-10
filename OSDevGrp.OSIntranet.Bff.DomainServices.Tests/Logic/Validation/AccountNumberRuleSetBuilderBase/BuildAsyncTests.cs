using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ExtendedValidationRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PatternRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.AccountNumberRuleSetBuilderBase;

[TestFixture]
public class BuildAsyncTests
{
    #region Private variables

    private Mock<IExtendedValidationRuleSetBuilder>? _extendedValidationRuleSetBuilderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _extendedValidationRuleSetBuilderMock = new Mock<IExtendedValidationRuleSetBuilder>();
        _fixture = new Fixture();
    }

    #region Tests for required=true (Account)

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenRequiredIsTrue_AssertWithRequiredValueRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKey()
    {
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.Account, required: true);

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithRequiredValueRule(It.Is<StaticTextKey>(value => value == StaticTextKey.Account)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenRequiredIsTrue_AssertWithMinLengthRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndAccountNumberMinLength()
    {
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.Account, required: true);

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithMinLengthRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.Account),
                It.Is<int>(value => value == AccountingRuleSetSpecifications.AccountNumberMinLength)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenRequiredIsTrue_AssertWithMaxLengthRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndAccountNumberMaxLength()
    {
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.Account, required: true);

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithMaxLengthRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.Account),
                It.Is<int>(value => value == AccountingRuleSetSpecifications.AccountNumberMaxLength)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenRequiredIsTrue_AssertWithPatternRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndAccountNumberRegexPattern()
    {
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.Account, required: true);

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithPatternRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.Account),
                It.Is<string>(value => value == AccountingRuleSetSpecifications.AccountNumberRegexPattern)),
            Times.Once);
    }

    #endregion

    #region Tests for required=false (BudgetAccount, ContactAccount)

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenRequiredIsFalse_AssertWithRequiredValueRuleWasNotCalledOnExtendedValidationRuleSetBuilder()
    {
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.BudgetAccount, required: false);

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithRequiredValueRule(It.IsAny<StaticTextKey>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenRequiredIsFalse_AssertWithMinLengthRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndAccountNumberMinLength()
    {
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.BudgetAccount, required: false);

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithMinLengthRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.BudgetAccount),
                It.Is<int>(value => value == AccountingRuleSetSpecifications.AccountNumberMinLength)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenRequiredIsFalse_AssertWithMaxLengthRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndAccountNumberMaxLength()
    {
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.BudgetAccount, required: false);

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithMaxLengthRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.BudgetAccount),
                It.Is<int>(value => value == AccountingRuleSetSpecifications.AccountNumberMaxLength)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenRequiredIsFalse_AssertWithPatternRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndAccountNumberRegexPattern()
    {
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.BudgetAccount, required: false);

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithPatternRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.BudgetAccount),
                It.Is<string>(value => value == AccountingRuleSetSpecifications.AccountNumberRegexPattern)),
            Times.Once);
    }

    #endregion

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnExtendedValidationRuleSetBuilderWithGivenFormatProvider()
    {
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.ContactAccount, required: false);

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
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.ContactAccount, required: false);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenRequiredIsTrue_ReturnsValidationRuleSetBuildedByExtendedValidationRuleSetBuilder()
    {
        IValidationRule[] validationRuleSet =
        [
            _fixture!.CreateRequiredValueRule(),
            _fixture!.CreateMinLengthRule(),
            _fixture!.CreateMaxLengthRule(),
            _fixture!.CreatePatternRule(),
        ];
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.Account, required: true, validationRuleSet: validationRuleSet);

        IReadOnlyCollection<IValidationRule> result = await sut.BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.EqualTo(validationRuleSet));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenRequiredIsFalse_ReturnsValidationRuleSetBuildedByExtendedValidationRuleSetBuilder()
    {
        IValidationRule[] validationRuleSet =
        [
            _fixture!.CreateMinLengthRule(),
            _fixture!.CreateMaxLengthRule(),
            _fixture!.CreatePatternRule(),
        ];
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.BudgetAccount, required: false, validationRuleSet: validationRuleSet);

        IReadOnlyCollection<IValidationRule> result = await sut.BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.EqualTo(validationRuleSet));
    }

    private DomainServices.Logic.Validation.AccountNumberRuleSetBuilderBase CreateSut(StaticTextKey staticTextKey, bool required, IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        _extendedValidationRuleSetBuilderMock!.Setup(_fixture!, validationRuleSet: validationRuleSet);

        // Create a concrete implementation for testing the abstract base class
        return new TestableAccountNumberRuleSetBuilder(_extendedValidationRuleSetBuilderMock!.Object, staticTextKey, required);
    }

    private class TestableAccountNumberRuleSetBuilder : DomainServices.Logic.Validation.AccountNumberRuleSetBuilderBase
    {
        public TestableAccountNumberRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder, StaticTextKey staticTextKey, bool required)
            : base(extendedValidationRuleSetBuilder, staticTextKey, required)
        {
        }
    }
}