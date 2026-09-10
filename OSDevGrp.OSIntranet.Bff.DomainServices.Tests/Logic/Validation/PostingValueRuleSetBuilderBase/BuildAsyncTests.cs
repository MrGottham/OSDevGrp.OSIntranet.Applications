using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ExtendedValidationRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinValueRuleFactory;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PostingValueRuleSetBuilderBase;

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

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithMinValueRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndMinValue()
    {
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.Debit, minValue: 10D, maxValue: 100D);

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithMinValueRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.Debit),
                It.Is<double>(value => value == 10D)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithMaxValueRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndMaxValue()
    {
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.Debit, minValue: 10D, maxValue: 100D);

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithMaxValueRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.Debit),
                It.Is<double>(value => value == 100D)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnExtendedValidationRuleSetBuilderWithGivenFormatProvider()
    {
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.Debit, minValue: 10D, maxValue: 100D);

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
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.Debit, minValue: 10D, maxValue: 100D);

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
    public async Task BuildAsync_WhenCalled_ReturnsValidationRuleSetBuildedByExtendedValidationRuleSetBuilder()
    {
        IValidationRule[] validationRuleSet =
        [
            _fixture!.CreateMinValueRule<double>(),
            _fixture!.CreateMaxValueRule<double>(),
        ];
        IValidationRuleSetBuilder sut = CreateSut(staticTextKey: StaticTextKey.Credit, minValue: 0D, maxValue: 999D, validationRuleSet: validationRuleSet);

        IReadOnlyCollection<IValidationRule> result = await sut.BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.EqualTo(validationRuleSet));
    }

    private DomainServices.Logic.Validation.PostingValueRuleSetBuilderBase CreateSut(StaticTextKey staticTextKey, double minValue, double maxValue, IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        _extendedValidationRuleSetBuilderMock!.Setup(_fixture!, validationRuleSet: validationRuleSet);

        // Create a concrete implementation for testing the abstract base class
        return new TestablePostingValueRuleSetBuilder(_extendedValidationRuleSetBuilderMock!.Object, staticTextKey, minValue, maxValue);
    }

    private class TestablePostingValueRuleSetBuilder : DomainServices.Logic.Validation.PostingValueRuleSetBuilderBase
    {
        public TestablePostingValueRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder, StaticTextKey staticTextKey, double minValue, double maxValue)
            : base(extendedValidationRuleSetBuilder, staticTextKey, minValue, maxValue)
        {
        }
    }
}