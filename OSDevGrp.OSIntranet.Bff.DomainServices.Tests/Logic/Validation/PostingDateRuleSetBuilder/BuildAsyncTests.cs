using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ExtendedValidationRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PostingDateRuleSetBuilder;

[TestFixture]
public class BuildAsyncTests
{
    #region Private variables

    private Mock<IExtendedValidationRuleSetBuilder>? _extendedValidationRuleSetBuilderMock;
    private Mock<TimeProvider>? _timeProviderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _extendedValidationRuleSetBuilderMock = new Mock<IExtendedValidationRuleSetBuilder>();
        _timeProviderMock = new Mock<TimeProvider>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithRequiredValueRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyForPostingDate()
    {
        IPostingDateRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithRequiredValueRule(It.Is<StaticTextKey>(value => value == StaticTextKey.PostingDate)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithMinValueRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndMinValueForPostingDate()
    {
        DateTimeOffset utcNow = _fixture!.Create<DateTimeOffset>();
        DateTimeOffset utcDate = new DateTimeOffset(utcNow.UtcDateTime.Date, TimeSpan.Zero);
        IPostingDateRuleSetBuilder sut = CreateSut(utcNow: utcNow);

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithMinValueRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.PostingDate),
            It.Is<DateTimeOffset>(value => value == utcDate.AddDays(-AccountingRuleSetSpecifications.BackDatingMaxValue))),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertWithMaxValueRuleWasCalledOnExtendedValidationRuleSetBuilderWithStaticTextKeyAndMaxValueForPostingDate()
    {
        DateTimeOffset utcNow = _fixture!.Create<DateTimeOffset>();
        DateTimeOffset utcDate = new DateTimeOffset(utcNow.UtcDateTime.Date, TimeSpan.Zero);
        IPostingDateRuleSetBuilder sut = CreateSut(utcNow: utcNow);

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _extendedValidationRuleSetBuilderMock!.Verify(m => m.WithMaxValueRule(
                It.Is<StaticTextKey>(value => value == StaticTextKey.PostingDate),
            It.Is<DateTimeOffset>(value => value == utcDate)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnExtendedValidationRuleSetBuilderWithGivenFormatProvider()
    {
        IPostingDateRuleSetBuilder sut = CreateSut();

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
        IPostingDateRuleSetBuilder sut = CreateSut();

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
    public async Task BuildAsync_WhenCalled_AssertGetUtcNowWasCalledOnTimeProvider()
    {
        IPostingDateRuleSetBuilder sut = CreateSut();

        await sut.BuildAsync(CultureInfo.InvariantCulture);

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsValidationRuleSetBuildedByExtendedValidationRuleSetBuilder()
    {
        IValidationRule[] validationRuleSet =
        [
            _fixture!.CreateRequiredValueRule(),
            _fixture!.CreateMinValueRule<DateTimeOffset>(),
            _fixture!.CreateMaxValueRule<DateTimeOffset>(),
        ];
        IPostingDateRuleSetBuilder sut = CreateSut(validationRuleSet: validationRuleSet);

        IReadOnlyCollection<IValidationRule> result = await sut.BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.EqualTo(validationRuleSet));
    }

    private IPostingDateRuleSetBuilder CreateSut(IReadOnlyCollection<IValidationRule>? validationRuleSet = null, DateTimeOffset? utcNow = null)
    {
        _extendedValidationRuleSetBuilderMock!.Setup(_fixture!, validationRuleSet: validationRuleSet);
        _timeProviderMock!.Setup(m => m.GetUtcNow())
            .Returns(utcNow ?? _fixture!.Create<DateTimeOffset>());

        return new DomainServices.Logic.Validation.PostingDateRuleSetBuilder(_extendedValidationRuleSetBuilderMock!.Object, _timeProviderMock!.Object);
    }
}