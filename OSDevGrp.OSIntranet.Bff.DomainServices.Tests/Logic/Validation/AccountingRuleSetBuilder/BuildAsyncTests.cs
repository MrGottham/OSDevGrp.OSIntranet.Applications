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
using System.Globalization;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.AccountingNumberRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.AccountingNameRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.LetterHeadNumberRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.BalanceBelowZeroRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.BackDatingRuleSetBuilder;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.AccountingRuleSetBuilder
;

[TestFixture]
public class BuildAsyncTests
{
    #region Private variables

    private Mock<IExtendedValidationRuleSetBuilder>? _extendedValidationRuleSetBuilderMock;
    private Mock<IAccountingNumberRuleSetBuilder>? _accountingNumberRuleSetBuilderMock;
    private Mock<IAccountingNameRuleSetBuilder>? _accountingNameRuleSetBuilderMock;
    private Mock<ILetterHeadNumberRuleSetBuilder>? _letterHeadNumberRuleSetBuilderMock;
    private Mock<IBalanceBelowZeroRuleSetBuilder>? _balanceBelowZeroRuleSetBuilderMock;
    private Mock<IBackDatingRuleSetBuilder>? _backDatingRuleSetBuilderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _extendedValidationRuleSetBuilderMock = new Mock<IExtendedValidationRuleSetBuilder>();
        _accountingNumberRuleSetBuilderMock = new Mock<IAccountingNumberRuleSetBuilder>();
        _accountingNameRuleSetBuilderMock = new Mock<IAccountingNameRuleSetBuilder>();
        _letterHeadNumberRuleSetBuilderMock = new Mock<ILetterHeadNumberRuleSetBuilder>();
        _balanceBelowZeroRuleSetBuilderMock = new Mock<IBalanceBelowZeroRuleSetBuilder>();
        _backDatingRuleSetBuilderMock = new Mock<IBackDatingRuleSetBuilder>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnAccountingNumberRuleSetBuilderWithGivenFormatProvider()
    {
        IAccountingRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _accountingNumberRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnAccountingNumberRuleSetBuilderWithGivenCancellationToken()
    {
        IAccountingRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _accountingNumberRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnAccountingNameRuleSetBuilderWithGivenFormatProvider()
    {
        IAccountingRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _accountingNameRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnAccountingNameRuleSetBuilderWithGivenCancellationToken()
    {
        IAccountingRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _accountingNameRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnLetterHeadNumberRuleSetBuilderWithGivenFormatProvider()
    {
        IAccountingRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _letterHeadNumberRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnLetterHeadNumberRuleSetBuilderWithGivenCancellationToken()
    {
        IAccountingRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _letterHeadNumberRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnBalanceBelowZeroRuleSetBuilderWithGivenFormatProvider()
    {
        IAccountingRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _balanceBelowZeroRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnBalanceBelowZeroRuleSetBuilderWithGivenCancellationToken()
    {
        IAccountingRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _balanceBelowZeroRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnBackDatingRuleSetBuilderWithGivenFormatProvider()
    {
        IAccountingRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _backDatingRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnBackDatingRuleSetBuilderWithGivenCancellationToken()
    {
        IAccountingRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _backDatingRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsNonEmptyValidationRuleSet()
    {
        IAccountingRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.Not.Empty);
    }

    private IAccountingRuleSetBuilder CreateSut()
    {
        _extendedValidationRuleSetBuilderMock!.Setup(_fixture!);
        _accountingNumberRuleSetBuilderMock!.Setup(_fixture!);
        _accountingNameRuleSetBuilderMock!.Setup(_fixture!);
        _letterHeadNumberRuleSetBuilderMock!.Setup(_fixture!);
        _balanceBelowZeroRuleSetBuilderMock!.Setup(_fixture!);
        _backDatingRuleSetBuilderMock!.Setup(_fixture!);

        return new DomainServices.Logic.Validation.AccountingRuleSetBuilder(_extendedValidationRuleSetBuilderMock!.Object, _accountingNumberRuleSetBuilderMock!.Object, _accountingNameRuleSetBuilderMock!.Object, _letterHeadNumberRuleSetBuilderMock!.Object, _balanceBelowZeroRuleSetBuilderMock!.Object, _backDatingRuleSetBuilderMock!.Object);
    }
}